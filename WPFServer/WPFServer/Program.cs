using System.IO.Compression;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.ResponseCompression;
using StackExchange.Redis;
using WPFServer.Context;
using WPFServer.Data;
using WPFServer.ExceptionHandlers;
using WPFServer.Interfaces.Managers;
using WPFServer.Interfaces.Repositories;
using WPFServer.Interfaces.Services;
using WPFServer.Managers;
using WPFServer.Models;
using WPFServer.Repositories;
using WPFServer.Services.Caching;
using WPFServer.Services.Controllers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
});
builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));
builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
{
    var configuration = builder.Configuration.GetConnectionString("RedisConnection");
    return ConnectionMultiplexer.Connect(configuration ?? "localhost");
});
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("RedisConnection");
    options.InstanceName = "WPF";
});
builder.Services.AddIdentity<Person, IdentityRole>( options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;
}).AddEntityFrameworkStores<ApplicationContext>();
builder.Services.AddAuthentication( options =>
{
    options.DefaultAuthenticateScheme =
        options.DefaultChallengeScheme =
            options.DefaultForbidScheme =
                options.DefaultScheme =
                    options.DefaultSignInScheme =
                        options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidIssuer = StaticData.ISSURE,
        ValidateAudience = true,
        ValidAudience = StaticData.AUDIENCE,
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(StaticData.KEY)),
        ValidateIssuerSigningKey = true
    };
});
builder.Services.AddAuthorization().AddControllers();

builder.Services.AddScoped<IDatabase>(sp =>
{
    var connection = sp.GetRequiredService<IConnectionMultiplexer>();
    return connection.GetDatabase();
});

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddScoped<IStringCachingService, StringCachingService>();
builder.Services.AddScoped<IHashCachingService, HashCachingService>();
builder.Services.AddScoped<IListCachingService, ListCachingService>();

builder.Services.AddScoped<ICachingManager, CachingManager>();

builder.Services.AddScoped<IExerciseRepository, ExerciseRepository>();
builder.Services.AddScoped<IExerciseFilesRepository, ExerciseFilesRepository>();
builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<IPersonsFilesRepository, PersonsFilesRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IJwtRepository, JwtRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<ISubjectService, SubjectService>();
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IExerciseService, ExerciseService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader()
              .WithExposedHeaders("Authorization");
    });
});

builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Fastest;
});

builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.WebHost.ConfigureKestrel(options =>
    options.Limits.MaxRequestBodySize = StaticData.MAX_REQUEST_SIZE);

var app = builder.Build();

app.UseStatusCodePages(async statusCodeContext =>
{
    var response = statusCodeContext.HttpContext.Response;
    response.ContentType = "text/plain, charset UTF-8";

    await response.WriteAsync(response.StatusCode switch
    {
        404 => "Not Found",
        400 => "400",
        401 => "401",
        403 => "403",
        _ => "Fail"
    });
});

app.UseExceptionHandler();
app.UseResponseCompression();
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();