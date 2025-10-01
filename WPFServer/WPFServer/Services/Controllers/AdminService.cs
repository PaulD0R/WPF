using WPFServer.DTOs.Comment;
using WPFServer.DTOs.Person;
using WPFServer.Exceptions;
using WPFServer.Extensions.Mappers;
using WPFServer.Interfaces.Managers;
using WPFServer.Interfaces.Repositories;
using WPFServer.Interfaces.Services;

namespace WPFServer.Services.Controllers;

public class AdminService(
    IAdminRepository adminRepository,
    ICachingManager cachingManager)
    : IAdminService
{
    public async Task<ICollection<PrivatePersonDto>> GetAllPersonAsync()
    {
        var persons = await adminRepository.GetAllUsersAsync();
        return persons.Select(p => p.ToPrivatePersonDto()).ToList();
    }

    public async Task<ICollection<LiteCommentDto>> GetCommentsAsync(string id)
    {
        var comments = await adminRepository.GetCommentsByPersonIdAsync(id);
        return comments?.Select(c => c.ToLiteCommentDto()).ToList() ?? 
               throw new NotFoundException("No comments found");
    }

    public async Task<bool> ChangeRoleAsync(string id, RoleRequest role)
    {
        return await adminRepository.ChangeRoleAsync(id, role.Role!) ?
                true : throw new NotFoundException($"Person {id} not found");
    }

    public async Task<LitePersonDto> UpdatePersonAsync(string id, UpdatePersonRequest newPersonRequest)
    {
        var person = await adminRepository.ChangeUserAsync(id, newPersonRequest.ToPerson());
        if (person == null) throw new NotFoundException($"Person {id} not found");
        
        var type = typeof(NewPersonRequest);
        foreach (var state in type.GetFields())
        {
            var value = state.GetValue(person);
            if (value == null) continue;
                
            await cachingManager.Hash.SetFieldAsync($"user:{id}", state.Name, value);
        }

        return person.ToLitePersonDto();
    }

    public async Task<bool> DeletePersonAsync(string id)
    {            
        if (await adminRepository.DeleteUserAsync(id)) 
            throw new NotFoundException($"Person {id} not found");
        
        await cachingManager.Hash.RemoveAsync($"user:{id}");

        return true;
    }

    public async Task<bool> DeleteCommentAsync(int id)
    {
        return await adminRepository.DeleteCommentAsync(id) ?
                true : throw new NotFoundException($"Person {id} not found");;
    }
}