using Microsoft.AspNetCore.Authorization;//to use [AllowAnonymous] and roles
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models
{
  [AllowAnonymous]//anonymous users can access
  public class ApplicationUser : IdentityUser
  {
  }
}
//notice-- the AppUser.cs does not contain any property or method. This is because the IdentityUser.cs class provides it with some of the properties like the user name, e-mail, phone number, password hash, role memberships and so on.you can certainly add more properties