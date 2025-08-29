using WPFTest.MVVM.Model.Comments;
using WPFTest.MVVM.Model.Person;

namespace WPFTest.ApiServices.Interfaces
{
    public interface IApiAdminService
    {
        Task<ICollection<Person>> GetUsersAsync();
        Task<ICollection<LiteComment>> GetCommentsAsync(string userId);
        Task<bool> UpdateUserAsync(string userId, UpdatePerson updatePerson);
        Task<bool> ChangeRoleAsync(string userId, string role);
        Task<bool> DeleteUserAsync(string userId);
        Task<bool> DeleteCommentAsync(int commentId);
    }
}
