using System.ComponentModel.DataAnnotations;

namespace Fun.Models.Domain
{
    public class Friend
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
