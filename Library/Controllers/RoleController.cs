using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Library.Models;

namespace Library.Controllers
{
  public class RoleController : Controller
  {
    private RoleManager<IdentityRole> roleManager;

    private UserManager<ApplicationUser> userManager;


    public RoleController(RoleManager<IdentityRole> roleMgr, UserManager<ApplicationUser> userMgr)
    {
      roleManager = roleMgr;
      userManager = userMgr;
    }
    //add a dependency of RoleManager class to the Constructor in order to get the roles in a variable called roleManager. also add UserManager dependency

    public ViewResult Index() => View(roleManager.Roles);

    private void Errors(IdentityResult result)
    {
      foreach (IdentityError error in result.Errors)
        ModelState.AddModelError("", error.Description);
    }

    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create([Required]string name)//The Create Action takes the Role name as string in it’s parameter and uses the CreateAsync() method to create the Identity Role.
    {
      if (ModelState.IsValid)
      {
        IdentityResult result = await roleManager.CreateAsync(new IdentityRole(name));
        if (result.Succeeded)
          return RedirectToAction("Index");
        else
          Errors(result);
      }
      return View(name);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
      IdentityRole role = await roleManager.FindByIdAsync(id);
      if (role != null)
      {
        IdentityResult result = await roleManager.DeleteAsync(role);
        if (result.Succeeded)
          return RedirectToAction("Index");
        else
          Errors(result);
      }
      else
        ModelState.AddModelError("", "No role found");
      return View("Index", roleManager.Roles);
    }

    //In the following Update Routes I have used the following members of the UserManager class to play with the roles:
    // AddToRoleAsync(AppUser user, string name)	Adds a user to a role
    // RemoveFromRoleAsync(AppUser user, string name)	Removes a user from a role
    // GetRolesAsync(AppUser user)	Gives the names of the roles in which the user is a member
    // IsInRoleAsync(AppUser user, string name)	Returns true is the user is a member of a specified role, else returns false

    public async Task<IActionResult> Update(string id)//The HTTP GET version of the Update Action method is used to fetch members and non-members of a selected Identity Role.
    {
      IdentityRole role = await roleManager.FindByIdAsync(id);
      List<ApplicationUser> members = new List<ApplicationUser>();
      List<ApplicationUser> nonMembers = new List<ApplicationUser>();
      
      foreach (ApplicationUser user in userManager.Users)
      {
        var list = await userManager.IsInRoleAsync(user, role.Name) ? members : nonMembers;
        list.Add(user);
      }
      return View(new RoleEdit
      {
        Role = role,
        Members = members,
        NonMembers = nonMembers
      });
    }

    [HttpPost]
    public async Task<IActionResult> Update(RoleModification model)//the HTTP POST version of the Update Action method is used for adding or removing users from an Identity Role.
    {
      IdentityResult result;
      if (ModelState.IsValid)
      {
        foreach (string userId in model.AddIds ?? new string[] { })
        {
          ApplicationUser user = await userManager.FindByIdAsync(userId);
          if (user != null)
          {
            result = await userManager.AddToRoleAsync(user, model.RoleName);
            if (!result.Succeeded)
            Errors(result);
          }
        }
        foreach (string userId in model.DeleteIds ?? new string[] { })
        {
          ApplicationUser user = await userManager.FindByIdAsync(userId);
          if (user != null)
          {
            result = await userManager.RemoveFromRoleAsync(user, model.RoleName);
            if (!result.Succeeded)
            Errors(result);
          }
        }
      }

      if (ModelState.IsValid)
        return RedirectToAction(nameof(Index));
      else
        return await Update(model.RoleId);
    }
  }
}