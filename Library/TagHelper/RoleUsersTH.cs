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
    private UserManager<ApplicationUser> _userManager;
    private RoleManager<IdentityRole> _roleManager;

    public RoleUsersTH(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
      _userManager = userManager;
      _roleManager = roleManager;
    }

    [HtmlAttributeName("i-role")]
    public string Role {get; set;}

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
      List<string> names = new List<string>();
      IdentityRole role = await _roleManager.FindByIdAsync(Role);
      if (role != null)
      {
        foreach (var user in _userManager.Users)
        {
          if(user != null && await _userManager.IsInRoleAsync(user, role.Name))
          {
            names.Add(user.UserName);
          }
        }
      }//The RoleManager and UserManager objects fetches a list of all the users that resides in a given role.
      
      output.Content.SetContent(names.Count == 0 ? "No Users" : string.Join(", ", names));
    }
  }
} 