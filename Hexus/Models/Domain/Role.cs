using Microsoft.AspNetCore.Identity;

namespace Hexus.Models.Domain
{
    public class Role:IdentityRole
    {
        public Guid ServerId { get; set; }

    }
}
