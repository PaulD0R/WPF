using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using WPFServer.DTOs.Subject;
using WPFServer.Extensions;
using WPFServer.Extensions.Mappers;
using WPFServer.Interfaces;

namespace WPFServer.Controllers
{
    [Route("WPF/Subjects")]
    [EnableCors("AllowAll")]
    [Authorize]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectRepository _subjectRepository;

        public SubjectController(ISubjectRepository subjectRepository)
        {
            _subjectRepository = subjectRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var subjects = await _subjectRepository.GetAllAsync();

            return Ok(subjects.Select(x => x.ToLightSubjectDto()));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var subject = await _subjectRepository.GetByIdAsync(id);
            var personName = User.GetUserName();

            if (subject == null) return NotFound();
            if (personName == null) return Unauthorized();

            return Ok(subject.ToSubjectDto(personName));
        }

        [HttpPost("Add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddSubject([FromBody] NewSubjectRequest subjectRequest)
        {
            var subject = subjectRequest.ToSubject();

            if (!await _subjectRepository.AddAsync(subject)) return BadRequest();

            return Ok(subject);
        }
    }
}
