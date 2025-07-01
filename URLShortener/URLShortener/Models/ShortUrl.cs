using Microsoft.AspNetCore.Identity;

namespace URLShortener.Models
{
    public class ShortUrl
    {
        public int Id { get; set; }
        public required string Key { get; set; }
        public required string OriginalUrl { get; set; }
        public DateTime CreatedDate { get; set; }

        public string? UserId { get; set; } //FK for AspNetUsers.Id
        public IdentityUser? User { get; set; } //short url created by this user
    }
}
