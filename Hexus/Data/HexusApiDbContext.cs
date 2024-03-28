using Hexus.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Hexus.Data
{
    public class HexusApiDbContext:DbContext
    {
        public HexusApiDbContext(DbContextOptions<HexusApiDbContext> options):base(options)
        {
            
        }

      //  public DbSet<User> Users { get; set; }
      /*  public DbSet<Server> Server { get; set; }
        public DbSet<Role> Roles { get; set; }*/
       // public DbSet<Category> Categories { get; set; } 
       /* public DbSet<Channel> Channels { get; set; }    
        public DbSet<Voice> Voices { get; set; }
        public DbSet<Text> Texts { get; set; }*/
    }
}
