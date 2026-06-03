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
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (for PostgreSQL when running with Aspire)

## Running with Aspire (API + PostgreSQL)

Run the AppHost project to start the API and a PostgreSQL container together:

```bash
dotnet run --project src/BookStore.AppHost
```

or using http only:

```bash
dotnet run --project src/BookStore.AppHost --launch-profile http
```

Aspire injects a connection string named `booksdb` into services that reference the database.

## Running the API

```bash
dotnet run
```

The API will be available at `https://localhost:5001` (or `http://localhost:5299`).

An OpenAPI document is served at `/openapi/v1.json` when running in Development mode.

## API Documentation

Interactive API documentation is available via [Scalar](https://scalar.com/) when the application is running in Development mode.

| Resource | URL |
|----------|-----|
| OpenAPI JSON spec | `http://localhost:5299/openapi/v1.json` |
| Scalar UI | `http://localhost:5299/scalar/v1` |

The Scalar UI lets you browse all endpoints, view request/response schemas, and execute requests directly from the browser.

## Running the tests

```bash
dotnet test tests/BookStore.Api.Tests/BookStore.Api.Tests.csproj
```

## CI

A GitHub Actions workflow runs on every push and pull request to `main`, building the project and running the test suite.
