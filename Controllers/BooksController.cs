using book_store.Models;
using Microsoft.AspNetCore.Mvc;

namespace book_store.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class BooksController : ControllerBase
{
    static private List<Book> books = new List<Book>
    {
        new() { Id = 1, Title = "The Pragmatic Programmer", Author = "Andrew Hunt", YearPublished = 1999 },
        new() { Id = 2, Title = "Clean Code", Author = "Robert C. Martin", YearPublished = 2008 },
        new() { Id = 3, Title = "Domain-Driven Design", Author = "Eric Evans", YearPublished = 2003 },
        new() { Id = 4, Title = "Lord of the Rings", Author = "J.R.R. Tolkien", YearPublished = 1954 }
    };

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Book>), StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<Book>> GetBooks()
    {
        return Ok(books);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Book), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Book> GetBook(int id)
    {
        var book = books.FirstOrDefault(b => b.Id == id);
        if (book == null)
        {
            return NotFound();
        }
        return Ok(book);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Book), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<Book> CreateBook(Book book)
    {
        if (book == null)
        {
            return BadRequest();
        }
        book.Id = books.Max(b => b.Id) + 1;
        books.Add(book);
        return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult UpdateBook(int id, Book updatedBook)
    {        
        var book = books.FirstOrDefault(b => b.Id == id);
        if (book == null)
        {
            return NotFound();
        }
        book.Title = updatedBook.Title;
        book.Author = updatedBook.Author;
        book.YearPublished = updatedBook.YearPublished;
        return NoContent();
    }
}
