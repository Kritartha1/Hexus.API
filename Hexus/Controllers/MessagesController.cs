using AutoMapper;
using Hexus.Extensions;
using Hexus.Helper;
using Hexus.Models.Domain;
using Hexus.Models.DTO;
using Hexus.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace Hexus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IMessageRepository messageRepository;
        private readonly IMapper mapper;
        private readonly ILogger<MessagesController> logger;
        private readonly UserManager<User> userManager;

        public MessagesController(IUserRepository userRepository,
            IMessageRepository messageRepository,
            IMapper mapper,
            ILogger<MessagesController> logger,
            UserManager<User> userManager)
        {
            this.userRepository = userRepository;
            this.messageRepository = messageRepository;
            this.mapper = mapper;
            this.logger = logger;
            this.userManager = userManager;
        }

        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
        {
            var username = User.FindFirst(ClaimTypes.Email)?.Value;
            if (username == null)
            {
                return Unauthorized("No current user found!");
            }
            logger.LogInformation( username);

            if (username == createMessageDto.ReceiverUsername.ToLower())
            {
                return BadRequest("Can't send message to yourself");

            }
            
            var sender = await userManager.FindByEmailAsync(username);
            if (sender == null)
            {
                return NotFound("No sender found!");
            }
            var receiver = await userManager.FindByEmailAsync(createMessageDto.ReceiverUsername);

            if(receiver == null) { return NotFound("No receiver found"); }

            var message = new Message
            {
                Sender = sender,
                Receiver = receiver,
                SenderUserName = sender.UserName,
                ReceiverUserName = receiver.UserName,
                Content = createMessageDto.Content
            };

            messageRepository.AddMessage(message);

            if(await messageRepository.SaveAllAsync()){
                return Ok(mapper.Map<MessageDto>(message)); 
            }

            return BadRequest("Failed to send message");
            

            
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<MessageDto>>> GetMessagesForUser([FromQuery]MessageParams messageParams)
        {
            var name = User.FindFirst(ClaimTypes.Email)?.Value;
            if (name == null)
            {
                return Unauthorized("No current user found!");
            }
            messageParams.Username = name;
            
            

            var messages=await messageRepository.GetMessagesForUser(messageParams);
            Response.AddPaginationHeader(new PaginationHeader(messages.CurrentPage,
                messages.PageSize,messages.TotalCount,messages.TotalPages));
            return Ok(messages);
        }

        [HttpGet("thread/{username}")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageThread(string username)
        {
            var currentUsername= User.FindFirst(ClaimTypes.Email)?.Value;
            if (currentUsername != null)
            {
                return Ok(await messageRepository.GetMessageThread(currentUsername, username));

            }

            return Unauthorized("No current user found!");

        }

        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult> DeleteMessage([FromRoute]Guid id) 
        {
            var username= User.FindFirst(ClaimTypes.Email)?.Value;
            var message = await messageRepository.GetMessage(id);

            if (message.SenderUserName != username && message.ReceiverUserName != username)
            {
                return Unauthorized();
            }

            if (message.SenderUserName == username)
            {
                message.senderDeleted = true;
            }
            if (message.ReceiverUserName == username)
            {
                message.receiverDeleted = true;
            }

            if (message.senderDeleted && message.receiverDeleted)
            {
                messageRepository.DeleteMessage(message);
            }

            if(await messageRepository.SaveAllAsync())
            {
                return Ok();
            }

            return BadRequest("Problem in deleting the message");


        }

    }
}
