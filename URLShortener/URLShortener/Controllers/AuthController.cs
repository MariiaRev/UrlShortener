using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using URLShortener.DTOs.Requests;

namespace URLShortener.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager) : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager = signInManager;
        private readonly UserManager<IdentityUser> _userManager = userManager;

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null)
                return Unauthorized("Invalid username or password");

            var result = await _signInManager.PasswordSignInAsync(user, request.Password, isPersistent: false, lockoutOnFailure: false);
            if (!result.Succeeded)
                return Unauthorized("Invalid username or password");

            return Ok(new { success = true });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { success = true });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var existingUser = await _userManager.FindByNameAsync(request.Username);
            if (existingUser != null)
                return BadRequest("User already exists");

            var user = new IdentityUser { UserName = request.Username };
            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await _signInManager.SignInAsync(user, isPersistent: false);

            return Ok(new { success = true });
        }
    }
}

