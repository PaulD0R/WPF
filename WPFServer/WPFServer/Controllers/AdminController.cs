using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WPFServer.DTOs.Person;
using WPFServer.Interfaces.Services;

namespace WPFServer.Controllers
{
    [ApiController]
    [Route("WPF/Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController(IAdminService adminService) : ControllerBase
    {
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllPersons()
        {
            return Ok(await adminService.GetAllPersonAsync());
        }

        [HttpGet("{userId}/Comments")]
        public async Task<IActionResult> GetCommentsByUserId([FromRoute] string userId)
        {
            return Ok(await adminService.GetCommentsAsync(userId));
        }

        [HttpPatch("{userId}/ChangeRole")]
        public async Task<IActionResult> ChangeRole([FromRoute] string userId, [FromBody] RoleRequest role)
        {
            await adminService.ChangeRoleAsync(userId, role);
            return NoContent();
        }

        [HttpPatch("{userId}")]
        public async Task<IActionResult> UpdateUser([FromRoute] string userId, [FromBody] UpdatePersonRequest newPerson)
        {
            await adminService.UpdatePersonAsync(userId, newPerson);
            return NoContent();
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeletePerson([FromRoute] string userId)
        {
            await adminService.DeletePersonAsync(userId);
            return NoContent();
        }

        [HttpDelete("Comments/{commentId:int}")]
        public async Task<IActionResult> DeleteComment([FromRoute] int commentId)
        {
            await adminService.DeleteCommentAsync(commentId);
            return NoContent();
        }
    }
}
