using WPFServer.DTOs.Exercise;
using WPFServer.DTOs.Subject;
using WPFServer.Exceptions;
using WPFServer.Extensions.Mappers;
using WPFServer.Interfaces.Managers;
using WPFServer.Interfaces.Repositories;
using WPFServer.Interfaces.Services;

namespace WPFServer.Services.Controllers;

public class SubjectService(
    ISubjectRepository subjectRepository,
    ICachingManager  cachingManager)
    :  ISubjectService
{
    public async Task<ICollection<LiteSubjectDto>> GetAllAsync()
    {
        var subjects = await subjectRepository.GetAllAsync();
        return subjects.Select(x => x.ToLiteSubjectDto()).ToList();
    }

    public async Task<SubjectDto> GetByIdAsync(int id)
    {
        var key = $"subject:{id}";
        var subjectDto = await cachingManager.String.GetAsync<SubjectDto>(key);
        if (subjectDto != null) return subjectDto;
        
        var subject = await subjectRepository.GetByIdAsync(id);
        if (subject == null) throw new NotFoundException($"Subject {id} not found");;

        subjectDto = subject.ToSubjectDto();
        await cachingManager.String.SetAsync(key, subjectDto);

        return subjectDto;
    }

    public async Task<SubjectDto> AddAsync(NewSubjectRequest subjectRequest)
    {
        var subject = subjectRequest.ToSubject();

        if (!await subjectRepository.AddAsync(subject)) throw new Exception("Error creating subject");

        await cachingManager.String.SetAsync($"subject:{subject.Id}", subject.ToSubjectDto());
        
        return subject.ToSubjectDto();
    }
}