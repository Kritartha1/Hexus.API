using Hexus.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hexus.Data
{
    public class HexusApiAuthDbContext:IdentityDbContext<User>
    {
        public HexusApiAuthDbContext(DbContextOptions<HexusApiAuthDbContext> options):base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Server> Server { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<Voice> Voices { get; set; }
        public DbSet<Text> Texts { get; set; }

        public DbSet<Friend> Friends { get; set; }
        public DbSet<DM> DMs { get; set; }

        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var userRoleId = "b62bc63a-78f6-4fc3-b2ab-16b0244a222d";
            var superAdminRoleId = "10538139-dbb7-40be-9119-5c2297e16e10";
            var adminRoleId = "987c7f4b-3a86-4c16-a2cf-a3f172b63175";
            var moderatorRoleId = "b4e76a9c-e602-436c-a655-c028ff59e537";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id=userRoleId,
                    ConcurrencyStamp=userRoleId,
                    Name="User",
                    NormalizedName="User".ToUpper()
                },

                new IdentityRole
                {
                    Id=adminRoleId,
                    ConcurrencyStamp =adminRoleId,
                    Name="Admin",
                    NormalizedName="Admin".ToUpper()
                },
                 new IdentityRole
                {
                    Id=superAdminRoleId,
                    ConcurrencyStamp =superAdminRoleId,
                    Name="SuperAdmin",
                    NormalizedName="SuperAdmin".ToUpper()
                },
                  new IdentityRole
                {
                    Id=moderatorRoleId,
                    ConcurrencyStamp =moderatorRoleId,
                    Name="Moderator",
                    NormalizedName="Moderator".ToUpper()
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);

            builder.Entity<Message>()
                .HasOne(u => u.Receiver)
                .WithMany(m => m.MessagesReceived)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
                .HasOne(u => u.Sender)
                .WithMany(m => m.MessagesSent)
                .OnDelete(DeleteBehavior.Restrict);



        }

    }
}
