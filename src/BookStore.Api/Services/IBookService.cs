using BookStore.Api.Models;

namespace BookStore.Api.Services;

public interface IBookService
{
    Task<IReadOnlyList<Book>> GetBooksAsync(CancellationToken cancellationToken = default);
    Task<Book?> GetBookAsync(int id, CancellationToken cancellationToken = default);
    Task<Book> CreateBookAsync(Book book, CancellationToken cancellationToken = default);
    Task<bool> UpdateBookAsync(int id, Book updatedBook, CancellationToken cancellationToken = default);
}
