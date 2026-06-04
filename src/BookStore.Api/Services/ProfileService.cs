using BookStore.Api.Data;
using BookStore.Api.Data.Entities;
using BookStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Api.Services;

public class ProfileService : IProfileService
{
    private readonly BookStoreDbContext _dbContext;

    public ProfileService(BookStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Profile?> GetProfileAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.Profiles
            .AsNoTracking()
            .FirstOrDefaultAsync(profile => profile.Id == id, cancellationToken);

        return entity == null ? null : MapToModel(entity);
    }

    public async Task<Profile> CreateProfileAsync(Profile profile, CancellationToken cancellationToken = default)
    {
        var entity = MapToEntity(profile);
        _dbContext.Profiles.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return MapToModel(entity);
    }

    private static Profile MapToModel(ProfileEntity entity) => new()
    {
        Id = entity.Id,
        Username = entity.Username,
        Email = entity.Email
    };

    private static ProfileEntity MapToEntity(Profile model) => new()
    {
        Username = model.Username,
        Email = model.Email
    };
}
