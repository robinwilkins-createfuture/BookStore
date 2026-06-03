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
    public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks(CancellationToken cancellationToken = default)
    {
        var books = await _bookService.GetBooksAsync(cancellationToken);
        return Ok(books.Select(MapToDto));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(BookDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BookDto>> GetBook(int id, CancellationToken cancellationToken = default)
    {
        var book = await _bookService.GetBookAsync(id, cancellationToken);
        if (book == null)
        {
            return NotFound();
        }

        return Ok(MapToDto(book));
    }

    [HttpPost]
    [ProducesResponseType(typeof(BookDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BookDto>> CreateBook(BookDto book, CancellationToken cancellationToken = default)
    {
        if (book == null)
        {
            return BadRequest();
        }

        var createdBook = await _bookService.CreateBookAsync(MapToModel(book), cancellationToken);
        var createdBookDto = MapToDto(createdBook);

        return CreatedAtAction(nameof(GetBook), new { id = createdBookDto.Id }, createdBookDto);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateBook(int id, BookDto updatedBook, CancellationToken cancellationToken = default)
    {
        var updated = await _bookService.UpdateBookAsync(id, MapToModel(updatedBook), cancellationToken);
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
        Title = bookDto.Title,
        Author = bookDto.Author,
        YearPublished = bookDto.YearPublished
    };
}
