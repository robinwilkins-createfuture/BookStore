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

    public async Task<IReadOnlyList<Book>> GetBooksAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _dbContext.Books
            .AsNoTracking()
            .OrderBy(b => b.Id)
            .ToListAsync(cancellationToken);

        return entities.Select(MapToModel).ToList();
    }

    public async Task<Book?> GetBookAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.Books
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);

        return entity == null ? null : MapToModel(entity);
    }

    public async Task<Book> CreateBookAsync(Book book, CancellationToken cancellationToken = default)
    {
        var bookEntity = MapToEntity(book);
        _dbContext.Books.Add(bookEntity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return MapToModel(bookEntity);
    }

    public async Task<bool> UpdateBookAsync(int id, Book updatedBook, CancellationToken cancellationToken = default)
    {
        var bookEntity = await _dbContext.Books.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
        if (bookEntity == null)
        {
            return false;
        }

        bookEntity.Title = updatedBook.Title;
        bookEntity.Author = updatedBook.Author;
        bookEntity.YearPublished = updatedBook.YearPublished;

        await _dbContext.SaveChangesAsync(cancellationToken);

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
        Title = model.Title,
        Author = model.Author,
        YearPublished = model.YearPublished
    };
}
