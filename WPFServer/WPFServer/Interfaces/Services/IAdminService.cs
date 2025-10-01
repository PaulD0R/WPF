using WPFServer.DTOs.Comment;
using WPFServer.DTOs.Person;

namespace WPFServer.Interfaces.Services;

public interface IAdminService
{
    Task<ICollection<PrivatePersonDto>> GetAllPersonAsync();
    Task<ICollection<LiteCommentDto>> GetCommentsAsync(string id);
    Task<bool> ChangeRoleAsync(string id, RoleRequest role);
    Task<LitePersonDto> UpdatePersonAsync(string id, UpdatePersonRequest newPersonRequest);
    Task<bool> DeletePersonAsync(string id);
    Task<bool> DeleteCommentAsync(int id);
}