using Hexus.Models.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Hexus.SIgnalR
{
    [Authorize]
    public class PresenceHub:Hub
    {
        private readonly PresenceTracker tracker;

        public PresenceHub(PresenceTracker tracker)
        {
            this.tracker = tracker;
        }
        public override async Task OnConnectedAsync()
        {
          
           await tracker.UserConnected(Context.User?.FindFirst(ClaimTypes.Email)?.Value, Context.ConnectionId);
           await Clients.Others.SendAsync("UserIsOnline", Context.User?.FindFirst(ClaimTypes.Email)?.Value);

            var currentUsers = await tracker.GetOnlineUsers();
            await Clients.All.SendAsync("GetOnlineUsers", currentUsers);
        }

        /* public async Task SendMessage(string user, string message)
         {
             await Clients.All.SendAsync("ReceiveMessage", user, message);
         }*/
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await tracker.UserDisconnected(Context.User?.FindFirst(ClaimTypes.Email)?.Value, Context.ConnectionId);
            await Clients.Others.SendAsync("UserIsOffline", Context.User?.FindFirst(ClaimTypes.Email)?.Value);

            var currentUsers = await tracker.GetOnlineUsers();
            await Clients.All.SendAsync("GetOnlineUsers", currentUsers);

            await  base.OnDisconnectedAsync(exception);
        }
    }
}
