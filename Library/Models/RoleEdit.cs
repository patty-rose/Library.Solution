using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Library.Models
{
    public class RoleEdit//The RoleEdit class is used to represent the Role and the details of the Users who are in the role or not in the role.
    {
        public IdentityRole Role { get; set; }
        public IEnumerable<ApplicationUser> Members { get; set; }
        public IEnumerable<ApplicationUser> NonMembers { get; set; }
    }
}