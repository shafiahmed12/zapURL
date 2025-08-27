namespace zapURL.Api.Models;

public class User
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Guid StackAuthUserId { get; set; }
    public string Email { get; set; } = null!;
    public ICollection<ShortUrl> ShortUrls { get; set; } = new List<ShortUrl>();
}
