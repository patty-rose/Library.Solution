using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Library.Models;

namespace Library.Controllers
{
  public class BooksController : Controller
  {
    private readonly LibraryContext _db;

    private readonly UserManager<ApplicationUser> _userManager;

    public BooksController(UserManager<ApplicationUser> userManager, LibraryContext db)
    {
      _userManager = userManager;
      _db = db;
      //this constructor allows us to set the value of our new _db property to our LibraryContext. This is achievable due to a dependency injection we set up in our AddDbContext method in the ConfigureServices method in our Startup.cs file.
    }

    public async Task<ActionResult> Index()// we can access all our Books in List form by doing the following: _db.Books.ToList()
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;//locate unique identifier for the currently-logged-in user.1. this = BookController itself 2.FindFirst() = locates first record that meets the criteria (FirstOrDefault will succeed even if there is no match..) 3.ClaimTypes.NameIdentifier is the argument to FindFirst(), this locates unique ID with currently logged in account (requires using System.Security.Claims) 4. ? = existential property, only call the property to the right of the ? if the method to the left of the ? doesn't return null. 5. Value -- if successful then the userId is set to the Value property of NameIdentifier.

      var currentUser = await _userManager.FindByIdAsync(userId);//1. await _userManager -- calls on and waits for usermanager service we injected into controller. 2. FindByIdAsync() built-in Identity method to find user's acct by unique id. 3. Provide just located userId.

      var userBooks = _db.Books.Where(entry => entry.User.Id == currentUser.Id).ToList();//stores collection of only thet Book's for the currently logged-in user. 1. Where() is a LINQ method for querying a db. Where accepts an expression to filter results.

      return View(userBooks);
    }

    [HttpPost, ActionName("Index")]
    public ActionResult SaveIsComplete(Book book)
    {
      _db.Entry(book).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Create()
    {
      ViewBag.AuthorId = new SelectList(_db.Authors, "AuthorId", "Name");
      return View();
    }

    //DEPRECATED CREATE POST
    // [HttpPost]
    // public ActionResult Create(Book book)
    // {
    //   _db.Books.Add(book);
    //   _db.SaveChanges();
    //   return RedirectToAction("Index");
    // }

    [HttpPost]
    public async Task<ActionResult> Create(Book book, int AuthorId)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;//find value of currently logged-in user.

      var currentUser = await _userManager.FindByIdAsync(userId);
      book.User = currentUser;//associate current user to Book's User property.

      _db.Books.Add(book);//adds Book to the BooksDbSet
      _db.SaveChanges();//save changes to database object called DB or _db
      if (AuthorId != 0)
      {
        _db.AuthorBook.Add(new AuthorBook() { AuthorId = AuthorId, BookId = book.BookId });//
        _db.SaveChanges();
      }
      return RedirectToAction("Index");//return Index view
    }//Add() is a method we run on our DBSet property of our DBContext, while SaveChanges() is a method we run on the DBContext itself.
    //Together, they update the DBSet and then sync those changes to the database which the DBContext represents.

    public ActionResult Details(int id)//matches "id" object that we created using ActionLink in Views/Books/Index
    {
      var thisBook = _db.Books.Include(book => book.JoinEntities).ThenInclude(join => join.Author).FirstOrDefault(book => book.BookId == id);
      //book is when the bookID = our id argument (LINQ Lambda)
      return View(thisBook);
    }//1. Our _db.Books expression gives us a list of Book objects from the database. However, if we completed the query now (using the FirstOrDefault() method), we'd simply have an Book without its related Authors.2. We need to .Include(book => book.JoinEntities) to load the JoinEntities property of each Book. However, the JoinEntities property on an Book is just a collection of join entities, each of type ICollection<AuthorBook>. These are not the actual authors related to an Book.3.We need the actual Author objects themselves, so we use ThenInclude() method to load the Author of each AuthorBook. Remember that a AuthorBook is simply a reference to a relationship. Each AuthorBook includes the id of an Book as well as the id of a Author. We are actually returning the associated Author of a AuthorBook here.4.Finally, our FirstOrDefault() method specifies which book from the database we're working with.

    //GET action routes to form page for updating book
    public ActionResult Edit(int id)
    {
      var thisBook = _db.Books.FirstOrDefault(book => book.BookId == id);
      ViewBag.AuthorId = new SelectList(_db.Authors, "AuthorId", "Name");
      return View(thisBook); //finding specific book and passing it into view
    }

    [HttpPost]//POST actually updates book
    public ActionResult Edit(Book book, int AuthorId)
    {
      if (AuthorId != 0)
      {
        _db.AuthorBook.Add(new AuthorBook() { AuthorId = AuthorId, BookId = book.BookId });
      }
      _db.Entry(book).State = EntityState.Modified;//We find and update all of the properties of the book we are editing by passing the book (our route parameter) itself into the Entry() method. Then we need to update its State property to EntityState.Modified. This is so Entity knows that the entry has been modified, as it is not explicitly tracking it (we never actually retrieved the book from the database).
      _db.SaveChanges();//once entry's state is marked as modified we can save and then redirect to Index view
      return RedirectToAction("Index");
    }

    public ActionResult AddAuthor(int id)
    {
      var thisBook = _db.Books.FirstOrDefault(book => book.BookId == id);
      ViewBag.AuthorId = new SelectList(_db.Authors, "AuthorId", "Name");
      return View(thisBook);
    }

    [HttpPost]
    public ActionResult AddAuthor(Book book, int AuthorId)
    {
      if (AuthorId != 0)
      {
        _db.AuthorBook.Add(new AuthorBook() { AuthorId = AuthorId, BookId = book.BookId });
        _db.SaveChanges();
      }
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      var thisBook = _db.Books.FirstOrDefault(book => book.BookId == id);
      return View(thisBook);
    }
    //POST action is named DeleteConfirmed instead of Delete since both GET + POST action methods take id as a parameter. C# will not allow us to have two methods with the same signature(method name and parameters). The POST attribute is not considered part of the method signature 

    [HttpPost, ActionName("Delete")] //Note that our annotation includes [ActionName("Delete")]. This is so we can still utilize the proper Delete action even though we've named our method DeleteConfirmed.
    public ActionResult DeleteConfirmed(int id)
    {
      var thisBook = _db.Books.FirstOrDefault(book => book.BookId == id);
      _db.Books.Remove(thisBook);//built in Remove method
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult DeleteAuthor(int joinId)
    {
      var joinEntry = _db.AuthorBook.FirstOrDefault(entry => entry.AuthorBookId == joinId);
      _db.AuthorBook.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }//This route will find the entry in the join table by using the join entry's AuthorBookId. The AuthorBookId is being passed in through the variable joinId in our route's parameter and came from the BeginForm() HTML helper method in our details view.
  }
}