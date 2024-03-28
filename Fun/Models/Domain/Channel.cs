namespace Fun.Models.Domain
{
    public class Channel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }    
        public string Description { get; set; } 
        public ICollection<Role> Roles { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
