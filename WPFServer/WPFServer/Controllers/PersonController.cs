using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WPFServer.DTOs.Person;
using WPFServer.DTOs.PersonsFiles;
using WPFServer.Extensions;
using WPFServer.Extensions.Mappers;
using WPFServer.Interfaces;

namespace WPFServer.Controllers
{
    [ApiController]
    [Authorize]
    [Route("WPF/Persons")]
    public class PersonController : ControllerBase
    {
        private readonly IPersonRepository _personRepository;
        private readonly IPersonsFilesRepository _personsFilesRepository;

        public PersonController(IPersonRepository personRepository, IPersonsFilesRepository personsFilesRepository)
        {
            _personRepository = personRepository;
            _personsFilesRepository = personsFilesRepository;
        }

        [HttpGet("Id/{id}")]
        public async Task<IActionResult> GetPersonById([FromRoute] string id)
        {
            var person = await _personRepository.GetByIdAsync(id);

            if (person  == null) return NotFound();

            return Ok(person.ToFullPersonDto());
        }

        [HttpPatch("Id/{id}/AddRole")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddRoleById([FromRoute] string id, [FromBody] RoleRequestcs role)
        {
            var result = await _personRepository.AddRoleByIdAsync(id, role?.Role ?? string.Empty);

            if (!result) return NotFound();

            return Ok();
        }

        [HttpGet("Name/{name}")]
        public async Task<ActionResult> GetByName([FromRoute] string name)
        {
            var privatePersonUserName = User.GetUserName();

            if (privatePersonUserName == null) return Unauthorized();

            var privatePerson = await _personRepository.GetByNameAsync(privatePersonUserName);
            var person = await _personRepository.GetByNameAsync(name);

            if (person == null) return NotFound();

            var personDto = person.ToFullPersonDto();

            foreach (var exercise in personDto.Exercises)
            {
                exercise.IsLiked = await _personRepository.GetIsLikedByIdAsync(privatePerson.Id, exercise.Id);
            }

            return Ok(personDto);
        }

        [HttpGet("Name/{name}/Similar")]
        public async Task<ActionResult> GetByNameSimilar([FromRoute] string name)
        {
            var persons = await _personRepository.GetByNameSimilarsAsync(name);

            return Ok(persons.Select(x => x.ToLightPersonDto()));
        }

        [HttpGet("Me")]
        public async Task<IActionResult> GetPrivatePerson()
        {
            var name = User.GetUserName();

            if (name == null) return Unauthorized();

            var person = await _personRepository.GetByNameAsync(name);

            if (person == null) return NotFound();

            return Ok(person.ToFullPrivatePersonDto());
        }

        [HttpGet("Me/IsLicked/{exerciseId:int}")]
        public async Task<IActionResult> GetIsLiked([FromRoute] int exerciseId)
        {
            var name = User.GetUserName();

            if (name == null) return Unauthorized();

            var isLiked = await _personRepository.GetIsLikedByIdAsync(name, exerciseId);

            if (isLiked == null) return NotFound();

            return Ok(new { IsLiked = isLiked });
        }

        [HttpPut("Me/Image/Change")]
        public async Task<IActionResult> ChangePrivateImage([FromBody] NewPersonsImageRequest newImage)
        {
            var name = User.GetUserName();

            if (name == null) return Unauthorized();

            var person = await _personRepository.GetByNameAsync(name);

            if (person == null) return NotFound();
            if (newImage.Image == null) return BadRequest();

            await _personsFilesRepository.ChangeImageAsync(person.Id, newImage.Image);

            return Ok(newImage);
        }

        [HttpDelete("Me/Image/Delete")]
        public async Task<IActionResult> DeletePrivateImage()
        {
            var name = User.GetUserName();

            if (name == null) return Unauthorized();

            var person = await _personRepository.GetByNameAsync(name);

            if (person == null) return NotFound();

            await _personsFilesRepository.DeleteImageAsync(person.Id);

            return Ok();
        }

        [HttpGet("Me/Comments")]
        public async Task<IActionResult> GetComments()
        {
            var userName = User.GetUserName();

            if (userName == null) return Unauthorized();

            var comments = await _personRepository.GetCommentsByNameAsync(userName);
            return Ok(comments?.Select(x => x.ToCommentDto()));
        }

        [HttpDelete("Me/Comments/Delete/{commentId:int}")]
        public async Task<IActionResult> DeleteComment([FromRoute] int commentId)
        {
            var userName = User.GetUserName();

            if (userName == null) return Unauthorized();
            if (!await _personRepository.DeleteCommentByIdAsync(userName, commentId)) return BadRequest();

            return NoContent();

        }
    }
}
