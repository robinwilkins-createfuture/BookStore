using BookStore.Api.Models;

namespace BookStore.Api.Services;

public interface IProfileService
{
    Task<Profile?> GetProfileAsync(int id, CancellationToken cancellationToken = default);
    Task<Profile> CreateProfileAsync(Profile profile, CancellationToken cancellationToken = default);
}
