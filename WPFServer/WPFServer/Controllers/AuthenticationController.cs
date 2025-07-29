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
            if (person == null) return Unauthorized("Invalid UserName");

            var result = await _signInManager.CheckPasswordSignInAsync(person, signinRequest.Password, false);
            if (!result.Succeeded) return Unauthorized("Invalid Password");

            var token = _authenticationRepository.CreateJwt(person);

            return Ok(person.ToPersonDto(token));
        }

        [HttpPost("Signup")]
        public async Task<IActionResult> Signup([FromBody] NewPersonRequest newPersonRequest)
        {
            var person = newPersonRequest.ToPerson();
            var createPerson = await _userManager.CreateAsync(person, newPersonRequest.Password);

            if (createPerson.Succeeded)
            {
                var token = _authenticationRepository.CreateJwt(person);

                await _personsFilesRepository.CreateNewAsync(person.Id);

                return Ok(person.ToPersonDto(token));
            }

            return StatusCode(500, createPerson.Errors);
        }
    }
}
