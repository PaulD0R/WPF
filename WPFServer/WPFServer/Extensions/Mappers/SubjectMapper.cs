using WPFServer.DTOs.Subject;
using WPFServer.Models;

namespace WPFServer.Extensions.Mappers
{
    public static class SubjectMapper
    {
        public static Subject ToSubject(this NewSubjectRequest subjectRequest)
        {
            return new Subject
            {
                Name = subjectRequest.Name ??  string.Empty,
                Year = subjectRequest.Year ?? 0,
                Description = subjectRequest.Description ??  string.Empty,
                Exercises = []
            };
        }

        public static SubjectDto ToSubjectDto(this Subject subject)
        {
            return new SubjectDto
            {
                Id = subject.Id,
                Name = subject.Name,
                Year = subject.Year,
                Description = subject.Description
            };
        }

        public static LiteSubjectDto ToLiteSubjectDto(this Subject subject)
        {
            return new LiteSubjectDto
            {
                Id = subject.Id,
                Name = subject.Name,
                Year = subject.Year
            };
        }
    }
}
