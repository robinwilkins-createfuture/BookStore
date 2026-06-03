using BookStore.Api.Data;
using BookStore.Api.Data.Entities;
using BookStore.Api.Models;
using BookStore.Api.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BookStore.Api.Tests.Services;

public class BookServiceTests
{
    private static BookService CreateService(string dbName)
    {
        var options = new DbContextOptionsBuilder<BookStoreDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        var context = new BookStoreDbContext(options);
        context.Books.AddRange(
            new BookEntity { Id = 1, Title = "The Pragmatic Programmer", Author = "Andrew Hunt", YearPublished = 1999 },
            new BookEntity { Id = 2, Title = "Clean Code", Author = "Robert C. Martin", YearPublished = 2008 },
            new BookEntity { Id = 3, Title = "Domain-Driven Design", Author = "Eric Evans", YearPublished = 2003 },
            new BookEntity { Id = 4, Title = "Lord of the Rings", Author = "J.R.R. Tolkien", YearPublished = 1954 }
        );
        context.SaveChanges();

        return new BookService(context);
    }

    [Fact]
    public async Task GetBooks_ReturnsSeededBooks()
    {
        var service = CreateService(nameof(GetBooks_ReturnsSeededBooks));

        var books = await service.GetBooksAsync();

        Assert.NotNull(books);
        Assert.Equal(4, books.Count);
    }

    [Fact]
    public async Task GetBook_WithValidId_ReturnsBook()
    {
        var service = CreateService(nameof(GetBook_WithValidId_ReturnsBook));

        var book = await service.GetBookAsync(1);

        Assert.NotNull(book);
        Assert.Equal(1, book!.Id);
        Assert.Equal("The Pragmatic Programmer", book.Title);
        Assert.Equal("Andrew Hunt", book.Author);
        Assert.Equal(1999, book.YearPublished);
    }

    [Fact]
    public async Task GetBook_WithInvalidId_ReturnsNull()
    {
        var service = CreateService(nameof(GetBook_WithInvalidId_ReturnsNull));

        var book = await service.GetBookAsync(999);

        Assert.Null(book);
    }

    [Fact]
    public async Task CreateBook_AssignsNextId_AndAddsBook()
    {
        var service = CreateService(nameof(CreateBook_AssignsNextId_AndAddsBook));
        var newBook = new Book
        {
            Id = 999,
            Title = "Refactoring",
            Author = "Martin Fowler",
            YearPublished = 1999
        };

        var createdBook = await service.CreateBookAsync(newBook);

        Assert.Equal(5, createdBook.Id);
        var allBooks = await service.GetBooksAsync();
        Assert.Equal(5, allBooks.Count);
        var retrievedBook = await service.GetBookAsync(5);
        Assert.Equal("Refactoring", retrievedBook!.Title);
    }

    [Fact]
    public async Task UpdateBook_WithValidId_UpdatesAndReturnsTrue()
    {
        var service = CreateService(nameof(UpdateBook_WithValidId_UpdatesAndReturnsTrue));
        var updatedBook = new Book
        {
            Id = 10,
            Title = "The Pragmatic Programmer 20th Anniversary Edition",
            Author = "Andrew Hunt and David Thomas",
            YearPublished = 2019
        };

        var updated = await service.UpdateBookAsync(1, updatedBook);

        Assert.True(updated);
        var book = await service.GetBookAsync(1);
        Assert.NotNull(book);
        Assert.Equal(updatedBook.Title, book!.Title);
        Assert.Equal(updatedBook.Author, book.Author);
        Assert.Equal(updatedBook.YearPublished, book.YearPublished);
    }

    [Fact]
    public async Task UpdateBook_WithInvalidId_ReturnsFalse()
    {
        var service = CreateService(nameof(UpdateBook_WithInvalidId_ReturnsFalse));
        var updatedBook = new Book
        {
            Id = 10,
            Title = "Does Not Matter",
            Author = "Unknown",
            YearPublished = 2026
        };

        var updated = await service.UpdateBookAsync(999, updatedBook);

        Assert.False(updated);
    }
}
