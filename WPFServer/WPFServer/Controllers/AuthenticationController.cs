using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WPFServer.DTOs.Person;
using WPFServer.Extensions;
using WPFServer.Extensions.Mappers;
using WPFServer.Interfaces;

namespace WPFServer.Controllers
{
    [ApiController]
    [Route("WPF/Authentication")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly IJwtRepository _jwtRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public AuthenticationController( IAuthenticationRepository authenticationRepository, IJwtRepository jwtRepository, IRefreshTokenRepository refreshTokenRepository)
        {
            _authenticationRepository = authenticationRepository;
            _jwtRepository = jwtRepository;
            _refreshTokenRepository = refreshTokenRepository;
        }

        [HttpPost("Signin")]
        public async Task<IActionResult> Signin([FromBody] SigninRequest signinRequest)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState.Values.First().Errors.First().ErrorMessage);

                var person = await _authenticationRepository.Signin(signinRequest.Name, signinRequest.Password);

                if (person == null) return BadRequest("Пользователь не найден");

                var token = await _jwtRepository.CreateJwtAsync(person);
                var refreshToken = await _refreshTokenRepository.CreateRefreshTokenAsync(person);

                return Ok(new TokensDto
                {
                    Jwt = token,
                    RefreshToken = refreshToken
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Signup")]
        public async Task<IActionResult> Signup([FromBody] NewPersonRequest newPersonRequest)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState.Values.First().Errors.First().ErrorMessage);

                var person = newPersonRequest.ToPerson();
                var createPerson = await _authenticationRepository.Signup(person, newPersonRequest.Password);

                if (createPerson == null) return BadRequest("Неудалось создать полльзователя");

                var token = await _jwtRepository.CreateJwtAsync(person);
                var refreshToken = await _refreshTokenRepository.CreateRefreshTokenAsync(person);

                return Ok(new TokensDto 
                {
                    Jwt = token,
                    RefreshToken = refreshToken
                });

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            if (!ModelState.IsValid) return BadRequest();

            var tokens = await _authenticationRepository.SigninWithRefreshToken(request.Token ?? string.Empty);

            if (tokens == null) return Unauthorized();

            return Ok(tokens);
        }

        [HttpDelete("Logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var id = User.GetId();

            if (id == null) return Unauthorized("Не авторизирован");

            return await _refreshTokenRepository.DeleteRefreshToken(id) ?
                NoContent() : BadRequest();
        }
    }
}
