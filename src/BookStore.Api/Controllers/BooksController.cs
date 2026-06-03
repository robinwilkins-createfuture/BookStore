using BookStore.Api.Dtos;
using BookStore.Api.Models;
using BookStore.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<BookDto>), StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<BookDto>> GetBooks()
    {
        var books = _bookService.GetBooks().Select(MapToDto);
        return Ok(books);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(BookDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<BookDto> GetBook(int id)
    {
        var book = _bookService.GetBook(id);
        if (book == null)
        {
            return NotFound();
        }

        return Ok(MapToDto(book));
    }

    [HttpPost]
    [ProducesResponseType(typeof(BookDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<BookDto> CreateBook(BookDto book)
    {
        if (book == null)
        {
            return BadRequest();
        }

        var createdBook = _bookService.CreateBook(MapToModel(book));
        var createdBookDto = MapToDto(createdBook);

        return CreatedAtAction(nameof(GetBook), new { id = createdBookDto.Id }, createdBookDto);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult UpdateBook(int id, BookDto updatedBook)
    {
        var updated = _bookService.UpdateBook(id, MapToModel(updatedBook));
        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }

    private static BookDto MapToDto(Book book) => new()
    {
        Id = book.Id,
        Title = book.Title,
        Author = book.Author,
        YearPublished = book.YearPublished
    };

    private static Book MapToModel(BookDto bookDto) => new()
    {
        Id = bookDto.Id,
        Title = bookDto.Title,
        Author = bookDto.Author,
        YearPublished = bookDto.YearPublished
    };
}
