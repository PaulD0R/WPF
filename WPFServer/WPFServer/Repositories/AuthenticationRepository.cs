using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using WPFServer.Context;
using WPFServer.DTOs.Person;
using WPFServer.Interfaces;
using WPFServer.Models;

namespace WPFServer.Repositories
{
    public class AuthenticationRepository(ApplicationContext context, UserManager<Person> userManager, SignInManager<Person> signInManager, IMemoryCache cache, IPersonsFilesRepository personsFilesRepository, IJwtRepository jwtRepository, IRefreshTokenRepository refreshTokenRepository) : IAuthenticationRepository
    {
        private readonly ApplicationContext _context = context;
        private readonly UserManager<Person> _userManager = userManager;
        private readonly SignInManager<Person> _signInManager = signInManager;
        private readonly IMemoryCache _cache = cache;
        private readonly IPersonsFilesRepository _personsFilesRepository = personsFilesRepository;
        private readonly IJwtRepository _jwtRepository = jwtRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;


        public async Task<Person> Signin(string name, string password)
        {
            var person = await _userManager.Users.Include(x => x.Files)
                .Include(x => x.Exercises).ThenInclude(x => x.Subject).FirstOrDefaultAsync(x => x.UserName == name) 
                ?? throw new Exception("Пользователь не найден");
            var result = await _signInManager.CheckPasswordSignInAsync(person, password, false);

            if (!result.Succeeded) throw new Exception("Неверный пароль");

            _cache.Set(person.Id, new Person
            {
                Id = person.Id,
                UserName = person.UserName,
                Email = person.Email,
            }, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));

            return person;
        }

        public async Task<TokensDto?> SigninWithRefreshToken(string token)
        {
            var refreshToken = await _context.RefreshTokens.Include(x => x.Person)
                .FirstOrDefaultAsync(x => x.Token == token);

            if (refreshToken == null || refreshToken.LiveTime < DateTime.UtcNow) return null;

            var jwtToken = await _jwtRepository.CreateJwtAsync(refreshToken.Person);

            refreshToken.LiveTime = DateTime.UtcNow.AddDays(7);
            refreshToken.Token = _refreshTokenRepository.CreateToken();

            await _context.SaveChangesAsync();

            return new TokensDto
            {
                Jwt = jwtToken,
                RefreshToken = refreshToken.Token
            };
        }

        public async Task<Person?> Signup(Person person, string password)
        {
            var createPerson = await _userManager.CreateAsync(person, password);

            if (createPerson.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(person, "User");

                if (roleResult.Succeeded)
                {
                    await _personsFilesRepository.CreateNewAsync(person.Id);

                    _cache.Set(person.Id, new Person
                    {
                        Id = person.Id,
                        UserName = person.UserName,
                        Email = person.Email,
                    }, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));

                    return person;
                }
            }

            if (createPerson.Errors.Any(e => e.Code == "DuplicateUserName"))
                throw new Exception("Пользователь с таким именем уже существует");

            return null;
        }


    }
}
