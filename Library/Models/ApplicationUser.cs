using Microsoft.AspNetCore.Authorization;//to use [AllowAnonymous] and roles
using Microsoft.AspNetCore.Identity;

namespace Library.Models
{
  [AllowAnonymous]//anonymous users can access
  public class ApplicationUser : IdentityUser
  {

  }
}