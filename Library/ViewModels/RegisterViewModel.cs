//When we deal with data that only shows up in the view, we can use a ViewModel instead of a Model. This allows us to specify which fields we want to collect from our view. Since we don't need to collect information for all the properties built into ApplicationUser, we'll create a ViewModel that only contains the properties we need.

//ViewModel is reallyjust a grouping of properties and data annotations.

using System.ComponentModel.DataAnnotations;

namespace Library.ViewModels
{
  public class RegisterViewModel
  {
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; }

    // [Required]
    // [Role]
    // [Display(Name = "Account Role")]
    // public string RoleId { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }


  }
}