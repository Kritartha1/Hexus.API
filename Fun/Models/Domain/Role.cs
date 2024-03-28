namespace Fun.Models.Domain
{
    public class Role
    {
        public Guid Id { get; set; }
        public string RoleName { get; set; }
        public bool IsAdmin { get; set; }=false;
        public bool IsModerator { get; set; } = false;
        public bool KickMembers { get; set; } = false;
        public bool ChangeChannelNames { get; set; } = false;

    }
}
