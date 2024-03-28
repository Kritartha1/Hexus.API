using Microsoft.AspNetCore.Identity;

namespace Hexus.Models.Domain
{
    public class Server
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<User> Users { get; set; }
        /*public string[] Roles { get; set; }*/
        public IList<Role>? Roles { get; set; }  //user who creates a server gets the admin role
        public IList<Channel>? Channels { get; set; }
        public IList<Category> Categories { get; set; } 

    }
}
