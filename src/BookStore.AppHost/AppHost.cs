var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithPgAdmin();

var booksDb = postgres.AddDatabase("booksdb");

builder.AddProject<Projects.BookStore_Api>("bookstore-api")
    .WithReference(booksDb)
    .WaitFor(booksDb);

builder.Build().Run();
