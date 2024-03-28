using Hexus.Models.Domain;

namespace Hexus.Models.DTO
{
    public class UserDto
    {
        public string Id { get; set; }
        public IList<Server>? Servers { get; set; }
        public IList<Role>? Roles { get; set; }
        public IList<DM>? DMs { get; set; }
    }
}
