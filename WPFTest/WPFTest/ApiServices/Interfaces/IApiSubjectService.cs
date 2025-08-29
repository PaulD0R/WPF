using WPFTest.Exeptions;
using WPFTest.MVVM.Model.Subject;

namespace WPFTest.ApiServices.Interfaces
{
    public interface IApiSubjectService
    {
        Task<ICollection<LiteSubject>> GetAllAsync();
        Task<FullSubject?> GetByIdAsync(int id);
        Task<bool> AddSubjectAsync(NewSubject subject);
    }
}
