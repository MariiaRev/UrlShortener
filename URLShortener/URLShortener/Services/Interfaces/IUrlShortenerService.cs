using URLShortener.Models;

namespace URLShortener.Services.Interfaces
{
    public interface IUrlShortenerService
    {
        Task<string> GenerateKeyAsync(string url);
        Task<ShortUrl> CreateShortUrlAsync(string originalUrl, string? userId);
    }
}
