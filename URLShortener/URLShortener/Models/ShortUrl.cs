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

        public override bool Equals(object? obj)
        {
            if (obj is not ShortUrl other)
                return false;

            return Id == other.Id &&
                   Key == other.Key &&
                   OriginalUrl == other.OriginalUrl &&
                   UserId == other.UserId;
            // CreatedDate is excluded
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Key, OriginalUrl, UserId);
            // CreatedDate is excluded
        }
    }
}
