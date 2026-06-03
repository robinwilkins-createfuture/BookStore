using Scalar.AspNetCore;
using BookStore.Api.Configuration;
using BookStore.Api.Data;
using BookStore.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.AddServiceDefaults();
var databaseSettings = builder.Configuration
    .GetSection(DatabaseSettings.SectionName)
    .Get<DatabaseSettings>()
    ?? new DatabaseSettings();

var connectionString = builder.Configuration.GetConnectionString("booksdb")
    ?? builder.Configuration.GetConnectionString("DefaultConnection")
    ?? databaseSettings.ToConnectionString();

builder.Services.AddDbContext<BookStoreDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IProfileService, ProfileService>();
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

DatabaseInitializer.Initialize(app);

app.Run();
