using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
  public class RoleModification//The RoleModification class code helps update a role. Connected to the UpdateAction in the Role Controller.
  {
    [Required]
    public string RoleName { get; set; }

    public string RoleId { get; set; }

    public string[] AddIds { get; set; }

    public string[] DeleteIds { get; set; }
  }
}