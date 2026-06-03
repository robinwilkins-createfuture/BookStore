using BookStore.Api.Models;

namespace BookStore.Api.Services;

public interface IBookService
{
    IReadOnlyList<Book> GetBooks();
    Book? GetBook(int id);
    Book CreateBook(Book book);
    bool UpdateBook(int id, Book updatedBook);
}
