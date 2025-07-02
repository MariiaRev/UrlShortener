using URLShortener.Core.Results;
using URLShortener.Models;

namespace URLShortener.Services.Interfaces
{
    public interface IUserUrlService
    {
        // all
        Task<OperationResult<List<ShortUrl>>> GetAllShortUrlsAsync(int page, int pageSize);

        // authorized only
        Task<OperationResult<ShortUrl>> CreateShortUrlAsync(string url, string userId);
        Task<OperationResult<ShortUrl>> GetShortUrlInfoAsync(int id, string userId);
        Task<OperationResult> DeleteShortUrlAsync(int id, string userId);
        Task<OperationResult> DeleteAllUserUrlsAsync(string userId);
        Task<OperationResult> DeleteAllAsAdminAsync(string userId);
    }
}
