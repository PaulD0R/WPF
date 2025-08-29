using WPFTest.MVVM.Model.Files;
using WPFTest.MVVM.Model.Person;

namespace WPFTest.ApiServices.Interfaces
{
    public interface IApiPersonService
    {
        Task<PrivatePerson?> GetPrivateAsync();
        Task<FullPerson?> GetPersonByIdAsync(string id);
        Task<FullPerson?> GetPersonByNameAsync(string name);
        Task<bool?> GetIsLickedAsync(int exerciseId);
        Task<ImageNewFile?> ChangePrivateImageAsync(ImageNewFile newImage);
        Task<bool> DeletePrivateImageAsync();
        Task<bool> DeleteCommentAsync(int id);
    }
}
