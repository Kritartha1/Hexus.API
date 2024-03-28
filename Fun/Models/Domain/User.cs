namespace Fun.Models.Domain
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public ICollection<Server> Servers { get; set; }
        public ICollection<Friend> Friends { get; set; }
        public ICollection<Role> Roles { get; set; }    

    }
}
