using BookStore.Api.Dtos;
using BookStore.Api.Models;
using BookStore.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProfilesController : ControllerBase
{
    private readonly IProfileService _profileService;

    public ProfilesController(IProfileService profileService)
    {
        _profileService = profileService;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProfileDto>> GetProfile(int id, CancellationToken cancellationToken = default)
    {
        var profile = await _profileService.GetProfileAsync(id, cancellationToken);
        if (profile == null)
        {
            return NotFound();
        }

        return Ok(MapToDto(profile));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ProfileDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProfileDto>> CreateProfile(ProfileDto profile, CancellationToken cancellationToken = default)
    {
        if (profile == null)
        {
            return BadRequest();
        }

        var createdProfile = await _profileService.CreateProfileAsync(MapToModel(profile), cancellationToken);
        var createdProfileDto = MapToDto(createdProfile);

        return CreatedAtAction(nameof(GetProfile), new { id = createdProfileDto.Id }, createdProfileDto);
    }

    private static ProfileDto MapToDto(Profile profile) => new()
    {
        Id = profile.Id,
        Username = profile.Username,
        Email = profile.Email
    };

    private static Profile MapToModel(ProfileDto profileDto) => new()
    {
        Username = profileDto.Username,
        Email = profileDto.Email
    };
}
