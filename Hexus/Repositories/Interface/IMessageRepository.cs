using Hexus.Helper;
using Hexus.Models.Domain;
using Hexus.Models.DTO;

namespace Hexus.Repositories.Interface
{
    public interface IMessageRepository
    {
        /*Task<List<DM>> GetAllAsync();

        Task<DM?> GetByIdAsync(Guid id);

        Task<DM> CreateAsync(DM dm);

        Task<DM?> UpdateAsync(Guid id, DM dm);

        Task<DM?> DeleteAsync(Guid id);*/

        void AddMessage(Message message);
        void DeleteMessage(Message message);
        Task<Message?> GetMessage(Guid id);
        Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams);
        Task<IEnumerable<MessageDto>> GetMessageThread(string currentUserName, string receiverName);
        Task<bool> SaveAllAsync();

    }
}
