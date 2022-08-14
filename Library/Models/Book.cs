using System.Collections.Generic;
using System;

namespace Library.Models
{
  public class Book
  {

    public Book()
      {
          this.JoinEntities = new HashSet<AuthorBook>();
          this.IsComplete = false;
      }
      
    public int BookId { get; set; }  
    
    public string Title { get; set; }

    public bool IsComplete { get; set; }

    public virtual ICollection<AuthorBook> JoinEntities { get; } 

    public virtual ApplicationUser User { get; set; }
  }
}