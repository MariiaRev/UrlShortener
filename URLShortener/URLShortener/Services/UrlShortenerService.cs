using System.Diagnostics.SymbolStore;
using System.Security.Cryptography;
using URLShortener.Core.Results;
using URLShortener.Helpers;
using URLShortener.Models;
using URLShortener.Repositories.Interfaces;
using URLShortener.Services.Interfaces;

namespace URLShortener.Services
{
    public class UrlShortenerService(IShortUrlRepository repository) : IUrlShortenerService
    {
        private readonly IShortUrlRepository _repository = repository;

        // Note: Since we use the first 6 characters of the hash as the short key,
        // there are about 16 million (16^6) possible unique keys.
        // While this is sufficient for many use cases, theoretically we may run out of unique keys if the number of URLs grows very large.
        // We already handle collisions by adding a salt and retrying hashing until a unique key is found.
        // Implementing collision checks and retry logic is essential to ensure key uniqueness.
        // Additional improvements could include:
        // 1. Increasing the length of the key to allow more combinations.
        // 2. Using a different algorithm such as encoding an auto-incrementing ID (e.g., base62 encoding).
        public async Task<string> GenerateKeyAsync(string url)
        {
            int salt = 0;
            string key;

            do
            {
                var input = url + (salt > 0 ? salt.ToString() : string.Empty);
                var hash = HashHelper.ComputeSha256Hash(input);
                key = hash[..6];
                salt++;

            } while (await _repository.ExistKeyAsync(key));
            
            return key;
        }

        public async Task<ShortUrl> CreateShortUrlAsync(string originalUrl, string? userId)
        {
            string key = await GenerateKeyAsync(originalUrl);

            return new ShortUrl { Key = key, OriginalUrl = originalUrl, UserId = userId, CreatedDate = DateTime.UtcNow };
        }

        public async Task<OperationResult<string>> GetOriginalUrlByShortCode(string shortCode)
        {
            try
            {
                var shortUrl = await _repository.GetByKeyAsync(shortCode);

                if (shortUrl is null)
                    return OperationResult<string>.Fail("Url was not found by shorting.", "NotFound");

                return OperationResult<string>.Ok(shortUrl.OriginalUrl);

            }
            catch (Exception ex)
            {
                return OperationResult<string>.Fail(ex.Message);
            }
        }
    }
}
