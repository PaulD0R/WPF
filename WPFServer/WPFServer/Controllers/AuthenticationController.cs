using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WPFServer.DTOs.Person;
using WPFServer.Extensions.Mappers;
using WPFServer.Interfaces;
using WPFServer.Models;

namespace WPFServer.Controllers
{
    [ApiController]
    [Route("WPF/Authentication")]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<Person> _userManager;
        private readonly SignInManager<Person> _signInManager;
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly IPersonsFilesRepository _personsFilesRepository;

        public AuthenticationController(UserManager<Person> userManager, SignInManager<Person> signInManager, IAuthenticationRepository authenticationRepository, IPersonsFilesRepository personsFilesRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authenticationRepository = authenticationRepository;
            _personsFilesRepository = personsFilesRepository;
        }

        [HttpPost("Signin")]
        public async Task<IActionResult> Signin([FromBody] SigninRequest signinRequest)
        {
            if (!ModelState.IsValid) return BadRequest();

            var person = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == signinRequest.Name);
            if (person == null) return Unauthorized("Пользователь не существует");

            var result = await _signInManager.CheckPasswordSignInAsync(person, signinRequest.Password, false);
            if (!result.Succeeded) return Unauthorized("Неверный пароль");

            var token = await _authenticationRepository.CreateJwtAsync(person);

            return Ok(person.ToPersonDto(token));
        }

        [HttpPost("Signup")]
        public async Task<IActionResult> Signup([FromBody] NewPersonRequest newPersonRequest)
        {
            if (!ModelState.IsValid) return BadRequest("Некорректный запрос");

            var person = newPersonRequest.ToPerson();
            var createPerson = await _userManager.CreateAsync(person, newPersonRequest.Password);

            if (createPerson.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(person, "User");

                if (roleResult.Succeeded)
                {
                    var token = await _authenticationRepository.CreateJwtAsync(person);

                    await _personsFilesRepository.CreateNewAsync(person.Id);

                    return Ok(person.ToPersonDto(token));
                }
            }

            if (createPerson.Errors.Any(e => e.Code == "DuplicateUserName"))
                return Conflict("Пользователь с таким именем уже существует");

            return StatusCode(500);
        }
    }
}
