using BookStore.Api.Controllers;
using BookStore.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace BookStore.Api.Tests.Controllers;

public class BooksControllerTests
{
    private readonly BooksController _controller;

    public BooksControllerTests()
    {
        _controller = new BooksController();
    }

    [Fact]
    public void GetBooks_ReturnsOkResult()
    {
        var result = _controller.GetBooks();

        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public void GetBooks_ReturnsAllBooks()
    {
        var result = _controller.GetBooks();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var books = Assert.IsAssignableFrom<IEnumerable<Book>>(okResult.Value);
        Assert.NotEmpty(books);
    }

    [Fact]
    public void GetBook_WithValidId_ReturnsOkResult()
    {
        var result = _controller.GetBook(1);

        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public void GetBook_WithValidId_ReturnsCorrectBook()
    {
        var result = _controller.GetBook(1);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var book = Assert.IsType<Book>(okResult.Value);
        Assert.Equal(1, book.Id);
        Assert.Equal("The Pragmatic Programmer", book.Title);
        Assert.Equal("Andrew Hunt", book.Author);
        Assert.Equal(1999, book.YearPublished);
    }

    [Fact]
    public void GetBook_WithInvalidId_ReturnsNotFound()
    {
        var result = _controller.GetBook(999);

        Assert.IsType<NotFoundResult>(result.Result);
    }
}
