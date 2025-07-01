using Microsoft.EntityFrameworkCore;
using URLShortener.Data;
using URLShortener.Models;
using URLShortener.Repositories.Interfaces;

namespace URLShortener.Repositories
{
    public class ShortUrlRepository(AppDbContext context): IShortUrlRepository
    {
        private readonly AppDbContext _context = context;
        public async Task AddAsync(ShortUrl shortUrl)
        {
            await _context.ShortUrls.AddAsync(shortUrl);
            await _context.SaveChangesAsync();
        }

        public Task<ShortUrl?> GetByKeyAsync(string key)
        {
            return _context.ShortUrls.FirstOrDefaultAsync(x => x.Key == key);
        }

        public Task<ShortUrl?> GetByIdAsync(int id)
        {
            return _context.ShortUrls.FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<List<ShortUrl>> GetByUserIdAsync(string userId, int skip, int take)
        {
            return
                _context.ShortUrls
                .Where(x => x.UserId != null && x.UserId == userId)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public Task<List<ShortUrl>> GetAllAsync(int skip, int take)
        {
            return 
                _context.ShortUrls
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task DeleteAsync(ShortUrl shortUrl)
        {
            _context.ShortUrls.Remove(shortUrl);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(IEnumerable<ShortUrl> shortUrls)
        {
            _context.RemoveRange(shortUrls);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAllByUserIdAsync(string userId)
        {
            var shortUrls = _context.ShortUrls.Where(x => x.UserId != null && x.UserId == userId);
            _context.RemoveRange(shortUrls);
            await _context.SaveChangesAsync();
        }

        public Task<bool> ExistAsync(string originalUrl)
        {
            return _context.ShortUrls.AnyAsync(x => x.OriginalUrl == originalUrl);
        }

        public Task<bool> ExistKeyAsync(string key)
        {
            return _context.ShortUrls.AnyAsync(x => x.Key == key);
        }

        // for unit tests only
        public async Task AddRangeAsync(IEnumerable<ShortUrl> shortUrls)
        {
            await _context.ShortUrls.AddRangeAsync(shortUrls);
            await _context.SaveChangesAsync();
        }
    }
}
