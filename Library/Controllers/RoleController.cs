using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Library.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace Library.Controllers
{
  [AllowAnonymous]
  public class RoleController : Controller
  {
    private RoleManager<IdentityRole> _roleManager;

    private UserManager<ApplicationUser> _userManager;

    private readonly ILogger<RoleController> _logger;

    public RoleController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ILogger<RoleController> logger)
    {
      _roleManager = roleManager;
      _userManager = userManager;
      _logger = logger;
    }
    //add a dependency of _roleManager class to the Constructor in order to get the roles in a variable called _roleManager. also add _userManager dependency

    public IActionResult Index()
    {
      return View(_roleManager.Roles);
    } 

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
        IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));
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
      IdentityRole role = await _roleManager.FindByIdAsync(id);
      if (role != null)
      {
        IdentityResult result = await _roleManager.DeleteAsync(role);
        if (result.Succeeded)
        {
          return RedirectToAction("Index");
        }
        else
        {
          Errors(result);
        }
      }
      else
        ModelState.AddModelError("", "No role found");
      return View("Index", _roleManager.Roles);
    }

    //In the following Update Routes I have used the following members of the _userManager class to play with the roles:
    // AddToRoleAsync(AppUser user, string name)	Adds a user to a role
    // RemoveFromRoleAsync(AppUser user, string name)	Removes a user from a role
    // GetRolesAsync(AppUser user)	Gives the names of the roles in which the user is a member
    // IsInRoleAsync(AppUser user, string name)	Returns true is the user is a member of a specified role, else returns false

    public async Task<IActionResult> Update(string id)//The HTTP GET version of the Update Action method is used to fetch members and non-members of a selected Identity Role.
    {
      IdentityRole role = await _roleManager.FindByIdAsync(id);

      List<ApplicationUser> members = new List<ApplicationUser>();

      List<ApplicationUser> nonMembers = new List<ApplicationUser>();

      List<ApplicationUser> users = _userManager.Users.ToList();

      foreach (ApplicationUser user in users)
      {
        var list = await _userManager.IsInRoleAsync(user, role.Name) ? members : nonMembers;
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
      if(ModelState.IsValid)
      {
        foreach(string userId in model.AddIds ?? new string[] {})
        {
          ApplicationUser user = await _userManager.FindByIdAsync(userId);
          if(user != null)
          {
            result = await _userManager.AddToRoleAsync(user, model.RoleName);
            if(!result.Succeeded)
            {
              Errors(result);
            }
          }
        }
        foreach(string userId in model.DeleteIds ?? new string[] {})
        {
          ApplicationUser user = await _userManager.FindByIdAsync(userId);
          if(user != null)
          {
            result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);
            if(!result.Succeeded)
            {
              Errors(result);
            }
          }
        }
      }
      if(ModelState.IsValid)
      {
        return RedirectToAction("Index");
      }
      else
      {
        return await Update(model.RoleId);
      }
    }
  }
}