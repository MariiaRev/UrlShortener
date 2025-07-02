using Microsoft.AspNetCore.Identity;
using URLShortener.Services.Interfaces;

namespace URLShortener.Services
{
    public class UserService(UserManager<IdentityUser> userManager) : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager = userManager;

        public async Task<bool> IsAdminAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            return await _userManager.IsInRoleAsync(user, "Admin");
        }

        public async Task<bool> UserExistsAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user != null;
        }
    }
}
