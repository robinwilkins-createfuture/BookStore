using BookStore.Api.Controllers;
using BookStore.Api.Dtos;
using BookStore.Api.Models;
using BookStore.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace BookStore.Api.Tests.Controllers;

public class BooksControllerTests
{
    private readonly Mock<IBookService> _bookServiceMock;
    private readonly BooksController _controller;

    public BooksControllerTests()
    {
        _bookServiceMock = new Mock<IBookService>();
        _controller = new BooksController(_bookServiceMock.Object);
    }

    [Fact]
    public async Task GetBooks_ReturnsOkResult()
    {
        _bookServiceMock
            .Setup(service => service.GetBooksAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Book>());

        var result = await _controller.GetBooks();

        Assert.IsType<OkObjectResult>(result.Result);
        _bookServiceMock.Verify(service => service.GetBooksAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetBooks_ReturnsAllBooks()
    {
        var expectedBooks = new List<Book>
        {
            new() { Id = 1, Title = "The Pragmatic Programmer", Author = "Andrew Hunt", YearPublished = 1999 },
            new() { Id = 2, Title = "Clean Code", Author = "Robert C. Martin", YearPublished = 2008 }
        };

        _bookServiceMock
            .Setup(service => service.GetBooksAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedBooks);

        var result = await _controller.GetBooks();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var books = Assert.IsAssignableFrom<IEnumerable<BookDto>>(okResult.Value);
        Assert.NotEmpty(books);
        Assert.Equal(2, books.Count());
        _bookServiceMock.Verify(service => service.GetBooksAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetBook_WithValidId_ReturnsOkResult()
    {
        _bookServiceMock
            .Setup(service => service.GetBookAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Book { Id = 1, Title = "The Pragmatic Programmer", Author = "Andrew Hunt", YearPublished = 1999 });

        var result = await _controller.GetBook(1);

        Assert.IsType<OkObjectResult>(result.Result);
        _bookServiceMock.Verify(service => service.GetBookAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetBook_WithValidId_ReturnsCorrectBook()
    {
        var expectedBook = new Book
        {
            Id = 1,
            Title = "The Pragmatic Programmer",
            Author = "Andrew Hunt",
            YearPublished = 1999
        };

        _bookServiceMock
            .Setup(service => service.GetBookAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedBook);

        var result = await _controller.GetBook(1);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var book = Assert.IsType<BookDto>(okResult.Value);
        Assert.Equal(1, book.Id);
        Assert.Equal("The Pragmatic Programmer", book.Title);
        Assert.Equal("Andrew Hunt", book.Author);
        Assert.Equal(1999, book.YearPublished);
        _bookServiceMock.Verify(service => service.GetBookAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetBook_WithInvalidId_ReturnsNotFound()
    {
        _bookServiceMock
            .Setup(service => service.GetBookAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Book?)null);

        var result = await _controller.GetBook(999);

        Assert.IsType<NotFoundResult>(result.Result);
        _bookServiceMock.Verify(service => service.GetBookAsync(999, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateBook_WithValidBook_Returns201Created()
    {
        var newBookDto = new BookDto
        {
            Id = 0,
            Title = "Refactoring",
            Author = "Martin Fowler",
            YearPublished = 1999
        };
        var createdBook = new Book
        {
            Id = 5,
            Title = newBookDto.Title,
            Author = newBookDto.Author,
            YearPublished = newBookDto.YearPublished
        };

        _bookServiceMock
            .Setup(service => service.CreateBookAsync(It.IsAny<Book>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdBook);

        var result = await _controller.CreateBook(newBookDto);

        Assert.IsType<CreatedAtActionResult>(result.Result);
        _bookServiceMock.Verify(
            service => service.CreateBookAsync(It.Is<Book>(b =>
                b.Title == newBookDto.Title &&
                b.Author == newBookDto.Author &&
                b.YearPublished == newBookDto.YearPublished), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task CreateBook_WithValidBook_ReturnsCreatedBookDto()
    {
        var newBookDto = new BookDto
        {
            Id = 0,
            Title = "Refactoring",
            Author = "Martin Fowler",
            YearPublished = 1999
        };
        var createdBook = new Book
        {
            Id = 5,
            Title = newBookDto.Title,
            Author = newBookDto.Author,
            YearPublished = newBookDto.YearPublished
        };

        _bookServiceMock
            .Setup(service => service.CreateBookAsync(It.IsAny<Book>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdBook);

        var result = await _controller.CreateBook(newBookDto);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnedBook = Assert.IsType<BookDto>(createdResult.Value);
        Assert.Equal(5, returnedBook.Id);
        Assert.Equal("Refactoring", returnedBook.Title);
        Assert.Equal("Martin Fowler", returnedBook.Author);
        Assert.Equal(1999, returnedBook.YearPublished);
        Assert.Equal(nameof(BooksController.GetBook), createdResult.ActionName);
        Assert.Equal(5, ((BookDto)createdResult.Value!).Id);
    }

    [Fact]
    public async Task CreateBook_WithNullBook_ReturnsBadRequest()
    {
        var result = await _controller.CreateBook(null!);

        Assert.IsType<BadRequestResult>(result.Result);
        _bookServiceMock.Verify(service => service.CreateBookAsync(It.IsAny<Book>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task UpdateBook_WithValidIdAndBook_Returns204NoContent()
    {
        var updatedBookDto = new BookDto
        {
            Id = 109,
            Title = "The Pragmatic Programmer 20th Anniversary Edition",
            Author = "Andrew Hunt and David Thomas",
            YearPublished = 2019
        };

        _bookServiceMock
            .Setup(service => service.UpdateBookAsync(1, It.IsAny<Book>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var result = await _controller.UpdateBook(1, updatedBookDto);

        Assert.IsType<NoContentResult>(result);
        _bookServiceMock.Verify(
            service => service.UpdateBookAsync(1, It.Is<Book>(b =>
                b.Title == updatedBookDto.Title &&
                b.Author == updatedBookDto.Author &&
                b.YearPublished == updatedBookDto.YearPublished), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task UpdateBook_WithInvalidId_Returns404NotFound()
    {
        var updatedBookDto = new BookDto
        {
            Id = 99,
            Title = "Non-existent",
            Author = "Unknown",
            YearPublished = 2026
        };

        _bookServiceMock
            .Setup(service => service.UpdateBookAsync(999, It.IsAny<Book>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var result = await _controller.UpdateBook(999, updatedBookDto);

        Assert.IsType<NotFoundResult>(result);
        _bookServiceMock.Verify(
            service => service.UpdateBookAsync(999, It.IsAny<Book>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
