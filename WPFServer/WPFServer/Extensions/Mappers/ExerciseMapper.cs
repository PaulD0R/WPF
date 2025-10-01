using WPFServer.DTOs.Exercise;
using WPFServer.Models;

namespace WPFServer.Extensions.Mappers
{
    public static class ExerciseMapper
    {
        public static ExerciseDto ToExerciseDto(this Exercise exercise)
        {
            return new ExerciseDto
            {
                Id = exercise.Id,
                Number = exercise.Number,
                Task = exercise.Task,
                SubjectId = exercise.SubjectId,
                Subject = exercise.Subject.Name
            };
        }
        
        public static ExerciseDto ToExerciseDto(this Exercise exercise, bool isLiked)
        {
            return new ExerciseDto
            {
                Id = exercise.Id,
                Number = exercise.Number,
                Task = exercise.Task,
                SubjectId = exercise.SubjectId,
                Subject = exercise.Subject.Name,
                IsLiked = isLiked
            };
        }

        public static ExerciseDto ToExerciseDto(this Exercise exercise, string userId)
        {
            return new ExerciseDto
            {
                Id = exercise.Id,
                Number = exercise.Number,
                Task = exercise.Task,
                SubjectId = exercise.SubjectId,
                Subject = exercise.Subject.Name,
                IsLiked = exercise.Persons.Any(p => p.Id == userId)
            };
        }

        public static Exercise ToExercise(this NewExerciseRequest exerciseRequest)
        {
            return new Exercise
            {
                Number = exerciseRequest.Number ?? 0,
                Task = exerciseRequest.Task  ??  string.Empty,
                SubjectId = exerciseRequest.SubjectId,
                ExercisesFiles = exerciseRequest.Files!.ToExercisesFiles()
            };
        }

        public static FullExerciseDto ToFullExerciseDto(this Exercise exercise, string personId)
        {
            return new FullExerciseDto
            {
                Id = exercise.Id,
                Number = exercise.Number,
                Task = exercise.Task,
                SubjectId = exercise.SubjectId,
                Subject = exercise.Subject.ToLiteSubjectDto(),
                IsLiked = exercise.Persons.Any(p => p.Id == personId)
            };
        }
    }
}
