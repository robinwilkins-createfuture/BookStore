using BookStore.Api.Controllers;
using BookStore.Api.Dtos;
using BookStore.Api.Models;
using BookStore.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace BookStore.Api.Tests.Controllers;

public class ProfilesControllerTests
{
    private readonly Mock<IProfileService> _profileServiceMock;
    private readonly ProfilesController _controller;

    public ProfilesControllerTests()
    {
        _profileServiceMock = new Mock<IProfileService>();
        _controller = new ProfilesController(_profileServiceMock.Object);
    }

    [Fact]
    public async Task GetProfile_WithValidId_ReturnsOkResult()
    {
        _profileServiceMock
            .Setup(service => service.GetProfileAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Profile { Id = 1, Username = "alice", Email = "alice@example.com" });

        var result = await _controller.GetProfile(1);

        Assert.IsType<OkObjectResult>(result.Result);
        _profileServiceMock.Verify(service => service.GetProfileAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetProfile_WithValidId_ReturnsCorrectProfile()
    {
        var expectedProfile = new Profile
        {
            Id = 2,
            Username = "bob",
            Email = "bob@example.com"
        };

        _profileServiceMock
            .Setup(service => service.GetProfileAsync(2, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedProfile);

        var result = await _controller.GetProfile(2);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var profile = Assert.IsType<ProfileDto>(okResult.Value);
        Assert.Equal(2, profile.Id);
        Assert.Equal("bob", profile.Username);
        Assert.Equal("bob@example.com", profile.Email);
        _profileServiceMock.Verify(service => service.GetProfileAsync(2, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetProfile_WithInvalidId_ReturnsNotFound()
    {
        _profileServiceMock
            .Setup(service => service.GetProfileAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Profile?)null);

        var result = await _controller.GetProfile(999);

        Assert.IsType<NotFoundResult>(result.Result);
        _profileServiceMock.Verify(service => service.GetProfileAsync(999, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateProfile_WithValidProfile_Returns201Created()
    {
        var newProfileDto = new ProfileDto
        {
            Id = 0,
            Username = "charlie",
            Email = "charlie@example.com"
        };
        var createdProfile = new Profile
        {
            Id = 3,
            Username = "charlie",
            Email = "charlie@example.com"
        };

        _profileServiceMock
            .Setup(service => service.CreateProfileAsync(It.IsAny<Profile>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdProfile);

        var result = await _controller.CreateProfile(newProfileDto);

        Assert.IsType<CreatedAtActionResult>(result.Result);
        _profileServiceMock.Verify(
            service => service.CreateProfileAsync(It.Is<Profile>(profile =>
                profile.Username == newProfileDto.Username &&
                profile.Email == newProfileDto.Email), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task CreateProfile_WithValidProfile_ReturnsCreatedProfileDto()
    {
        var newProfileDto = new ProfileDto
        {
            Id = 0,
            Username = "charlie",
            Email = "charlie@example.com"
        };
        var createdProfile = new Profile
        {
            Id = 3,
            Username = "charlie",
            Email = "charlie@example.com"
        };

        _profileServiceMock
            .Setup(service => service.CreateProfileAsync(It.IsAny<Profile>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdProfile);

        var result = await _controller.CreateProfile(newProfileDto);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnedProfile = Assert.IsType<ProfileDto>(createdResult.Value);
        Assert.Equal(3, returnedProfile.Id);
        Assert.Equal("charlie", returnedProfile.Username);
        Assert.Equal("charlie@example.com", returnedProfile.Email);
        Assert.Equal(nameof(ProfilesController.GetProfile), createdResult.ActionName);
    }

    [Fact]
    public async Task CreateProfile_WithNullProfile_ReturnsBadRequest()
    {
        var result = await _controller.CreateProfile(null!);

        Assert.IsType<BadRequestResult>(result.Result);
        _profileServiceMock.Verify(
            service => service.CreateProfileAsync(It.IsAny<Profile>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
