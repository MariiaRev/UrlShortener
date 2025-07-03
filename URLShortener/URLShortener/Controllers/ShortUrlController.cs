using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using URLShortener.Services.Interfaces;

namespace URLShortener.Controllers
{
    // TODO: refactor according to DRY

    public class ShortUrlsController(IUserUrlService service, IUrlShortenerService shortener) : Controller
    {
        private readonly IUserUrlService _service = service;
        private readonly IUrlShortenerService _shortener = shortener;

        // /ShortUrls/
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var result = await _service.GetAllShortUrlsAsync(page, pageSize);

            if (!result.Success)
                return StatusCode(500, "Unexpected error occurred.");

            return View(result.Data);
        }

        // /ShortUrls/Add
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(string originalUrl)
        {
            if (string.IsNullOrEmpty(originalUrl))
            {
                ModelState.AddModelError("", "URL is required");
                return RedirectToAction("Index");
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

            return RedirectToAction("Index");
        }

        // /ShortUrls/Delete/5
        [Authorize]
        [HttpPost]
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

            return RedirectToAction("Index");
        }

        // /ShortUrls/Details/5
        [Authorize]
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

            return View(result.Data);
        }

        // /sho.rt/{shortCode}  - redirection
        [AllowAnonymous]
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