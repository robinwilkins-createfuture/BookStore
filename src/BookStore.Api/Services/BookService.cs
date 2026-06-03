using BookStore.Api.Data.Entities;
using BookStore.Api.Models;
using BookStore.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Api.Services;

public class BookService : IBookService
{
    private readonly BookStoreDbContext _dbContext;

    public BookService(BookStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IReadOnlyList<Book> GetBooks()
    {
        return _dbContext.Books
            .AsNoTracking()
            .OrderBy(b => b.Id)
            .Select(MapToModel)
            .ToList();
    }

    public Book? GetBook(int id)
    {
        return _dbContext.Books
            .AsNoTracking()
            .Where(b => b.Id == id)
            .Select(MapToModel)
            .FirstOrDefault();
    }

    public Book CreateBook(Book book)
    {
        var bookEntity = MapToEntity(book);
        _dbContext.Books.Add(bookEntity);
        _dbContext.SaveChanges();

        return MapToModel(bookEntity);
    }

    public bool UpdateBook(int id, Book updatedBook)
    {
        var bookEntity = _dbContext.Books.FirstOrDefault(b => b.Id == id);
        if (bookEntity == null)
        {
            return false;
        }

        bookEntity.Title = updatedBook.Title;
        bookEntity.Author = updatedBook.Author;
        bookEntity.YearPublished = updatedBook.YearPublished;

        _dbContext.SaveChanges();

        return true;
    }

    private static Book MapToModel(BookEntity entity) => new()
    {
        Id = entity.Id,
        Title = entity.Title,
        Author = entity.Author,
        YearPublished = entity.YearPublished
    };

    private static BookEntity MapToEntity(Book model) => new()
    {
        Id = model.Id,
        Title = model.Title,
        Author = model.Author,
        YearPublished = model.YearPublished
    };
}
