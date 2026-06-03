namespace BookStore.Api.Data.Entities;

public class ProfileEntity
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
}
