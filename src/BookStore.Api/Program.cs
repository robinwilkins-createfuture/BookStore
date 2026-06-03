using Scalar.AspNetCore;
using BookStore.Api.Data;
using BookStore.Api.Data.Entities;
using BookStore.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.AddServiceDefaults();
var connectionString = builder.Configuration.GetConnectionString("booksdb")
    ?? builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Host=localhost;Port=5432;Database=booksdb;Username=postgres;Password=postgres";

builder.Services.AddDbContext<BookStoreDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapControllers();
app.MapDefaultEndpoints();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<BookStoreDbContext>();
    dbContext.Database.EnsureCreated();

    if (!dbContext.Books.Any())
    {
        dbContext.Books.AddRange(
            new BookEntity { Title = "The Pragmatic Programmer", Author = "Andrew Hunt", YearPublished = 1999 },
            new BookEntity { Title = "Clean Code", Author = "Robert C. Martin", YearPublished = 2008 },
            new BookEntity { Title = "Domain-Driven Design", Author = "Eric Evans", YearPublished = 2003 },
            new BookEntity { Title = "Lord of the Rings", Author = "J.R.R. Tolkien", YearPublished = 1954 }
        );

        dbContext.SaveChanges();
    }
}

app.Run();
