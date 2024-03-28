namespace Hexus.Models.Domain
{
    public class Friend
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public Guid? DMId { get; set; }
        public DM? DM { get; set; }
    }
}
