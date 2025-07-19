using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using URLShortener.Services.Interfaces;

namespace URLShortener.Controllers
{
    // TODO: refactor according to DRY

    [ApiController]
    [Route("api/[controller]")]
    public class ShortUrlsController(IUserUrlService service, IUrlShortenerService shortener) : ControllerBase
    {
        private readonly IUserUrlService _service = service;
        private readonly IUrlShortenerService _shortener = shortener;

        // /ShortUrls
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll(int page = 1, int pageSize = 10)
        {
            var result = await _service.GetAllShortUrlsAsync(page, pageSize);

            if (!result.Success)
                return StatusCode(500, "Unexpected error occurred.");

            return Ok(result.Data);
        }

        // /ShortUrls/Add
        [Authorize]
        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add(string originalUrl)
        {
            if (string.IsNullOrEmpty(originalUrl))
            {
                ModelState.AddModelError("", "URL is required");
                return BadRequest();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Forbid(); // or RedirectToPage("/Account/Login");
            }

            var result = await _service.CreateShortUrlAsync(originalUrl, userId);

            if (!result.Success)
            {
                return result.ErrorCode switch
                {
                    "UknownUser" => Forbid(),
                    "InvalidData" or "NotUnique" => BadRequest(result.ErrorMessage),
                    _ => StatusCode(500, "Unexpected error occurred.")
                };
            }

            return Ok();
        }

        // /ShortUrls/Delete/5
        [Authorize]
        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Forbid();
            }

            var result = await _service.DeleteShortUrlAsync(id, userId);

            if(!result.Success)
            {
                return result.ErrorCode switch
                {
                    "NotFound" => NotFound(),
                    "UknownUser" or "Forbidden" => Forbid(),
                    "InvalidData" => BadRequest(result.ErrorMessage),
                    _ => StatusCode(500, "Unexpected error occurred.")
                };
            }

            return Ok();
        }

        // /ShortUrls/Details/5
        [Authorize]
        [HttpGet]
        [Route("details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Forbid();
            }

            var result = await _service.GetShortUrlInfoAsync(id, userId);

            if (!result.Success)
            {
                return result.ErrorCode switch
                {
                    "NotFound" => NotFound(),
                    "UknownUser" => Forbid(),
                    "InvalidData" => BadRequest(result.ErrorMessage),
                    _ => StatusCode(500, "Unexpected error occurred.")
                };
            }

            return Ok(result.Data);
        }

        // /sho.rt/{shortCode}  - redirection
        [AllowAnonymous]
        [HttpGet]
        [Route("sho.rt/{shortCode}")]
        public async Task<IActionResult> RedirectToOriginal(string shortCode)
        {
            var result = await _shortener.GetOriginalUrlByShortCode(shortCode);

            if (!result.Success)
            {
                return result.ErrorCode switch
                {
                    "NotFound" => NotFound(),
                    _ => StatusCode(500, "Unexpected error occurred.")
                };
            }

            if (result.Data is null)
                return StatusCode(500, "Unexpected error occurred.");           // because if null it's the backend problem

            return Redirect(result.Data);
        }
    }
}