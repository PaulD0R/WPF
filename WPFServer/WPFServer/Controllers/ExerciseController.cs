using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using WPFServer.DTOs.Exercise;
using WPFServer.Extensions;
using WPFServer.Extensions.Mappers;
using WPFServer.Interfaces;
using WPFServer.Models;

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

        public ExerciseController(IExercisesRepository exercisesRepository, IPersonRepository personRepository)
        {
            _exercisesRepository = exercisesRepository;
            _personRepository = personRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var personName = User.GetUserName();
            var exercises = await _exercisesRepository.GetAllAsync();

            if (personName == null) return Unauthorized();

            var exercisesDtos = exercises.Select(x => x.ToExerciseDto(personName)).ToList();

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
            var personName = User.GetUserName();

            if (personName == null) return Unauthorized();
            if (exercises == null) return NotFound();

            var exercisesDtos = exercises.Select(x => x.ToExerciseDto(personName)).ToList();

            return Ok(exercisesDtos);
        }

        [HttpGet("{id:int}/File/Task")]
        public async Task<IActionResult> GetTasksFileById([FromRoute] int id)
        {
            var file = await _exercisesRepository.GetTasksFileByIdAsync(id);

            if (file == null) return NotFound();

            return Ok(file);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var exercise = await _exercisesRepository.GetByIdAsync(id);
            var personName = User.GetUserName();

            if (personName == null) return Unauthorized();
            if (exercise == null) return NotFound();

            return Ok(exercise.ToFullExerciseDto(personName));
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] NewExerciseRequest request)
        {
            var exercise = request.ToExercise();

            if (exercise == null) return BadRequest();

            await _exercisesRepository.AddAsync(exercise);

            return Ok();
        }

        [HttpGet("{id:int}/LikesCount")]
        public async Task<IActionResult> GetLikesCount([FromRoute] int id)
        {
            var likesCount = await _exercisesRepository.GetLikesCountByIdAsync(id);

            if (likesCount == null) return NotFound();

            return Ok(new { LikesCount = likesCount });
        }

        [HttpPut("{id:int}/isLiked")]
        public async Task<IActionResult> ChangeIsLicked([FromRoute] int id)
        {
            var personName = User.GetUserName();

            if (personName == null) return NotFound();

            var person = await _personRepository.GetByNameAsync(personName);
            
            if (person == null) return Unauthorized();

            var isLicked = await _exercisesRepository.ChangeIsLikedAsync(person, id);
            return Ok(new { isLiked = isLicked});
        }

    }
}
