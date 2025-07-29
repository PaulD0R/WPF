using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPersonById([FromRoute] string id)
        {
            var person = await _personRepository.GetByIdAsync(id);

            if (person  == null) return NotFound();

            return Ok(person.ToFullPersonDto());
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

            var isLiked = await _personRepository.GetIsLikedById(name, exerciseId);

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
    }
}
