using Microsoft.AspNetCore.Identity;

namespace Hexus.Models.Domain
{
    public class User:IdentityUser
    {
        public IList<Guid>? ServerIds {  get; set; }
        public IList<Server>? Servers { get; set; }  
    }
}
