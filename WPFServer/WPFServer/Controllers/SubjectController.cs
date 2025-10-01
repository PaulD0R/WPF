using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using WPFServer.DTOs.Subject;
using WPFServer.Extensions;
using WPFServer.Interfaces.Services;

namespace WPFServer.Controllers
{
    [Route("WPF/Subjects")]
    [EnableCors("AllowAll")]
    [Authorize]
    [ApiController]
    public class SubjectController(
        ISubjectService subjectService,
        IExerciseService exerciseService)
        : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await subjectService.GetAllAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            return Ok(await subjectService.GetByIdAsync(id));
        }

        [HttpGet("{id:int}/exercises")]
        public async Task<IActionResult> GetExercisesBySubjectId([FromRoute] int id)
        {
            var personId = User.GetId();
            if (personId == null) return Unauthorized();
            
            return Ok(await exerciseService.GetExercisesBySubjectAsync(id, personId));
        }

        [HttpPost("Add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddSubject([FromBody] NewSubjectRequest subjectRequest)
        {
            var subject = await subjectService.AddAsync(subjectRequest);
            return CreatedAtAction(nameof(GetById), new {subject.Id}, subject);
        }
    }
}
