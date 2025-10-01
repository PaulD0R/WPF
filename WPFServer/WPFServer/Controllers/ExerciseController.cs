using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using WPFServer.DTOs.Comment;
using WPFServer.DTOs.Exercise;
using WPFServer.Extensions;
using WPFServer.Interfaces.Services;

namespace WPFServer.Controllers
{
    [Route("WPF/Exercises")]
    [EnableCors("AllowAll")]
    [Authorize]
    [ApiController]
    public class ExerciseController(
        IExerciseService  exerciseService,
        ICommentService  commentService)
        : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var personId = User.GetId();
            if (personId == null) return Unauthorized("Не авторизирован");
            
            return Ok(await exerciseService.GetAllAsync(personId));
        }

        [HttpGet("Count")]
        public async Task<IActionResult> Count()
        {
            return Ok(await exerciseService.CountAsync());
        }

        [HttpGet("Page{page:int}")]
        public async Task<IActionResult> GetByPage([FromRoute] int page)
        {
            var personId = User.GetId();
            if (personId == null) return Unauthorized("Не авторизирован");

            return Ok(await exerciseService.GetByPageAsync(page, personId));
        }

        [HttpGet("{id:int}/File/Task")]
        public async Task<IActionResult> GetTasksFileById([FromRoute] int id)
        {
            return Ok(await exerciseService.GetTaskAsync(id));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var personId = User.GetId();
            if (personId == null) return Unauthorized("Не авторизирован");

            return Ok(await exerciseService.GetByIdAsync(id, personId));
        }

        [HttpPost("Add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add([FromBody] NewExerciseRequest request)
        {
            var exercise = await exerciseService.AddAsync(request);
            return CreatedAtAction(nameof(GetById), new { exercise.Id }, exercise);
        }

        [HttpGet("{id:int}/LikesCount")]
        public async Task<IActionResult> GetLikesCount([FromRoute] int id)
        {
            return Ok(await exerciseService.LikesCountAsync(id));
        }

        [HttpPut("{id:int}/isLiked")]
        public async Task<IActionResult> ChangeIsLicked([FromRoute] int id)
        {
            var personId = User.GetId();
            if (personId == null) return Unauthorized("Не авторизирован");

            await exerciseService.SwitchIsLikedAsync(id, personId);
            return NoContent();
        }

        [HttpPost("{id:int}/Comments/Add")]
        public async Task<IActionResult> AddComment([FromRoute] int id, [FromBody] CommentRequest request)
        {
            var personId = User.GetId();
            if (personId == null) return Unauthorized("Не авторизирован");
            
            return CreatedAtAction(nameof(GetAll),
                await commentService.AddCommentAsync(id, personId, request));
        }

        [HttpGet("{id:int}/Comments")]
        public async Task<IActionResult> GetComments([FromRoute] int id)
        {
            var personId = User.GetId();
            if (personId == null) return Unauthorized("Не авторизирован");

            return Ok(await commentService.GetCommentsByExerciseIdAsync(id, personId));
        }

        [HttpGet("{id:int}/Comments/Person")]
        public async Task<IActionResult> GetPersonComment([FromRoute] int id)
        {
            var personId = User.GetId();
            if (personId == null) return Unauthorized("Не авторизирован");

            return Ok(await commentService.GetPersonCommentsByExerciseIdAsync(id, personId));
        }
    }
}
