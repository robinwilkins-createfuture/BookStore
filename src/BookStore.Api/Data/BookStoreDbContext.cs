using BookStore.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Api.Data;

public class BookStoreDbContext : DbContext
{
    public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options) : base(options)
    {
    }

    public DbSet<BookEntity> Books => Set<BookEntity>();
    public DbSet<ProfileEntity> Profiles => Set<ProfileEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BookEntity>(entity =>
        {
            entity.ToTable("Books");
            entity.HasKey(b => b.Id);
            entity.Property(b => b.Title).IsRequired();
            entity.Property(b => b.Author).IsRequired();
        });

        modelBuilder.Entity<ProfileEntity>(entity =>
        {
            entity.ToTable("Profiles");
            entity.HasKey(profile => profile.Id);
            entity.Property(profile => profile.Username).IsRequired();
            entity.Property(profile => profile.Email).IsRequired();
        });
    }
}
