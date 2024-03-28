using AutoMapper;
using Hexus.Models.Domain;
using Hexus.Models.DTO;
using Hexus.Repositories.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Hexus.SIgnalR
{
    public class MessageHub:Hub
    {
        private readonly IMessageRepository messageRepository;
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;

        public MessageHub(IMessageRepository messageRepository,
             UserManager<User> userManager,
             IMapper mapper)
        {
            this.messageRepository = messageRepository;
            this.userManager = userManager;
            this.mapper = mapper;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext=Context.GetHttpContext();
            var otherUser = httpContext.Request.Query["user"];
            var groupName = GetGroupName(Context.User?.FindFirst(ClaimTypes.Email)?.Value, otherUser);

            await Groups.AddToGroupAsync(Context.ConnectionId,groupName);

            var messages = await messageRepository
                .GetMessageThread(Context.User?.FindFirst(ClaimTypes.Email)?.Value, otherUser);

            await Clients.Group(groupName).SendAsync("ReceiveMessageThread", messages);

        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(CreateMessageDto createMessageDto)
        {
            var username =Context.User.FindFirst(ClaimTypes.Email)?.Value;
            if (username == null)
            {
                throw new HubException("Username is null");
            }
            //logger.LogInformation(username);

            if (username == createMessageDto.ReceiverUsername.ToLower())
            {
                throw new HubException("You cannot send messages to yourself");

            }

            var sender = await userManager.FindByEmailAsync(username);
            if (sender == null)
            {
                throw new HubException("No sender found!");
            }
            var receiver = await userManager.FindByEmailAsync(createMessageDto.ReceiverUsername);

            if (receiver == null) { throw new HubException("No receiver found"); }

            var message = new Message
            {
                Sender = sender,
                Receiver = receiver,
                SenderUserName = sender.UserName,
                ReceiverUserName = receiver.UserName,
                Content = createMessageDto.Content
            };

            messageRepository.AddMessage(message);

            if (await messageRepository.SaveAllAsync())
            {
                var group = GetGroupName(sender.UserName, receiver.UserName);
                await Clients.Group(group).SendAsync("NewMessage", mapper.Map<MessageDto>(message));
            }
   
        }


        private string GetGroupName(string caller,string other)
        {
            var stringCompare = string.CompareOrdinal(caller, other)<0;

            return stringCompare ? $"{caller}-{other}": $"{other}-{caller}";
        }
    }
}
