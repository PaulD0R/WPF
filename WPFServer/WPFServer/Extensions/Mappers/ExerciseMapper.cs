using WPFServer.DTOs.Exercise;
using WPFServer.Models;

namespace WPFServer.Extensions.Mappers
{
    public static class ExerciseMapper
    {
        public static ExerciseDto ToExerciseDto(this Exercise exercise, bool isLiked)
        {
            return new ExerciseDto
            {
                Id = exercise.Id,
                Number = exercise.Number,
                Task = exercise.Task,
                SubjectId = exercise.SubjectId,
                Subject = exercise.Subject?.Name,
                IsLiked = isLiked
            };
        }

        public static ExerciseDto ToExerciseDto(this Exercise exercise, string userId)
        {
            return new ExerciseDto()
            {
                Id = exercise.Id,
                Number = exercise.Number,
                Task = exercise.Task,
                SubjectId = exercise.SubjectId,
                Subject = exercise.Subject?.Name,
                IsLiked = exercise.Persons?.Any(p => p.Id == userId) ?? false
            };
        }

        public static Exercise ToExercise(this ExerciseRequest exerciseRequest)
        {
            return new Exercise
            {
                Number = exerciseRequest.Number,
                Task = exerciseRequest.Task,
                SubjectId = exerciseRequest.SubjectId,
                Comments = []
            };
        }

        public static Exercise ToExercise(this NewExerciseRequest exerciseRequest)
        {
            return new Exercise
            {
                Number = exerciseRequest.Number,
                Task = exerciseRequest.Task,
                SubjectId = exerciseRequest.SubjectId,
                ExercisesFiles = exerciseRequest.Files?.ToExercisesFiles()
            };
        }

        public static FullExerciseDto ToFullExerciseDto(this Exercise exercise, string personId)
        {
            return new FullExerciseDto()
            {
                Id = exercise.Id,
                Number = exercise.Number,
                Task = exercise.Task,
                SubjectId = exercise.SubjectId,
                Subject = exercise.Subject?.ToLightSubjectDto(),
                IsLiked = exercise.Persons?.Any(p => p.Id == personId) ?? false,
                Comments = exercise.Comments?.Select(x => x.ToCommentDto(personId)).ToList()
            };
        }
    }
}
