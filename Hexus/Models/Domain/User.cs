using Microsoft.AspNetCore.Identity;

namespace Hexus.Models.Domain
{
    public class User:IdentityUser
    {
       // public IList<Guid>? ServerIds {  get; set; }
        public string DisplayName { get; set; }
        public IList<Server>? Servers { get; set; }  
        public IList<Role>? Roles { get; set; }
        public IList<Friend>? Friends { get; set; }
        public List<Message> MessagesSent { get; set; }
        public List<Message> MessagesReceived { get; set; }
    }
}
