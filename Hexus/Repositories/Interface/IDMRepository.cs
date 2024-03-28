using Hexus.Models.Domain;

namespace Hexus.Repositories.Interface
{
    public interface IDMRepository
    {
        Task<List<DM>> GetAllAsync();

        Task<DM?> GetByIdAsync(Guid id);

        Task<DM> CreateAsync(DM dm);

        Task<DM?> UpdateAsync(Guid id, DM dm);

        Task<DM?> DeleteAsync(Guid id);

       
    }
}
