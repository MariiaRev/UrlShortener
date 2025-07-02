using Moq;
using URLShortener.Models;
using URLShortener.Repositories.Interfaces;
using URLShortener.Services;
using URLShortener.Services.Interfaces;

namespace UnitTests.ServicesTests
{
    public class UserUrlServiceTests
    {
        [Fact]
        public async Task CreateShortUrlAsync_ShouldCreateShortUrl()
        {
            var mockRepo = new Mock<IShortUrlRepository>();
            mockRepo.Setup(repo => repo.ExistAsync(It.IsAny<string>()))
                .ReturnsAsync(false);
            mockRepo.Setup(r => r.AddAsync(It.IsAny<ShortUrl>()))
                .Returns(Task.CompletedTask);

            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(s => s.UserExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            var mockShortener = new Mock<IUrlShortenerService>();
            mockShortener.Setup(s => s.CreateShortUrlAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((string url, string userId) => new ShortUrl { Key = "qwerty", OriginalUrl = url, UserId = userId});

            var service = new UserUrlService(mockRepo.Object, mockShortener.Object, mockUserService.Object);
            
            string url = "https://example.com";
            string userId = "rfnjrnfj";

            var result = await service.CreateShortUrlAsync(url, userId);

            Assert.True(result.Success);
            mockRepo.Verify(r => r.AddAsync(It.IsAny<ShortUrl>()), Times.Once);
        }
    
        [Fact]
        public async Task CreateShortUrlAsync_IfUknownUser()
        {
            var mockRepo = new Mock<IShortUrlRepository>();
            mockRepo.Setup(repo => repo.ExistAsync(It.IsAny<string>()))
                .ReturnsAsync(false);
            mockRepo.Setup(r => r.AddAsync(It.IsAny<ShortUrl>()))
                .Returns(Task.CompletedTask);

            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(s => s.UserExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(false);

            var mockShortener = new Mock<IUrlShortenerService>();
            mockShortener.Setup(s => s.CreateShortUrlAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((string url, string userId) => new ShortUrl { Key = "qwerty", OriginalUrl = url, UserId = userId});

            var service = new UserUrlService(mockRepo.Object, mockShortener.Object, mockUserService.Object);
            
            string url = "https://example.com";
            string userId = "rfnjrnfj";

            var result = await service.CreateShortUrlAsync(url, userId);

            Assert.False(result.Success);
            Assert.Equal("Uknown_User", result.ErrorCode);
            mockRepo.Verify(r => r.AddAsync(It.IsAny<ShortUrl>()), Times.Never);
        }
    
        [Fact]
        public async Task CreateShortUrlAsync_IfInvalidUrl()
        {
            var mockRepo = new Mock<IShortUrlRepository>();
            mockRepo.Setup(repo => repo.ExistAsync(It.IsAny<string>()))
                .ReturnsAsync(false);
            mockRepo.Setup(r => r.AddAsync(It.IsAny<ShortUrl>()))
                .Returns(Task.CompletedTask);

            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(s => s.UserExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            var mockShortener = new Mock<IUrlShortenerService>();
            mockShortener.Setup(s => s.CreateShortUrlAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((string url, string userId) => new ShortUrl { Key = "qwerty", OriginalUrl = url, UserId = userId});

            var service = new UserUrlService(mockRepo.Object, mockShortener.Object, mockUserService.Object);
            
            string url = "example.com";
            string userId = "rfnjrnfj";

            var result = await service.CreateShortUrlAsync(url, userId);

            Assert.False(result.Success);
            Assert.Equal("Invalid_Url", result.ErrorCode);
            mockRepo.Verify(r => r.AddAsync(It.IsAny<ShortUrl>()), Times.Never);
        }
        
        [Fact]
        public async Task CreateShortUrlAsync_IfIsNotUniqueUrl()
        {
            var mockRepo = new Mock<IShortUrlRepository>();
            mockRepo.Setup(repo => repo.ExistAsync(It.IsAny<string>()))
                .ReturnsAsync(true);
            mockRepo.Setup(r => r.AddAsync(It.IsAny<ShortUrl>()))
                .Returns(Task.CompletedTask);

            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(s => s.UserExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            var mockShortener = new Mock<IUrlShortenerService>();
            mockShortener.Setup(s => s.CreateShortUrlAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((string url, string userId) => new ShortUrl { Key = "qwerty", OriginalUrl = url, UserId = userId});

            var service = new UserUrlService(mockRepo.Object, mockShortener.Object, mockUserService.Object);
            
            string url = "https://example.com";
            string userId = "rfnjrnfj";

            var result = await service.CreateShortUrlAsync(url, userId);

            Assert.False(result.Success);
            Assert.Equal("Not_Unique_Url", result.ErrorCode);
            mockRepo.Verify(r => r.AddAsync(It.IsAny<ShortUrl>()), Times.Never);
        }
        
        [Fact]
        public async Task CreateShortUrlAsync_IfShortenerError()
        {
            var mockRepo = new Mock<IShortUrlRepository>();
            mockRepo.Setup(repo => repo.ExistAsync(It.IsAny<string>()))
                .ReturnsAsync(false);
            mockRepo.Setup(r => r.AddAsync(It.IsAny<ShortUrl>()))
                .Returns(Task.CompletedTask);

            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(s => s.UserExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            var mockShortener = new Mock<IUrlShortenerService>();
            mockShortener.Setup(s => s.CreateShortUrlAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("Test exception"));

            var service = new UserUrlService(mockRepo.Object, mockShortener.Object, mockUserService.Object);
            
            string url = "https://example.com";
            string userId = "rfnjrnfj";

            var result = await service.CreateShortUrlAsync(url, userId);

            Assert.False(result.Success);
            Assert.Equal("Test exception", result.ErrorMessage);
            mockRepo.Verify(r => r.AddAsync(It.IsAny<ShortUrl>()), Times.Never);
        }
        
        [Fact]
        public async Task CreateShortUrlAsync_IfDbError()
        {
            var mockRepo = new Mock<IShortUrlRepository>();
            mockRepo.Setup(repo => repo.ExistAsync(It.IsAny<string>()))
                .ReturnsAsync(false);
            mockRepo.Setup(r => r.AddAsync(It.IsAny<ShortUrl>()))
                .ThrowsAsync(new Exception("Test exception"));

            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(s => s.UserExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            var mockShortener = new Mock<IUrlShortenerService>();
            mockShortener.Setup(s => s.CreateShortUrlAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((string url, string userId) => new ShortUrl { Key = "qwerty", OriginalUrl = url, UserId = userId });                

            var service = new UserUrlService(mockRepo.Object, mockShortener.Object, mockUserService.Object);
            
            string url = "https://example.com";
            string userId = "rfnjrnfj";

            var result = await service.CreateShortUrlAsync(url, userId);

            Assert.False(result.Success);
            Assert.Equal("Test exception", result.ErrorMessage);
            mockRepo.Verify(r => r.AddAsync(It.IsAny<ShortUrl>()), Times.Once);
        }

    }
}
