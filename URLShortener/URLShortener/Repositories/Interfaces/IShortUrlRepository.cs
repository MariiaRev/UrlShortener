using URLShortener.Models;

namespace URLShortener.Repositories.Interfaces
{
    public interface IShortUrlRepository
    {
        Task AddAsync(ShortUrl shortUrl);
        Task<ShortUrl?> GetByKeyAsync (string key);
        Task<ShortUrl?> GetByIdAsync(int id);
        Task<List<ShortUrl>> GetByUserIdAsync (string userId, int pageNumber, int pageSize);
        Task<List<ShortUrl>> GetAllAsync(int pageNumber, int pageSize);
        Task DeleteAsync(ShortUrl shortUrl);
        Task DeleteRangeAsync(IEnumerable<ShortUrl> shortUrls);
        Task DeleteAllByUserIdAsync(string userId);
        Task<bool> ExistAsync(string originalUrl);
        Task<bool> ExistKeyAsync(string key);
    }
}
