using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WPFServer.DTOs.Person;
using WPFServer.Extensions;
using WPFServer.Interfaces.Services;

namespace WPFServer.Controllers
{
    [ApiController]
    [Route("WPF/Authentication")]
    public class AuthenticationController(IAuthenticationService authorizationService) : ControllerBase
    {
        [HttpPost("Signin")]
        public async Task<IActionResult> Signin([FromBody] SigninRequest signinRequest)
        {
            return Ok(await authorizationService.SigninAsync(signinRequest));
        }

        [HttpPost("Signup")]
        public async Task<IActionResult> Signup([FromBody] NewPersonRequest newPersonRequest)
        {
            return Ok(await authorizationService.SignupAsync(newPersonRequest));
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            return Ok(await authorizationService.RefreshTokenAsync(request));
        }

        [HttpDelete("Logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var id = User.GetId();
            if (id == null) return Unauthorized("Не авторизирован");

            await authorizationService.LogoutAsync(id);
            return NoContent();
        }
    }
}
