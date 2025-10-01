using WPFServer.Models;

namespace WPFServer.Interfaces.Repositories
{
    public interface ISubjectRepository
    {
        Task<ICollection<Subject>> GetAllAsync();
        Task<Subject?> GetByIdAsync(int id); 
        Task<bool> AddAsync(Subject subject);
        Task<bool> DeleteAsync(int id);
    }
}
