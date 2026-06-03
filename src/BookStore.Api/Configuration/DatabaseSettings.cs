namespace BookStore.Api.Configuration;

public class DatabaseSettings
{
    public const string SectionName = "Database";

    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 5432;
    public string Database { get; set; } = "booksdb";
    public string Username { get; set; } = "postgres";
    public string Password { get; set; } = "postgres";

    public string ToConnectionString() =>
        $"Host={Host};Port={Port};Database={Database};Username={Username};Password={Password}";
}