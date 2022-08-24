using Microsoft.AspNetCore.Authorization;//to use [AllowAnonymous] and roles
using System.Collections.Generic;
using System;

namespace Library.Models
{
  [AllowAnonymous]//anonymous users can access 
  public class Book
  {

    public Book()
      {
          this.JoinEntities = new HashSet<AuthorBook>();
      }
      
    public int BookId { get; set; }  
    
    public string Title { get; set; }

    public virtual ICollection<AuthorBook> JoinEntities { get; } 
    public virtual ICollection<BookUser> JoinBookUser {get;}

    // public virtual ApplicationUser User { get; set; }
  }
}