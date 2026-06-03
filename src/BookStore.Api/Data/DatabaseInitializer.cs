using BookStore.Api.Data.Entities;
using System.Globalization;
using System.Text;

namespace BookStore.Api.Data;

public static class DatabaseInitializer
{
    public static void Initialize(WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            return;
        }

        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BookStoreDbContext>();

        dbContext.Database.EnsureCreated();

        if (dbContext.Books.Any())
        {
            return;
        }

        var seedFilePath = Path.Combine(app.Environment.ContentRootPath, "Data", "Seed", "books.csv");
        var seedBooks = ReadBooksFromCsv(seedFilePath);

        dbContext.Books.AddRange(seedBooks);

        dbContext.SaveChanges();
    }

    private static List<BookEntity> ReadBooksFromCsv(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Seed file not found: {filePath}", filePath);
        }

        var books = new List<BookEntity>();
        var lines = File.ReadAllLines(filePath);

        foreach (var rawLine in lines.Skip(1))
        {
            if (string.IsNullOrWhiteSpace(rawLine))
            {
                continue;
            }

            var values = ParseCsvLine(rawLine);
            if (values.Count != 3)
            {
                throw new FormatException($"Invalid CSV row. Expected 3 columns but got {values.Count}. Row: {rawLine}");
            }

            if (!int.TryParse(values[2], NumberStyles.Integer, CultureInfo.InvariantCulture, out var yearPublished))
            {
                throw new FormatException($"Invalid yearPublished value '{values[2]}' in row: {rawLine}");
            }

            books.Add(new BookEntity
            {
                Title = values[0],
                Author = values[1],
                YearPublished = yearPublished
            });
        }

        return books;
    }

    private static List<string> ParseCsvLine(string line)
    {
        var values = new List<string>();
        var current = new StringBuilder();
        var inQuotes = false;

        foreach (var character in line)
        {
            if (character == '"')
            {
                inQuotes = !inQuotes;
                continue;
            }

            if (character == ',' && !inQuotes)
            {
                values.Add(current.ToString().Trim());
                current.Clear();
                continue;
            }

            current.Append(character);
        }

        values.Add(current.ToString().Trim());
        return values;
    }
}
