using AutoMapper;
using AutoMapper.QueryableExtensions;
using Hexus.Data;
using Hexus.Helper;
using Hexus.Models.Domain;
using Hexus.Models.DTO;
using Hexus.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Hexus.Repositories.Implementation
{
    public class MessageRepository : IMessageRepository
    {
        private readonly HexusApiAuthDbContext dbContext;
        private readonly IMapper mapper;

        public MessageRepository(HexusApiAuthDbContext dbContext,
            IMapper mapper) {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }
        public void AddMessage(Message message)
        {
            dbContext.Messages.Add(message);    
        }

        public void DeleteMessage(Message message)
        {
            dbContext.Messages.Remove(message);
        }

        public async Task<Message?> GetMessage(Guid id)
        {
            return await dbContext.Messages.FindAsync(id);
        }


        /*Task<PagedList<MessageDto>> GetMessagesForUser()
        {
            throw new NotImplementedException();
        }*/

        public async Task<IEnumerable<MessageDto>> GetMessageThread(string currentUserName, string receiverUserName)
        {
            var messages = await dbContext.Messages
                .Include(u => u.Sender)
                .Include(u => u.Receiver)
                .Where(
                m => m.ReceiverUserName == currentUserName &&m.receiverDeleted==false &&
                m.SenderUserName == receiverUserName ||
                (m.ReceiverUserName == receiverUserName &&m.senderDeleted==false &&
                m.SenderUserName == currentUserName)
                ).
                OrderByDescending(m => m.MessageSent)
                .ToListAsync();

            var unreadMessages = messages.Where(m => m.DateRead == null &&
            m.ReceiverUserName == currentUserName).ToList();

            if (unreadMessages.Any())
            {
                foreach(var message in unreadMessages)
                {
                    message.DateRead = DateTime.UtcNow;
                }

                await dbContext.SaveChangesAsync();
            }

            return mapper.Map<IEnumerable<MessageDto>>(messages); 
        }

        public async Task<bool> SaveAllAsync()
        {
            return await dbContext.SaveChangesAsync() > 0;
            
        }

        public async Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams)
        {
            var query = dbContext.Messages.OrderByDescending(x => x.MessageSent)
                 .AsQueryable();
            query = messageParams.Container switch
            {
                "Inbox" => query.Where(u => u.ReceiverUserName == messageParams.Username &&u.receiverDeleted==false),
                "Outbox" => query.Where(u => u.SenderUserName == messageParams.Username &&u.senderDeleted==false),
                _ => query.Where(u => u.ReceiverUserName == messageParams.Username &&u.receiverDeleted==false
                && u.DateRead == null)
            };

            var messages = query.ProjectTo<MessageDto>(mapper.ConfigurationProvider);

            return await PagedList<MessageDto>.CreateAsync(messages,
                messageParams.PageNumber, messageParams.PageSize);
        }
    }
}
