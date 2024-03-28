using Fun.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Fun.Data

{
    public class FunApiDbContext:DbContext
    {
        public FunApiDbContext(DbContextOptions<FunApiDbContext> options) : base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Server> Servers { get; set; }  
        public DbSet<Role> Roles { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<Channel> Channels { get; set; }
    }

    
}
