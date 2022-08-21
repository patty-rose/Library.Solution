using Library.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.TagHelpers
{
  [HtmlTargetElement("td", Attributes = "i-role")]
  public class RoleUsersTH : TagHelper
  {
    private UserManager<ApplicationUser> userManager;
    private RoleManager<IdentityRole> roleManager;

    public RoleUsersTH(UserManager<ApplicationUser> usermgr, RoleManager<IdentityRole> rolemrg)
    {
        userManager = usermgr;
        roleManager = rolemrg;
    }

    [HtmlAttributeName("i-role")]
    public string Role { get; set; }//This Custom Tag Helper operates on the td elements having an i-role attribute. This attribute is used to receive the id of the role that is being processed.

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
      List<string> names = new List<string>();
      IdentityRole role = await roleManager.FindByIdAsync(Role);
      if (role != null)
      {
        foreach (var user in userManager.Users)
        {
            if (user != null && await userManager.IsInRoleAsync(user, role.Name))
                names.Add(user.UserName);
        }
      }//The RoleManager and UserManager objects fetches a list of all the users that resides in a given role.

      output.Content.SetContent(names.Count == 0 ? "No Users" : string.Join(", ", names));
    }
  }
}