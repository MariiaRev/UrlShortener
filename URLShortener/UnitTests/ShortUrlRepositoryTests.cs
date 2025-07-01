using Microsoft.EntityFrameworkCore;
using URLShortener.Data;
using URLShortener.Models;
using URLShortener.Repositories;

namespace UnitTests
{
    public class ShortUrlRepositoryTests
    {
        [Fact]
        public async Task AddAsync_ShouldAddShortUrl()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new AppDbContext(options);
            var repo = new ShortUrlRepository(context);

            var shortUrl = new ShortUrl { Key = "key", OriginalUrl = "original", CreatedDate = DateTime.UtcNow };

            await repo.AddAsync(shortUrl);

            var exists = await repo.ExistAsync(shortUrl.OriginalUrl);
            Assert.True(exists);
        }
        
        [Fact]
        public async Task AddRangeAsync_ShouldAddShortUrls()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new AppDbContext(options);
            var repo = new ShortUrlRepository(context);

            var shortUrl1 = new ShortUrl { Key = "key1", OriginalUrl = "original1", CreatedDate = DateTime.UtcNow.AddDays(-2) };
            var shortUrl2 = new ShortUrl { Key = "key2", OriginalUrl = "original2", CreatedDate = DateTime.UtcNow.AddDays(-1) };
            var shortUrl3 = new ShortUrl { Key = "key3", OriginalUrl = "original3", CreatedDate = DateTime.UtcNow };

            List<ShortUrl> shortUrls = [shortUrl1, shortUrl2, shortUrl3];

            await repo.AddRangeAsync(shortUrls);

            var exists1 = await repo.ExistAsync(shortUrl1.OriginalUrl);
            var exists2 = await repo.ExistAsync(shortUrl2.OriginalUrl);
            var exists3 = await repo.ExistAsync(shortUrl3.OriginalUrl);

            var exists = exists1 && exists2 && exists3;

