using WPFServer.DTOs.Person;
using WPFServer.DTOs.PersonsFiles;
using WPFServer.Exceptions;
using WPFServer.Extensions.Mappers;
using WPFServer.Interfaces.Managers;
using WPFServer.Interfaces.Repositories;
using WPFServer.Interfaces.Services;

namespace WPFServer.Services.Controllers;

public class PersonService(
    IPersonRepository personRepository,
    IPersonsFilesRepository personsFilesRepository,
    ICachingManager  cachingManager)
    : IPersonService
{
    public async Task<FullPersonDto> GetByIdAsync(string id)
    {
        var key = $"user:{id}";
        var personDto = await cachingManager.Hash.GetAsync<FullPersonDto>(key);
        if (personDto != null) return personDto;
            
        var person = await personRepository.GetByIdAsync(id);
        if (person == null) throw new NotFoundException($"Person {id} not found");;
                
        await cachingManager.Hash.SetAsync(key, person.ToFullPrivatePersonDto());
        
        return person.ToFullPersonDto();
    } 

    public async Task<FullPersonDto> GetByNameAsync(string name)
    {
        var person = await personRepository.GetByNameAsync(name);
        return person?.ToFullPersonDto() ??
               throw new NotFoundException($"Person {name} not found");;
    }

    public async Task<FullPrivatePersonDto> GetMeAsync(string id)
    {
        var key = $"user:{id}";
        var personDto = await cachingManager.Hash
            .GetAsync<FullPrivatePersonDto>(key);
        if (personDto != null) return personDto;
            
        var person = await personRepository.GetByIdAsync(id);
        if (person == null) throw new NotFoundException($"Person {id} not found");;
                
        await cachingManager.Hash.SetAsync(key, person.ToFullPrivatePersonDto());
                
        return person.ToFullPrivatePersonDto();
    }

    public async Task<bool> IsLikedAsync(string id, int exerciseId)
    {
        return await personRepository.GetIsLikedByIdAsync(id, exerciseId) ?? 
               throw new NotFoundException($"Person {id} not found");
    }

    public async Task<bool> ChangeImageAsync(string id, NewPersonsImageRequest? newImageRequest = null)
    {
        if (await personsFilesRepository.ChangeImageAsync(id, newImageRequest?.Image ?? []) == null)
            throw new NotFoundException($"Person {id} not found");
            
        await cachingManager.Hash.SetFieldAsync($"user:{id}", "Image", newImageRequest?.Image ?? []);
        return true;
    }
}