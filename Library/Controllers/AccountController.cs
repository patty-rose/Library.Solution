using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;//allows for async tasks
using Library.Models;
using Library.ViewModels;//Different from models or views..

namespace Library.Controllers
{
  public class AccountController : Controller
  {
    private readonly LibraryContext _db;

    private readonly UserManager<ApplicationUser> _userManager;//helps manage saving and updating user account information.

    private readonly 
    SignInManager<ApplicationUser> _signInManager;//provides functionality for users to log into their accounts

    //We have private preferences for _userManager and _signInManager. We'll use dependency injection in the AccountController constructor to configure these services for us.

    //constructor:
    public AccountController (UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, LibraryContext db)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _db = db;
    }

    //routes:
    public ActionResult Index()
    {
        return View();
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Register (RegisterViewModel model) //async so returns a Task<Ar>. Task class is built-in and represents async actions that haven't been completed yet.
    {
      var user = new ApplicationUser { UserName = model.Email };

      //IR is set to await because CreateAsync() is an async action. CA() takes a user object with all user info (email, name, etc.) and a password that will be encrypted when added to DB.
      IdentityResult result = await _userManager.CreateAsync(user, model.Password);//CreateAsync is from Identity's UserManager service which we injected in the controller parameters and constructor. This method will create a user in our app and our database with the provided password. Returning a new IdentityResult  object(this class represents the result of an Identity-driven async action.) called result. 

      if (result.Succeeded)
      {
          return RedirectToAction("Index");
      }
      else
      {
          return View();
      }
    }

    public ActionResult Login()
    {
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Login(LoginViewModel model)
    {
      Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: true, lockoutOnFailure: false);//signInManager has been injected in AccountController parameters and constructor. Which includes PasswordSignInAsyn(userName, password, isPersistent, lockoutOnFailure). We set IP and LOOF explicitly since we are not currently concerned with them.

      if (result.Succeeded)
      {
        return RedirectToAction("Index");
      }
      else//this ensures our program doesn't freeze or break if authentication isn't successful.
      {
        return View();
      }
    }

    [HttpPost]
    public async Task<ActionResult> LogOff()
    {
      await _signInManager.SignOutAsync();
      return RedirectToAction("Index");
    }

  }
}