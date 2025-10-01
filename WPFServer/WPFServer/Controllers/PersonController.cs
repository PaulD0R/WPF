using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WPFServer.DTOs.PersonsFiles;
using WPFServer.Extensions;
using WPFServer.Interfaces.Services;

namespace WPFServer.Controllers
{
    [ApiController]
    [Authorize]
    [Route("WPF/Persons")]
    public class PersonController(
        IPersonService  personService,
        ICommentService commentService)
        : ControllerBase
    {
        [HttpGet("Id/{id}")]
        public async Task<IActionResult> GetPersonById([FromRoute] string id)
        {
            return Ok(await personService.GetByIdAsync(id));
        }

        [HttpGet("Id/{id}/Exercises")]
        public async Task<IActionResult> GetExercisesById([FromRoute] string id)
        {
            return Ok(await commentService.GetCommentsByPersonIdAsync(id));
        }

        [HttpGet("Name/{name}")]
        public async Task<ActionResult> GetByName([FromRoute] string name)
        {
            return Ok(await personService.GetByNameAsync(name));
        }

        [HttpGet("Me")]
        public async Task<IActionResult> GetPrivatePerson()
        {
            var id = User.GetId();
            if (id == null) return Unauthorized("Не авторизирован");

            return Ok(await personService.GetMeAsync(id));
        }

        [HttpGet("Me/IsLiked/{exerciseId:int}")]
        public async Task<IActionResult> GetIsLiked([FromRoute] int exerciseId)
        {
            var id = User.GetId();
            if (id == null) return Unauthorized("Не авторизирован");

            return Ok(new { IsLiked = await personService.IsLikedAsync(id, exerciseId) });
        }

        [HttpPut("Me/Image/Change")]
        public async Task<IActionResult> ChangePrivateImage([FromBody] NewPersonsImageRequest newImage)
        {
            var id = User.GetId();
            if (id == null) return Unauthorized("Не авторизирован");

            await personService.ChangeImageAsync(id, newImage);
            return NoContent();
        }

        [HttpDelete("Me/Image/Delete")]
        public async Task<IActionResult> DeletePrivateImage()
        {
            var id = User.GetId();
            if (id == null) return Unauthorized("Не авторизирован");

            await personService.ChangeImageAsync(id);
            return NoContent();
        }

        [HttpGet("Me/Comments")]
        public async Task<IActionResult> GetComments()
        {
            var id = User.GetId();
            if (id == null) return Unauthorized("Не авторизирован");

            return Ok(await commentService.GetCommentsByPersonIdAsync(id));
        }

        [HttpDelete("Me/Comments/Delete/{commentId:int}")]
        public async Task<IActionResult> DeleteComment([FromRoute] int commentId)
        {
            var id = User.GetId();
            if (id == null) return Unauthorized("Не авторизирован");
            
            await commentService.DeleteCommentAsync(commentId, id);
            return NoContent();
        }
    }
}
