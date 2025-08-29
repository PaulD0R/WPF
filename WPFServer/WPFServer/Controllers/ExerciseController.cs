using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using WPFServer.DTOs.Comment;
using WPFServer.DTOs.Exercise;
using WPFServer.Extensions;
using WPFServer.Extensions.Mappers;
using WPFServer.Interfaces;

namespace WPFServer.Controllers
{
    [Route("WPF/Exercises")]
    [EnableCors("AllowAll")]
    [Authorize]
    [ApiController]
    public class ExerciseController : ControllerBase
    {
        private readonly IExercisesRepository _exercisesRepository;
        private readonly IPersonRepository _personRepository;
        private readonly ICommentRepository _commentRepository;

        public ExerciseController(IExercisesRepository exercisesRepository, IPersonRepository personRepository, ICommentRepository commentRepository)
        {
            _exercisesRepository = exercisesRepository;
            _personRepository = personRepository;
            _commentRepository = commentRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var personId = User.GetId();
            var exercises = await _exercisesRepository.GetAllAsync();

            if (personId == null) return Unauthorized("Не авторизирован");

            var exercisesDtos = exercises.Select(x => x.ToExerciseDto(personId)).ToList();

            return Ok(exercisesDtos);
        }

        [HttpGet("Count")]
        public async Task<IActionResult> Count()
        {
            var count = await _exercisesRepository.GetLengthAsync();
            return Ok(count);
        }

        [HttpGet("Page{page:int}")]
        public async Task<IActionResult> GetByPage([FromRoute] int page)
        {
            var exercises = await _exercisesRepository.GetByPageAsync(page);
            var personId = User.GetId();

            if (personId == null) return Unauthorized("Не авторизирован");
            if (exercises == null) return NotFound("Упражнение не найдено");

            var exercisesDtos = exercises.Select(x => x.ToExerciseDto(personId)).ToList();

            return Ok(exercisesDtos);
        }

        [HttpGet("{id:int}/File/Task")]
        public async Task<IActionResult> GetTasksFileById([FromRoute] int id)
        {
            var file = await _exercisesRepository.GetTasksFileByIdAsync(id);

            if (file == null) return NotFound("Некорректный запрос");

            return Ok(file);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var exercise = await _exercisesRepository.GetByIdAsync(id);
            var personId = User.GetId();

            if (personId == null) return Unauthorized("Не авторизирован");
            if (exercise == null) return NotFound("Упражнение не найдено");

            return Ok(exercise.ToFullExerciseDto(personId));
        }

        [HttpPost("Add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add([FromBody] NewExerciseRequest request)
        {
            var exercise = request.ToExercise();

            if (exercise == null) return BadRequest("Некорректный запрос");

            await _exercisesRepository.AddAsync(exercise);

            return Ok();
        }

        [HttpGet("{id:int}/LikesCount")]
        public async Task<IActionResult> GetLikesCount([FromRoute] int id)
        {
            var likesCount = await _exercisesRepository.GetLikesCountByIdAsync(id);

            if (likesCount == null) return NotFound("Упражнение не найдено");

            return Ok(new { LikesCount = likesCount });
        }

        [HttpPut("{id:int}/isLiked")]
        public async Task<IActionResult> ChangeIsLicked([FromRoute] int id)
        {
            var personId = User.GetId();

            if (personId == null) return Unauthorized("Не авторизирован");

            var isLicked = await _exercisesRepository.ChangeIsLikedAsync(personId, id);
            return Ok(new { isLiked = isLicked});
        }

        [HttpPost("{id:int}/Comments/Add")]
        public async Task<IActionResult> AddComment([FromRoute] int id, [FromBody] CommentRequest request)
        {
            var exercise = await _exercisesRepository.GetByIdAsync(id);
            var userId = User.GetId();

            if (userId == null) return Unauthorized("Не авторизирован");
            if (exercise == null) return NotFound("Упражнение не найдено");

            var person = await _personRepository.GetByIdAsync(userId);

            if (person == null) return NotFound("Пользователь не найден");

            var comment = request.ToComment(person, id);

            await _commentRepository.AddAsync(comment);
            return Ok();
        }

        [HttpGet("{id:int}/Comments")]
        public async Task<IActionResult> GetComments([FromRoute] int id)
        {
            var personId = User.GetId();

            if (personId == null) return Unauthorized("Не авторизирован");

            var comments = await _commentRepository.GetCommentsByExerciseIdAsync(id);

            if (comments == null) return NotFound("Комментарий не найден");

            return Ok(comments.Select(x => x.ToCommentDto(personId)));
        }

        [HttpGet("{id:int}/Comments/Person")]
        public async Task<IActionResult> GetPersonComment([FromRoute] int id)
        {
            var personId = User.GetId();

            if (personId == null) return Unauthorized("Не авторизирован");

            var comments = await _commentRepository.GetPersonCommentsByExerciseId(id, personId);

            if (comments == null) return NotFound("Коментарий не найден");

            return Ok(comments.Select(x => x.ToCommentDto(personId)));
        }
    }
}