            Assert.True(exists);
        }

        [Theory]
        [InlineData("https://example.com", "a1234")]
        [InlineData("example.com", "")]
        public async Task GetByKeyAsync_ShouldGetShortUrl(string urlStr, string keyStr)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new AppDbContext(options);
            var repo = new ShortUrlRepository(context);

            var shortUrl = new ShortUrl { Key = keyStr, OriginalUrl = urlStr, CreatedDate = DateTime.UtcNow };
            var shortUrl1 = new ShortUrl { Key = "key1", OriginalUrl = "original1", CreatedDate = DateTime.UtcNow };
            var shortUrl2 = new ShortUrl { Key = "key2", OriginalUrl = "original2", CreatedDate = DateTime.UtcNow };
            var shortUrl3 = new ShortUrl { Key = "key3", OriginalUrl = "original3", CreatedDate = DateTime.UtcNow };

            List<ShortUrl> shortUrls = [shortUrl, shortUrl1, shortUrl2, shortUrl3];

            await repo.AddRangeAsync(shortUrls);

            var result = await repo.GetByKeyAsync(keyStr);

            Assert.Equal(shortUrl, result);
        }
    
        [Fact]
        public async Task GetByKeyAsync_ShouldNotGetShortUrl()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new AppDbContext(options);
            var repo = new ShortUrlRepository(context);

            var keyStr = "key";
            var shortUrl1 = new ShortUrl { Key = "key1", OriginalUrl = "original1", CreatedDate = DateTime.UtcNow };
            var shortUrl2 = new ShortUrl { Key = "key2", OriginalUrl = "original2", CreatedDate = DateTime.UtcNow };
            var shortUrl3 = new ShortUrl { Key = "key3", OriginalUrl = "original3", CreatedDate = DateTime.UtcNow };

            List<ShortUrl> shortUrls = [shortUrl1, shortUrl2, shortUrl3];

            await repo.AddRangeAsync(shortUrls);

            var result = await repo.GetByKeyAsync(keyStr);

            Assert.Null(result);
        }
    
        [Theory]
        [InlineData("https://example.com", "a1234")]
        [InlineData("example.com", "")]
        public async Task GetByIdAsync_ShouldGetShortUrl(string urlStr, string keyStr)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new AppDbContext(options);
            var repo = new ShortUrlRepository(context);

            var shortUrl = new ShortUrl { Key = keyStr, OriginalUrl = urlStr, CreatedDate = DateTime.UtcNow };
            var shortUrl1 = new ShortUrl { Key = "key1", OriginalUrl = "original1", CreatedDate = DateTime.UtcNow };
            var shortUrl2 = new ShortUrl { Key = "key2", OriginalUrl = "original2", CreatedDate = DateTime.UtcNow };
            var shortUrl3 = new ShortUrl { Key = "key3", OriginalUrl = "original3", CreatedDate = DateTime.UtcNow };

            List<ShortUrl> shortUrls = [shortUrl, shortUrl1, shortUrl2, shortUrl3];

            await repo.AddRangeAsync(shortUrls);
            var foundShortUrl = await repo.GetByKeyAsync(keyStr);

            Assert.NotNull(foundShortUrl);

            var result = await repo.GetByIdAsync(foundShortUrl.Id);

            Assert.Equal(shortUrl, result);
        }
    
        [Fact]
        public async Task GetByIdAsync_ShouldNotGetShortUrl()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new AppDbContext(options);
            var repo = new ShortUrlRepository(context);

            var id = -1;
            var shortUrl1 = new ShortUrl { Key = "key1", OriginalUrl = "original1", CreatedDate = DateTime.UtcNow };
            var shortUrl2 = new ShortUrl { Key = "key2", OriginalUrl = "original2", CreatedDate = DateTime.UtcNow };
            var shortUrl3 = new ShortUrl { Key = "key3", OriginalUrl = "original3", CreatedDate = DateTime.UtcNow };

            List<ShortUrl> shortUrls = [shortUrl1, shortUrl2, shortUrl3];

            await repo.AddRangeAsync(shortUrls);

            var result = await repo.GetByIdAsync(id);

            Assert.Null(result);
        }
        
        [Fact]
        public async Task GetByUserIdAsync_ShouldGetShortUrls()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new AppDbContext(options);
            var repo = new ShortUrlRepository(context);

            var userId = "11";
            List<ShortUrl> shortUrls =
                [
                    new ShortUrl { Key = "key1", OriginalUrl = "original1", CreatedDate = DateTime.UtcNow, UserId = "21sd" },
                    new ShortUrl { Key = "key2", OriginalUrl = "original2", CreatedDate = DateTime.UtcNow },
                    new ShortUrl { Key = "key3", OriginalUrl = "original3", CreatedDate = DateTime.UtcNow, UserId = userId },
                    new ShortUrl { Key = "key4", OriginalUrl = "original4", CreatedDate = DateTime.UtcNow, UserId = "kodjfj" },
                    new ShortUrl { Key = "key5", OriginalUrl = "original5", CreatedDate = DateTime.UtcNow, UserId = userId },
                    new ShortUrl { Key = "key6", OriginalUrl = "original6", CreatedDate = DateTime.UtcNow, UserId = userId },
                    new ShortUrl { Key = "key7", OriginalUrl = "original7", CreatedDate = DateTime.UtcNow, UserId = "21sd" }
                ];
            
            List<ShortUrl> expectedResult = shortUrls.Where(x => x.UserId == userId).ToList();

            await repo.AddRangeAsync(shortUrls);

            var result = await repo.GetByUserIdAsync(userId, 0, 10);
            Assert.Equal(result, expectedResult);
        }
        
        [Theory]
        [InlineData(0, 2)]
        [InlineData(1, 2)]
        [InlineData(1, 1)]
        public async Task GetByUserIdAsync_ShouldGetExactNumberShortUrls(int skip, int take)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new AppDbContext(options);
            var repo = new ShortUrlRepository(context);

            var userId = "11";
            List<ShortUrl> shortUrls =
                [
                    new ShortUrl { Key = "key1", OriginalUrl = "original1", CreatedDate = DateTime.UtcNow, UserId = "21sd" },
                    new ShortUrl { Key = "key2", OriginalUrl = "original2", CreatedDate = DateTime.UtcNow },
                    new ShortUrl { Key = "key3", OriginalUrl = "original3", CreatedDate = DateTime.UtcNow, UserId = userId },
                    new ShortUrl { Key = "key4", OriginalUrl = "original4", CreatedDate = DateTime.UtcNow, UserId = "kodjfj" },
                    new ShortUrl { Key = "key5", OriginalUrl = "original5", CreatedDate = DateTime.UtcNow, UserId = userId },
                    new ShortUrl { Key = "key6", OriginalUrl = "original6", CreatedDate = DateTime.UtcNow, UserId = userId },
                    new ShortUrl { Key = "key7", OriginalUrl = "original7", CreatedDate = DateTime.UtcNow, UserId = "21sd" }
                ];
            
            List<ShortUrl> expectedResult = shortUrls.Where(x => x.UserId == userId).Skip(skip).Take(take).ToList();

            await repo.AddRangeAsync(shortUrls);

            var result = await repo.GetByUserIdAsync(userId, skip, take);
            Assert.Equal(result, expectedResult);
        }
        
        [Theory]
        [InlineData("12jdc")]
        [InlineData("")]
        [InlineData("null")]
        public async Task GetByUserIdAsync_ShouldGetEmptyList(string userId)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new AppDbContext(options);
            var repo = new ShortUrlRepository(context);

            List<ShortUrl> shortUrls =
                [
                    new ShortUrl { Key = "key1", OriginalUrl = "original1", CreatedDate = DateTime.UtcNow, UserId = "21sd" },
                    new ShortUrl { Key = "key2", OriginalUrl = "original2", CreatedDate = DateTime.UtcNow },
                    new ShortUrl { Key = "key3", OriginalUrl = "original3", CreatedDate = DateTime.UtcNow, UserId = "11" },
                    new ShortUrl { Key = "key4", OriginalUrl = "original4", CreatedDate = DateTime.UtcNow, UserId = "kodjfj" },
                    new ShortUrl { Key = "key5", OriginalUrl = "original5", CreatedDate = DateTime.UtcNow, UserId = "11" },
                    new ShortUrl { Key = "key6", OriginalUrl = "original6", CreatedDate = DateTime.UtcNow, UserId = "11" },
                    new ShortUrl { Key = "key7", OriginalUrl = "original7", CreatedDate = DateTime.UtcNow, UserId = "21sd" }
                ];

            List<ShortUrl> expectedResult = [];

            await repo.AddRangeAsync(shortUrls);

            var result = await repo.GetByUserIdAsync(userId, 0, 10);
            Assert.Equal(result, expectedResult);
        }

        [Theory]
        [InlineData(0, 2)]
        [InlineData(1, 2)]
        [InlineData(1, 1)]
        [InlineData(0, 10)]
        [InlineData(0, 0)]
        public async Task GetAllAsync_ShouldGetExactNumberShortUrls(int skip, int take)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new AppDbContext(options);
            var repo = new ShortUrlRepository(context);

            List<ShortUrl> shortUrls =
                [
                    new ShortUrl { Key = "key1", OriginalUrl = "original1", CreatedDate = DateTime.UtcNow, UserId = "21sd" },
                    new ShortUrl { Key = "key2", OriginalUrl = "original2", CreatedDate = DateTime.UtcNow },
                    new ShortUrl { Key = "key3", OriginalUrl = "original3", CreatedDate = DateTime.UtcNow, UserId = "11" },
                    new ShortUrl { Key = "key4", OriginalUrl = "original4", CreatedDate = DateTime.UtcNow, UserId = "kodjfj" },
                    new ShortUrl { Key = "key5", OriginalUrl = "original5", CreatedDate = DateTime.UtcNow, UserId = "11" },
                    new ShortUrl { Key = "key6", OriginalUrl = "original6", CreatedDate = DateTime.UtcNow, UserId = "11" },
                    new ShortUrl { Key = "key7", OriginalUrl = "original7", CreatedDate = DateTime.UtcNow, UserId = "21sd" }
                ];

            List<ShortUrl> expectedResult = shortUrls.Skip(skip).Take(take).ToList();

            await repo.AddRangeAsync(shortUrls);

            var result = await repo.GetAllAsync(skip, take);
            Assert.Equal(result, expectedResult);
        }

        [Fact]
        public async Task GetAllAsync_ShouldGetAllShortUrls()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new AppDbContext(options);
            var repo = new ShortUrlRepository(context);

            List<ShortUrl> shortUrls =
                [
                    new ShortUrl { Id = 1, Key = "key1", OriginalUrl = "original1", CreatedDate = DateTime.UtcNow, UserId = "21sd" },
                    new ShortUrl { Id = 2, Key = "key2", OriginalUrl = "original2", CreatedDate = DateTime.UtcNow },
                    new ShortUrl { Id = 3, Key = "key3", OriginalUrl = "original3", CreatedDate = DateTime.UtcNow, UserId = "11" },
                    new ShortUrl { Id = 4, Key = "key4", OriginalUrl = "original4", CreatedDate = DateTime.UtcNow, UserId = "kodjfj" },
                    new ShortUrl { Id = 5, Key = "key5", OriginalUrl = "original5", CreatedDate = DateTime.UtcNow, UserId = "11" },
                    new ShortUrl { Id = 6, Key = "key6", OriginalUrl = "original6", CreatedDate = DateTime.UtcNow, UserId = "11" },
                    new ShortUrl { Id = 7, Key = "key7", OriginalUrl = "original7", CreatedDate = DateTime.UtcNow, UserId = "21sd" }
                ];

            await repo.AddRangeAsync(shortUrls);

            var result = await repo.GetAllAsync(0, shortUrls.Count);
            Assert.Equal(result, shortUrls);
        }
    
        [Fact]
        public async Task GetAllAsync_ShouldGetEmptyList()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new AppDbContext(options);
            var repo = new ShortUrlRepository(context);

            List<ShortUrl> shortUrls = [];

            await repo.AddRangeAsync(shortUrls);

            var result = await repo.GetAllAsync(0, shortUrls.Count);
            Assert.Equal(result, shortUrls);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteShortUrl()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new AppDbContext(options);
            var repo = new ShortUrlRepository(context);

            var shortUrl1 = new ShortUrl { Key = "key1", OriginalUrl = "original1", CreatedDate = DateTime.UtcNow };
            var shortUrl2 = new ShortUrl { Key = "key2", OriginalUrl = "original2", CreatedDate = DateTime.UtcNow };
            var shortUrl3 = new ShortUrl { Key = "key3", OriginalUrl = "original3", CreatedDate = DateTime.UtcNow };

            List<ShortUrl> shortUrls = [shortUrl1, shortUrl2, shortUrl3];
            List<ShortUrl> expectedResult = [shortUrl1, shortUrl3];

            await repo.AddRangeAsync(shortUrls);

            await repo.DeleteAsync(shortUrl2);

            var result = await repo.GetAllAsync(0, shortUrls.Count);

            Assert.Equal(result, expectedResult);
        }
    
        [Fact]
        public async Task DeleteAsync_ShouldThrowException()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new AppDbContext(options);
            var repo = new ShortUrlRepository(context);

            var shortUrl1 = new ShortUrl { Key = "key1", OriginalUrl = "original1", CreatedDate = DateTime.UtcNow };
            var shortUrl2 = new ShortUrl { Key = "key2", OriginalUrl = "original2", CreatedDate = DateTime.UtcNow };
            var shortUrl3 = new ShortUrl { Key = "key3", OriginalUrl = "original3", CreatedDate = DateTime.UtcNow };
            var notInDb = new ShortUrl { Key = "key4", OriginalUrl = "original4", CreatedDate = DateTime.UtcNow };

            List<ShortUrl> shortUrls = [shortUrl1, shortUrl2, shortUrl3];

            await repo.AddRangeAsync(shortUrls);

            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => repo.DeleteAsync(notInDb));
        }

        [Fact]
        public async Task DeleteRangeAsync_ShouldDeleteShortUrls()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new AppDbContext(options);
            var repo = new ShortUrlRepository(context);

            var shortUrl1 = new ShortUrl { Key = "key1", OriginalUrl = "original1", CreatedDate = DateTime.UtcNow };
            var shortUrl2 = new ShortUrl { Key = "key2", OriginalUrl = "original2", CreatedDate = DateTime.UtcNow };
            var shortUrl3 = new ShortUrl { Key = "key3", OriginalUrl = "original3", CreatedDate = DateTime.UtcNow };
            var shortUrl4 = new ShortUrl { Key = "key4", OriginalUrl = "original4", CreatedDate = DateTime.UtcNow };

            List<ShortUrl> shortUrls = [shortUrl1, shortUrl2, shortUrl3, shortUrl4];
            List<ShortUrl> expectedResult = [shortUrl1, shortUrl3];

            await repo.AddRangeAsync(shortUrls);

            await repo.DeleteRangeAsync([shortUrl2, shortUrl4]);

            var result = await repo.GetAllAsync(0, shortUrls.Count);

            Assert.Equal(result, expectedResult);
        }
        
        [Fact]
        public async Task DeleteRangeAsync_ShouldNotDeleteEmptyShortUrls()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new AppDbContext(options);
            var repo = new ShortUrlRepository(context);

            var shortUrl1 = new ShortUrl { Key = "key1", OriginalUrl = "original1", CreatedDate = DateTime.UtcNow };
            var shortUrl2 = new ShortUrl { Key = "key2", OriginalUrl = "original2", CreatedDate = DateTime.UtcNow };
            var shortUrl3 = new ShortUrl { Key = "key3", OriginalUrl = "original3", CreatedDate = DateTime.UtcNow };
            var shortUrl4 = new ShortUrl { Key = "key4", OriginalUrl = "original4", CreatedDate = DateTime.UtcNow };

            List<ShortUrl> shortUrls = [shortUrl1, shortUrl2, shortUrl3, shortUrl4];

            await repo.AddRangeAsync(shortUrls);

            await repo.DeleteRangeAsync([]);

            var result = await repo.GetAllAsync(0, shortUrls.Count);

            Assert.Equal(result, shortUrls);
        }

        [Fact]
        public async Task DeleteRangeAsync_ShouldThrowExceptionIfNotInDb()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new AppDbContext(options);
            var repo = new ShortUrlRepository(context);

            var shortUrl1 = new ShortUrl { Key = "key1", OriginalUrl = "original1", CreatedDate = DateTime.UtcNow };
            var shortUrl2 = new ShortUrl { Key = "key2", OriginalUrl = "original2", CreatedDate = DateTime.UtcNow };
            var shortUrl3 = new ShortUrl { Key = "key3", OriginalUrl = "original3", CreatedDate = DateTime.UtcNow };
            var notInDb = new ShortUrl { Key = "key4", OriginalUrl = "original4", CreatedDate = DateTime.UtcNow };

            List<ShortUrl> shortUrls = [shortUrl1, shortUrl2, shortUrl3];

            await repo.AddRangeAsync(shortUrls);

            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => repo.DeleteRangeAsync([shortUrl1, notInDb]));
        }
        
        [Fact]
        public async Task DeleteRangeAsync_ShouldThrowExceptionIfNull()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new AppDbContext(options);
            var repo = new ShortUrlRepository(context);

            var shortUrl1 = new ShortUrl { Key = "key1", OriginalUrl = "original1", CreatedDate = DateTime.UtcNow };
            var shortUrl2 = new ShortUrl { Key = "key2", OriginalUrl = "original2", CreatedDate = DateTime.UtcNow };
            var shortUrl3 = new ShortUrl { Key = "key3", OriginalUrl = "original3", CreatedDate = DateTime.UtcNow };
            var notInDb = new ShortUrl { Key = "key4", OriginalUrl = "original4", CreatedDate = DateTime.UtcNow };

            List<ShortUrl> shortUrls = [shortUrl1, shortUrl2, shortUrl3];

            await repo.AddRangeAsync(shortUrls);

            await Assert.ThrowsAsync<ArgumentNullException>(() => repo.DeleteRangeAsync(null)) ;
        }

        [Fact]
        public async Task DeleteAllByUserIdAsync_ShouldDeleteShortUrls()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new AppDbContext(options);
            var repo = new ShortUrlRepository(context);

            var userId = "11";

            List<ShortUrl> shortUrls =
                [
                    new ShortUrl { Key = "key1", OriginalUrl = "original1", CreatedDate = DateTime.UtcNow, UserId = "21sd" },
                    new ShortUrl { Key = "key2", OriginalUrl = "original2", CreatedDate = DateTime.UtcNow },
                    new ShortUrl { Key = "key3", OriginalUrl = "original3", CreatedDate = DateTime.UtcNow, UserId = userId },
                    new ShortUrl { Key = "key4", OriginalUrl = "original4", CreatedDate = DateTime.UtcNow, UserId = "kodjfj" },
                    new ShortUrl { Key = "key5", OriginalUrl = "original5", CreatedDate = DateTime.UtcNow, UserId = userId },
                    new ShortUrl { Key = "key6", OriginalUrl = "original6", CreatedDate = DateTime.UtcNow, UserId = userId },
                    new ShortUrl { Key = "key7", OriginalUrl = "original7", CreatedDate = DateTime.UtcNow, UserId = "21sd" }
                ];
            List<ShortUrl> expectedResult = shortUrls.Where(x => x.UserId != userId).ToList();

            await repo.AddRangeAsync(shortUrls);

            await repo.DeleteAllByUserIdAsync(userId);

            var result = await repo.GetAllAsync(0, shortUrls.Count);

            Assert.Equal(result, expectedResult);
        }
        
        [Theory]
        [InlineData("11")]
        [InlineData("")]
        [InlineData(null)]
        public async Task DeleteAllByUserIdAsync_ShouldNotDeleteIfUnknownUser(string userId)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new AppDbContext(options);
            var repo = new ShortUrlRepository(context);

            List<ShortUrl> shortUrls =
                [
                    new ShortUrl { Key = "key1", OriginalUrl = "original1", CreatedDate = DateTime.UtcNow, UserId = "21sd" },
                    new ShortUrl { Key = "key2", OriginalUrl = "original2", CreatedDate = DateTime.UtcNow },
                    new ShortUrl { Key = "key3", OriginalUrl = "original3", CreatedDate = DateTime.UtcNow, UserId = "12cx" },
                    new ShortUrl { Key = "key4", OriginalUrl = "original4", CreatedDate = DateTime.UtcNow, UserId = "kodjfj" },
                    new ShortUrl { Key = "key5", OriginalUrl = "original5", CreatedDate = DateTime.UtcNow, UserId = "12cx" },
                    new ShortUrl { Key = "key6", OriginalUrl = "original6", CreatedDate = DateTime.UtcNow, UserId = "12cx" },
                    new ShortUrl { Key = "key7", OriginalUrl = "original7", CreatedDate = DateTime.UtcNow, UserId = "21sd" }
                ];

            await repo.AddRangeAsync(shortUrls);

            await repo.DeleteAllByUserIdAsync(userId);

            var result = await repo.GetAllAsync(0, shortUrls.Count);

            Assert.Equal(result, shortUrls);
        }

        [Fact]
        public async Task DeleteAllByUserIdAsync_ShouldNotDeleteIfNoShortUrlAtAll()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new AppDbContext(options);
            var repo = new ShortUrlRepository(context);

            var userId = "11";
            List<ShortUrl> shortUrls = [];

            await repo.AddRangeAsync(shortUrls);

            await repo.DeleteAllByUserIdAsync(userId);

            var result = await repo.GetAllAsync(0, shortUrls.Count);

            Assert.Equal(result, shortUrls);
        }

        [Fact]
        public async Task ExistAsync_ShouldReturnTrue()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new AppDbContext(options);
            var repo = new ShortUrlRepository(context);

            var shortUrl = new ShortUrl { Key = "key1", OriginalUrl = "original1", CreatedDate = DateTime.UtcNow, UserId = "21sd" };
            List<ShortUrl> shortUrls =
                [
                    shortUrl,
                    new ShortUrl { Key = "key2", OriginalUrl = "original2", CreatedDate = DateTime.UtcNow },
                    new ShortUrl { Key = "key3", OriginalUrl = "original3", CreatedDate = DateTime.UtcNow, UserId = "12cx" },
                    new ShortUrl { Key = "key4", OriginalUrl = "original4", CreatedDate = DateTime.UtcNow, UserId = "kodjfj" },
                    new ShortUrl { Key = "key5", OriginalUrl = "original5", CreatedDate = DateTime.UtcNow, UserId = "12cx" },
                    new ShortUrl { Key = "key6", OriginalUrl = "original6", CreatedDate = DateTime.UtcNow, UserId = "12cx" },
                    new ShortUrl { Key = "key7", OriginalUrl = "original7", CreatedDate = DateTime.UtcNow, UserId = "21sd" }
                ];

            await repo.AddRangeAsync(shortUrls);

            bool result = await repo.ExistAsync(shortUrl.OriginalUrl);
            Assert.True(result);
        }
    
        [Theory]
        [InlineData("original1")]
        [InlineData("")]
        [InlineData(null)]
        public async Task ExistAsync_ShouldReturnFalse(string originalUrl)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new AppDbContext(options);
            var repo = new ShortUrlRepository(context);

            List<ShortUrl> shortUrls =
                [
                    new ShortUrl { Key = "key2", OriginalUrl = "original2", CreatedDate = DateTime.UtcNow },
                    new ShortUrl { Key = "key3", OriginalUrl = "original3", CreatedDate = DateTime.UtcNow, UserId = "12cx" },
                    new ShortUrl { Key = "key4", OriginalUrl = "original4", CreatedDate = DateTime.UtcNow, UserId = "kodjfj" },
                    new ShortUrl { Key = "key5", OriginalUrl = "original5", CreatedDate = DateTime.UtcNow, UserId = "12cx" },
                    new ShortUrl { Key = "key6", OriginalUrl = "original6", CreatedDate = DateTime.UtcNow, UserId = "12cx" },
                    new ShortUrl { Key = "key7", OriginalUrl = "original7", CreatedDate = DateTime.UtcNow, UserId = "21sd" }
                ];

            bool result = await repo.ExistAsync(originalUrl);
            Assert.False(result);
        }
    }
}