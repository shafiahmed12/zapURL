namespace zapURL.Api.Models;

public class ShortUrl
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string OriginalUrl { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public Guid? CreatedBy { get; set; }
    public User? CreatedByUser { get; set; }
}