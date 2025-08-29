using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WPFServer.DTOs.Person;
using WPFServer.Extensions.Mappers;
using WPFServer.Interfaces;

namespace WPFServer.Controllers
{
    [ApiController]
    [Route("WPF/Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository _adminRepository;
        public AdminController(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllPersons()
        {
            var persons = await _adminRepository.GetAllUsersAsync();
            return Ok(persons.Select(x => x.ToPrivatePersonDto()));
        }

        [HttpGet("{userId}/Comments")]
        public async Task<IActionResult> GetCommentsByUserId([FromRoute] string userId)
        {
            var comments = await _adminRepository.GetCommentsByPersonIdAsync(userId);

            return Ok(comments?.Select(x => x.ToLightCommentDto()));
        }

        [HttpPut("{userId}/ChangeRole")]
        public async Task<IActionResult> ChangeRole([FromRoute] string userId, [FromBody] RoleRequestcs role)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.First().Errors.First().ErrorMessage);

            if (await _adminRepository.ChangeRoleAsync(userId, role.Role)) return Ok();

            return BadRequest("Ошибка смены роли");
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser([FromRoute] string userId, [FromBody] UpdatePersonRequest newPerson)
        {
            var person = await _adminRepository.ChangeUserAsync(userId, newPerson.ToPerson());

            return Ok(person?.ToLightPersonDto());
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeletePerson([FromRoute] string userId)
        {
            if (await _adminRepository.DeleteUserAsync(userId)) return NoContent();

            return BadRequest("Ошибка удаления");
        }

        [HttpDelete("Comments/{commentId:int}")]
        public async Task<IActionResult> DeleteComment([FromRoute] int commentId)
        {
            if (await _adminRepository.DeleteCommentAsync(commentId)) return NoContent();

            return BadRequest("Ошибка удаления");
        }
    }
}
