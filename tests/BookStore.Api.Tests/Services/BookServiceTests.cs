using BookStore.Api.Models;
using BookStore.Api.Services;
using Xunit;

namespace BookStore.Api.Tests.Services;

public class BookServiceTests
{
    [Fact]
    public void GetBooks_ReturnsSeededBooks()
    {
        var service = new BookService();

        var books = service.GetBooks();

        Assert.NotNull(books);
        Assert.Equal(4, books.Count);
    }

    [Fact]
    public void GetBook_WithValidId_ReturnsBook()
    {
        var service = new BookService();

        var book = service.GetBook(1);

        Assert.NotNull(book);
        Assert.Equal(1, book!.Id);
        Assert.Equal("The Pragmatic Programmer", book.Title);
        Assert.Equal("Andrew Hunt", book.Author);
        Assert.Equal(1999, book.YearPublished);
    }

    [Fact]
    public void GetBook_WithInvalidId_ReturnsNull()
    {
        var service = new BookService();

        var book = service.GetBook(999);

        Assert.Null(book);
    }

    [Fact]
    public void CreateBook_AssignsNextId_AndAddsBook()
    {
        var service = new BookService();
        var newBook = new Book
        {
            Title = "Refactoring",
            Author = "Martin Fowler",
            YearPublished = 1999
        };

        var createdBook = service.CreateBook(newBook);

        Assert.Equal(5, createdBook.Id);
        Assert.Equal(5, service.GetBooks().Count);
        Assert.Equal("Refactoring", service.GetBook(5)!.Title);
    }

    [Fact]
    public void UpdateBook_WithValidId_UpdatesAndReturnsTrue()
    {
        var service = new BookService();
        var updatedBook = new Book
        {
            Title = "The Pragmatic Programmer 20th Anniversary Edition",
            Author = "Andrew Hunt and David Thomas",
            YearPublished = 2019
        };

        var updated = service.UpdateBook(1, updatedBook);

        Assert.True(updated);
        var book = service.GetBook(1);
        Assert.NotNull(book);
        Assert.Equal(updatedBook.Title, book!.Title);
        Assert.Equal(updatedBook.Author, book.Author);
        Assert.Equal(updatedBook.YearPublished, book.YearPublished);
    }

    [Fact]
    public void UpdateBook_WithInvalidId_ReturnsFalse()
    {
        var service = new BookService();
        var updatedBook = new Book
        {
            Title = "Does Not Matter",
            Author = "Unknown",
            YearPublished = 2026
        };

        var updated = service.UpdateBook(999, updatedBook);

        Assert.False(updated);
    }
}
