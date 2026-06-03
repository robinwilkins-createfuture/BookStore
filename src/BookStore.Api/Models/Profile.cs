namespace BookStore.Api.Models;

public class Profile
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
}
