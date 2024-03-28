namespace Fun.Models.Domain
{
    public class Server
    {
        public Guid Id { get; set; }
        public string ServerName { get; set; }
        public bool IsAdmin { get; set; } //true only for the creator of the server. Before the creator leaves, he can choose the next admin.
        public ICollection<User> Users { get; set; }
        public ICollection<Role> Roles { get; set; }
        public ICollection<Channel> Channels { get; set; }
    
    }
}
