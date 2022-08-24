using Microsoft.AspNetCore.Authorization;//to use [AllowAnonymous] and roles
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Library.Models
{
  public class ApplicationUser : IdentityUser
  {
    public virtual Book Book {get; set;}
    public virtual ICollection<BookUser> JoinBookUser {get;}
  }

  // public class ApplicationUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
  // {
  //   public ApplicationUserClaimsPrincipalFactory(
  //     UserManager<ApplicationUser> userManager,
  //     RoleManager<IdentityRole> roleManager,
  //     IOptions<IdentityOptions> options
  //     ) : base(userManager, roleManager, options)
  //   {

  //   }

  //   protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
  //   {
  //     var identity = await base.GenerateClaimsAsync(user);

  //     identity.AddClaim(new Claim("Email",
  //       user.Email
  //       ));

  //     return identity;
  //   }
  // }
}
//notice-- the AppUser.cs does not contain any property or method. This is because the IdentityUser.cs class provides it with some of the properties like the user name, e-mail, phone number, password hash, role memberships and so on.you can certainly add more properties