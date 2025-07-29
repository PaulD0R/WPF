using WPFServer.DTOs.Subject;
using WPFServer.Models;

namespace WPFServer.Extensions.Mappers
{
    public static class SubjectMapper
    {
        public static Subject ToSubject(this NewSubjectRequest subjectRequest)
        {
            return new Subject()
            {
                Name = subjectRequest.Name,
                Year = subjectRequest.Year,
                Description = subjectRequest.Description,
                Exercises = new List<Exercise>()
            };
        }

        public static SubjectDto ToSubjectDto(this Subject subject, string userName)
        {
            return new SubjectDto()
            {
                Id = subject.Id,
                Name = subject.Name,
                Year = subject.Year,
                Description = subject.Description,
                Exercises = subject.Exercises?.Select(x => x.ToExerciseDto(userName)).ToList()
            };
        }

        public static LightSubjectDto ToLightSubjectDto(this Subject subject)
        {
            return new LightSubjectDto()
            {
                Id = subject.Id,
                Name = subject.Name,
                Year = subject.Year
            };
        }
    }
}
