using BookStore.Api.Data;
using BookStore.Api.Data.Entities;
using BookStore.Api.Models;
using BookStore.Api.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BookStore.Api.Tests.Services;

public class ProfileServiceTests
{
    private static ProfileService CreateService(string dbName)
    {
        var options = new DbContextOptionsBuilder<BookStoreDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        var context = new BookStoreDbContext(options);
        context.Profiles.AddRange(
            new ProfileEntity { Id = 1, Username = "alice", Email = "alice@example.com" },
            new ProfileEntity { Id = 2, Username = "bob", Email = "bob@example.com" }
        );
        context.SaveChanges();

        return new ProfileService(context);
    }

    [Fact]
    public async Task GetProfile_WithValidId_ReturnsProfile()
    {
        var service = CreateService(nameof(GetProfile_WithValidId_ReturnsProfile));

        var profile = await service.GetProfileAsync(1);

        Assert.NotNull(profile);
        Assert.Equal(1, profile!.Id);
        Assert.Equal("alice", profile.Username);
        Assert.Equal("alice@example.com", profile.Email);
    }

    [Fact]
    public async Task GetProfile_WithInvalidId_ReturnsNull()
    {
        var service = CreateService(nameof(GetProfile_WithInvalidId_ReturnsNull));

        var profile = await service.GetProfileAsync(999);

        Assert.Null(profile);
    }

    [Fact]
    public async Task CreateProfile_AssignsNextId_AndAddsProfile()
    {
        var service = CreateService(nameof(CreateProfile_AssignsNextId_AndAddsProfile));
        var newProfile = new Profile
        {
            Id = 999,
            Username = "charlie",
            Email = "charlie@example.com"
        };

        var createdProfile = await service.CreateProfileAsync(newProfile);

        Assert.Equal(3, createdProfile.Id);
        Assert.Equal("charlie", createdProfile.Username);
        Assert.Equal("charlie@example.com", createdProfile.Email);

        var retrieved = await service.GetProfileAsync(3);
        Assert.NotNull(retrieved);
        Assert.Equal("charlie", retrieved!.Username);
    }
}
