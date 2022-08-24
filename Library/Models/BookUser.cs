using System;
using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
  public class BookUser
  {
    public int BookUserId {get; set;}
    public int BookId {get; set;}

    public string UserId {get; set;}

    [DisplayFormat(DataFormatString = "{0:MMMM dd, yyyy}")]
    public DateTime CheckoutDate { get; set; }

    public virtual ApplicationUser User {get; set;}
    public virtual Book Book {get; set;}
  }
}