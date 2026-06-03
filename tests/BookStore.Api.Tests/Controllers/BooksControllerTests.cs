using BookStore.Api.Controllers;
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
    public void GetBooks_ReturnsOkResult()
    {
        _bookServiceMock
            .Setup(service => service.GetBooks())
            .Returns(new List<Book>());

        var result = _controller.GetBooks();

        Assert.IsType<OkObjectResult>(result.Result);
        _bookServiceMock.Verify(service => service.GetBooks(), Times.Once);
    }

    [Fact]
    public void GetBooks_ReturnsAllBooks()
    {
        var expectedBooks = new List<Book>
        {
            new() { Id = 1, Title = "The Pragmatic Programmer", Author = "Andrew Hunt", YearPublished = 1999 },
            new() { Id = 2, Title = "Clean Code", Author = "Robert C. Martin", YearPublished = 2008 }
        };

        _bookServiceMock
            .Setup(service => service.GetBooks())
            .Returns(expectedBooks);

        var result = _controller.GetBooks();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var books = Assert.IsAssignableFrom<IEnumerable<Book>>(okResult.Value);
        Assert.NotEmpty(books);
        Assert.Equal(2, books.Count());
        _bookServiceMock.Verify(service => service.GetBooks(), Times.Once);
    }

    [Fact]
    public void GetBook_WithValidId_ReturnsOkResult()
    {
        _bookServiceMock
            .Setup(service => service.GetBook(1))
            .Returns(new Book { Id = 1, Title = "The Pragmatic Programmer", Author = "Andrew Hunt", YearPublished = 1999 });

        var result = _controller.GetBook(1);

        Assert.IsType<OkObjectResult>(result.Result);
        _bookServiceMock.Verify(service => service.GetBook(1), Times.Once);
    }

    [Fact]
    public void GetBook_WithValidId_ReturnsCorrectBook()
    {
        var expectedBook = new Book
        {
            Id = 1,
            Title = "The Pragmatic Programmer",
            Author = "Andrew Hunt",
            YearPublished = 1999
        };

        _bookServiceMock
            .Setup(service => service.GetBook(1))
            .Returns(expectedBook);

        var result = _controller.GetBook(1);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var book = Assert.IsType<Book>(okResult.Value);
        Assert.Equal(1, book.Id);
        Assert.Equal("The Pragmatic Programmer", book.Title);
        Assert.Equal("Andrew Hunt", book.Author);
        Assert.Equal(1999, book.YearPublished);
        _bookServiceMock.Verify(service => service.GetBook(1), Times.Once);
    }

    [Fact]
    public void GetBook_WithInvalidId_ReturnsNotFound()
    {
        _bookServiceMock
            .Setup(service => service.GetBook(999))
            .Returns((Book?)null);

        var result = _controller.GetBook(999);

        Assert.IsType<NotFoundResult>(result.Result);
        _bookServiceMock.Verify(service => service.GetBook(999), Times.Once);
    }
}
