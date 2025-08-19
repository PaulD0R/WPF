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

            if (person  == null) return NotFound("Пользователь не найден");

            var personDto = person.ToFullPersonDto();

            foreach (var exercise in personDto.Exercises)
            {
                exercise.IsLiked = await _personRepository.GetIsLikedByIdAsync(id, exercise.Id);
            }

            return Ok(personDto);
        }

        [HttpPatch("Id/{id}/AddRole")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddRoleById([FromRoute] string id, [FromBody] RoleRequestcs role)
        {
            var result = await _personRepository.AddRoleByIdAsync(id, role?.Role ?? string.Empty);

            if (!result) return NotFound("Пользователь не найден");

            return Ok();
        }

        [HttpGet("Name/{name}")]
        public async Task<ActionResult> GetByName([FromRoute] string name)
        {
            var id = User.GetId();
            var person = await _personRepository.GetByNameAsync(name);

            if (id == null) return Unauthorized("Не авторизирован");
            if (person == null) return NotFound("Пользователь не найден");

            var personDto = person.ToFullPersonDto();

            foreach (var exercise in personDto.Exercises)
            {
                exercise.IsLiked = await _personRepository.GetIsLikedByIdAsync(id, exercise.Id);
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
            var id = User.GetId();

            if (id == null) return Unauthorized("Не авторизирован");

            var person = await _personRepository.GetByIdAsync(id);

            if (person == null) return NotFound("Пользователь не найден");

            return Ok(person.ToFullPrivatePersonDto());
        }

        [HttpGet("Me/IsLicked/{exerciseId:int}")]
        public async Task<IActionResult> GetIsLiked([FromRoute] int exerciseId)
        {
            var id = User.GetId();

            if (id == null) return Unauthorized("Не авторизирован");

            var isLiked = await _personRepository.GetIsLikedByIdAsync(id, exerciseId);

            if (isLiked == null) return NotFound("Пользователь не найден");

            return Ok(new { IsLiked = isLiked });
        }

        [HttpPut("Me/Image/Change")]
        public async Task<IActionResult> ChangePrivateImage([FromBody] NewPersonsImageRequest newImage)
        {
            var id = User.GetId();

            if (id == null) return Unauthorized("Не авторизирован");
            if (newImage.Image == null) return BadRequest("Некорректное изображение");

            await _personsFilesRepository.ChangeImageAsync(id, newImage.Image);

            return Ok(newImage);
        }

        [HttpDelete("Me/Image/Delete")]
        public async Task<IActionResult> DeletePrivateImage()
        {
            var id = User.GetId();

            if (id == null) return Unauthorized("Не авторизирован");

            await _personsFilesRepository.DeleteImageAsync(id);

            return NoContent();
        }

        [HttpGet("Me/Comments")]
        public async Task<IActionResult> GetComments()
        {
            var id = User.GetId();

            if (id == null) return Unauthorized("Не авторизирован");

            var comments = await _personRepository.GetCommentsByIdAsync(id);
            return Ok(comments?.Select(x => x.ToCommentDto()));
        }

        [HttpDelete("Me/Comments/Delete/{commentId:int}")]
        public async Task<IActionResult> DeleteComment([FromRoute] int commentId)
        {
            var id = User.GetId();

            if (id == null) return Unauthorized("Не авторизирован");
            if (!await _personRepository.DeleteCommentByIdAsync(id, commentId)) return BadRequest("Ошибка");

            return NoContent();
        }
    }
}
