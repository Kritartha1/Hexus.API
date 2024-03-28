using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Hexus.Models.Domain
{
    public class Role
    {
        /* public string role { get; set; }
         public Guid ServerId { get; set; }*/

        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
        public Guid ServerId { get; set; }
        // Navigation property
        public Server Server { get; set; }
        public IList<User>? Users { get; set; }


    }
}
