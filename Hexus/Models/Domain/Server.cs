using Microsoft.AspNetCore.Identity;

namespace Hexus.Models.Domain
{
    public class Server
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<User> Users { get; set; }
        public IList<IdentityRole>

    }
}
