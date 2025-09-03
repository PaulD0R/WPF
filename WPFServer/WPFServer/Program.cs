using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WPFServer.Context;
using WPFServer.Data;
using WPFServer.Interfaces;
using WPFServer.Models;
using WPFServer.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddMemoryCache();
builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseMySQL(StaticData.CONNECTION_STRING));

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

builder.Services.AddScoped<IExercisesRepository, ExercisesRepository>();
builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
builder.Services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<IPersonsFilesRepository, PersonsFilesRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IJwtRepository, JwtRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();

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
        404 => "Не найдено",
        400 => "Неверный запрос",
        401 => "Не авторизован",
        403 => "Доступ запрещен",
        _ => "Ошибка"
    });
});

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
    dbContext.Database.Migrate();
}

app.Run();