using WPFServer.DTOs.Subject;

namespace WPFServer.Interfaces.Services;

public interface ISubjectService
{
    Task<ICollection<LiteSubjectDto>> GetAllAsync();
    Task<SubjectDto> GetByIdAsync(int id);
    Task<SubjectDto> AddAsync(NewSubjectRequest subject);
}