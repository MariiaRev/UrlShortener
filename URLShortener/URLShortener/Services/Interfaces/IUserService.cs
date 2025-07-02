namespace URLShortener.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> IsAdminAsync(string userId);
        Task<bool> UserExistsAsync(string userId);
    }
}
