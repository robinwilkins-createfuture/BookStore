using BookStore.Api.Models;

namespace BookStore.Api.Services;

public class BookService : IBookService
{
    private readonly List<Book> _books =
    [
        new() { Id = 1, Title = "The Pragmatic Programmer", Author = "Andrew Hunt", YearPublished = 1999 },
        new() { Id = 2, Title = "Clean Code", Author = "Robert C. Martin", YearPublished = 2008 },
        new() { Id = 3, Title = "Domain-Driven Design", Author = "Eric Evans", YearPublished = 2003 },
        new() { Id = 4, Title = "Lord of the Rings", Author = "J.R.R. Tolkien", YearPublished = 1954 }
    ];

public IReadOnlyList<Book> GetBooks()
{
    lock (_books)
    {
        return _books.ToArray();
    }
}

public Book? GetBook(int id)
{
    lock (_books)
    {
        return _books.FirstOrDefault(b => b.Id == id);
    }
}

public Book CreateBook(Book book)
{
    lock (_books)
    {
        var nextId = _books.Count == 0 ? 1 : _books.Max(b => b.Id) + 1;
        book.Id = nextId;
        _books.Add(book);

        return book;
    }
}

public bool UpdateBook(int id, Book updatedBook)
{
    lock (_books)
    {
        var book = _books.FirstOrDefault(b => b.Id == id);
        if (book is null)
        {
            return false;
        }

        book.Title = updatedBook.Title;
        book.Author = updatedBook.Author;
        book.YearPublished = updatedBook.YearPublished;

        return true;
    }
}
}
