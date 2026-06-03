# Book Store API

A RESTful Web API built with ASP.NET Core 9 for managing a collection of books.

## Endpoints

| Method | Route | Description |
|--------|-------|-------------|
| GET | `/api/books` | Get all books |
| GET | `/api/books/{id}` | Get a book by ID |
| POST | `/api/books` | Create a new book |
| PUT | `/api/books/{id}` | Update an existing book |

### Book object

```json
{
  "id": 1,
  "title": "The Pragmatic Programmer",
  "author": "Andrew Hunt",
  "yearPublished": 1999
}
```

## Requirements

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)

## Running the API

```bash
dotnet run
```

The API will be available at `https://localhost:5001` (or `http://localhost:5000`).

An OpenAPI document is served at `/openapi/v1.json` when running in Development mode.

## Running the tests

```bash
dotnet test book-store.Tests/book-store.Tests.csproj
```

## CI

A GitHub Actions workflow runs on every push and pull request to `main`, building the project and running the test suite.
