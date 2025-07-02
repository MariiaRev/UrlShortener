using URLShortener.Core.Results;
using URLShortener.Models;
using URLShortener.Repositories.Interfaces;
using URLShortener.Services.Interfaces;

namespace URLShortener.Services
{
    public class UserUrlService
        (IShortUrlRepository repository, 
        IUrlShortenerService urlShortener, 
        IUserService userService) : IUserUrlService
    {
        private readonly IShortUrlRepository _repository = repository;
        private readonly IUrlShortenerService _urlShortener = urlShortener;
        private readonly IUserService _userService = userService;

        // all
        public async Task<OperationResult<List<ShortUrl>>> GetAllShortUrlsAsync(int page, int pageSize)
        {
            if (page <=0 && pageSize <=0)
            {
                return OperationResult<List<ShortUrl>>.Fail("Page and PageSize must be greater than zero.", "Invalid_Paggination_Data");
            }

            var urlList = await _repository.GetAllAsync(page, pageSize);

            return OperationResult<List<ShortUrl>>.Ok(urlList);
        }

        // authorized only
        public async Task<OperationResult<ShortUrl>> CreateShortUrlAsync(string url, string userId)
        {
            var validationResult = await ValidateUserAsync(userId);
            if (!validationResult.Success)
                return OperationResult<ShortUrl>.FromOperationResult(validationResult);

            if(!IsValidUrl(url))
            {
                return OperationResult<ShortUrl>.Fail("Invalid url", "Invalid_Url");
            }

            bool isNotUniqueUrl = await _repository.ExistAsync(url);
            if (isNotUniqueUrl)
            {
                return OperationResult<ShortUrl>.Fail("Url already exists", "Not_Unique_Url");
            }

            try
            {
                ShortUrl shortUrl = await _urlShortener.CreateShortUrlAsync(url, userId);
                await _repository.AddAsync(shortUrl);

                return OperationResult<ShortUrl>.Ok(shortUrl);
            }
            catch (Exception ex) 
            {
                return OperationResult<ShortUrl>.Fail(ex.Message);
            }
        }
        public async Task<OperationResult<ShortUrl>> GetShortUrlInfoAsync(int id, string userId)
        {
            var validationResult = await ValidateUserAsync(userId);
            if (!validationResult.Success)
                return OperationResult<ShortUrl>.FromOperationResult(validationResult);

            if (id < 0)
                return OperationResult<ShortUrl>.Fail("Id cannot be negative.", "Invalid_Id");

            var shortUrl = await _repository.GetByIdAsync(id);

            if (shortUrl is null)
                return OperationResult<ShortUrl>.Fail("No short url with such id", "Not_Found");

            return OperationResult<ShortUrl>.Ok(shortUrl);
        }

        public async Task<OperationResult> DeleteShortUrlAsync(int id, string userId)
        {
            var validationResult = await ValidateUserAsync(userId);
            if (!validationResult.Success)
                return validationResult;

            if (id < 0)
                return OperationResult.Fail("Id cannot be negative.", "Invalid_Id");

            var shortUrl = await _repository.GetByIdAsync(id);
            if (shortUrl is null)
                return OperationResult.Fail("No short url with such id", "Not_Found");

            bool belongsToUser = shortUrl.UserId is not null && shortUrl.UserId.Equals(userId);
            var isAdmin = await IsAdminAsync(userId);

            if (!belongsToUser && !isAdmin)
                return OperationResult.Fail("No rights for deletion", "No_Rights_For_Action");

            try
            {
                await _repository.DeleteAsync(shortUrl);
                return OperationResult.Ok();
            }
            catch (Exception ex)
            {
                return OperationResult.Fail(ex.Message);
            }
        }

        public async Task<OperationResult> DeleteAllUserUrlsAsync(string userId)
        {
            var validationResult = await ValidateUserAsync(userId);
            if (!validationResult.Success)
                return validationResult;

            try
            {
                await _repository.DeleteAllByUserIdAsync(userId);
                return OperationResult.Ok();
            }
            catch (Exception ex)
            {
                return OperationResult.Fail(ex.Message);
            }
        }

        public async Task<OperationResult> DeleteAllAsAdminAsync(string userId)
        {
            var validationResult = await ValidateUserAsync(userId);
            if (!validationResult.Success)
                return validationResult;

            var isAdmin = await IsAdminAsync(userId);
            if (!isAdmin)
                return OperationResult.Fail("No rights for deletion", "No_Rights_For_Action");

            try
            {
                await _repository.DeleteAllAsync();
                return OperationResult.Ok();
            }
            catch (Exception ex)
            {
                return OperationResult.Fail(ex.Message);
            }
        }

        private async Task<OperationResult> ValidateUserAsync(string userId)
        {
            return
                await _userService.UserExistsAsync(userId)
                ? OperationResult.Ok()
                : OperationResult.Fail("User doesn't exist", "Uknown_User");
        }

        private Task<bool> IsAdminAsync(string userId)
        {
            return _userService.IsAdminAsync(userId);
        }

        private static bool IsValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                   && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}
