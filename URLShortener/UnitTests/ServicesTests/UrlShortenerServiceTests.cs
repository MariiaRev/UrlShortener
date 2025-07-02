using Moq;
using URLShortener.Models;
using URLShortener.Repositories.Interfaces;
using URLShortener.Services;

namespace UnitTests.ServicesTests
{
    public class UrlShortenerServiceTests
    {
        [Theory]
        [InlineData("https://example.com", "100680")]
        [InlineData("dvsv", "e8638b")]
        public async Task GenerateKeyAsync_IfKeyDoesNotExistAndCorrectUrl(string url, string expectedResult)
        {
            var mockRepo = new Mock<IShortUrlRepository>();
            mockRepo.Setup(repo => repo.ExistKeyAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(false));

            var service = new UrlShortenerService(mockRepo.Object);

            var result = await service.GenerateKeyAsync(url);

            Assert.NotNull(result);
            Assert.Equal(expectedResult, result);
        }
    
        [Theory]
        [InlineData("", "e3b0c4")]
        [InlineData(null, "e3b0c4")]
        public async Task GenerateKeyAsync_IfKeyDoesNotExistAndIncorrectUrl(string url, string expectedResult)
        {
            var mockRepo = new Mock<IShortUrlRepository>();
            mockRepo.Setup(repo => repo.ExistKeyAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(false));

            var service = new UrlShortenerService(mockRepo.Object);

            var result = await service.GenerateKeyAsync(url);

            Assert.NotNull(result);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("https://example.com", "100680", "614fb0")]
        [InlineData("dvsv", "e8638b", "64687d")]
        public async Task GenerateKeyAsync_IfKeyExistsAndCorrectUrl(string url, string firstResult, string expectedResult)
        {
            var mockRepo = new Mock<IShortUrlRepository>();

            mockRepo.Setup(repo => repo.ExistKeyAsync(It.Is<string>(key => key == firstResult)))
                .Returns(Task.FromResult(true));
            
            mockRepo.Setup(repo => repo.ExistKeyAsync(It.Is<string>(key => key != firstResult)))
                .Returns(Task.FromResult(false));

            var service = new UrlShortenerService(mockRepo.Object);

            var result = await service.GenerateKeyAsync(url);

            Assert.NotNull(result);
            Assert.Equal(expectedResult, result);
        }
        
        [Theory]
        [InlineData("", "e3b0c4", "6b86b2")]
        [InlineData(null, "e3b0c4", "6b86b2")]
        public async Task GenerateKeyAsync_IfKeyExistsAndIncorrectUrl(string url, string firstResult, string expectedResult)
        {
            var mockRepo = new Mock<IShortUrlRepository>();

            mockRepo.Setup(repo => repo.ExistKeyAsync(It.Is<string>(key => key == firstResult)))
                .Returns(Task.FromResult(true));
            
            mockRepo.Setup(repo => repo.ExistKeyAsync(It.Is<string>(key => key != firstResult)))
                .Returns(Task.FromResult(false));

            var service = new UrlShortenerService(mockRepo.Object);

            var result = await service.GenerateKeyAsync(url);

            Assert.NotNull(result);
            Assert.Equal(expectedResult, result);
        }        

        [Theory]
        [InlineData("https://example.com", "100680", "")]
        [InlineData("https://example.com", "100680", "jdhfi")]
        [InlineData("dvsv", "e8638b", null)]
        public async Task CreateShortUrlAsync_IfKeyDoesNotExistAndCorrectUrl(string url, string expectedKey, string? userId)
        {
            var mockRepo = new Mock<IShortUrlRepository>();
            mockRepo.Setup(repo => repo.ExistKeyAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(false));

            var service = new UrlShortenerService(mockRepo.Object);
            ShortUrl expectedResult = new() { Key = expectedKey, OriginalUrl = url, UserId = userId };

            var result = await service.CreateShortUrlAsync(url, userId);

            Assert.NotNull(result);
            Assert.Equal(expectedResult, result);
        }
    
        [Theory]
        [InlineData("", "e3b0c4", "")]
        [InlineData("", "e3b0c4", "jdhfi")]
        [InlineData(null, "e3b0c4", null)]
        public async Task CreateShortUrlAsync_IfKeyDoesNotExistAndInCorrectUrl(string url, string expectedKey, string? userId)
        {
            var mockRepo = new Mock<IShortUrlRepository>();
            mockRepo.Setup(repo => repo.ExistKeyAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(false));

            var service = new UrlShortenerService(mockRepo.Object);
            ShortUrl expectedResult = new() { Key = expectedKey, OriginalUrl = url, UserId = userId };

            var result = await service.CreateShortUrlAsync(url, userId);

            Assert.NotNull(result);
            Assert.Equal(expectedResult, result);
        }


        [Theory]
        [InlineData("https://example.com", "100680", "614fb0", "")]
        [InlineData("https://example.com", "100680", "614fb0", "jdhfi")]
        [InlineData("dvsv", "e8638b", "64687d", null)]
        public async Task CreateShortUrlAsync_IfKeyExistsAndCorrectUrl(string url, string firstResult, string expectedKey, string? userId)
        {
            var mockRepo = new Mock<IShortUrlRepository>(); 
            
            mockRepo.Setup(repo => repo.ExistKeyAsync(It.Is<string>(key => key == firstResult)))
                .Returns(Task.FromResult(true));

            mockRepo.Setup(repo => repo.ExistKeyAsync(It.Is<string>(key => key != firstResult)))
                .Returns(Task.FromResult(false));

            var service = new UrlShortenerService(mockRepo.Object);
            ShortUrl expectedResult = new() { Key = expectedKey, OriginalUrl = url, UserId = userId };

            var result = await service.CreateShortUrlAsync(url, userId);

            Assert.NotNull(result);
            Assert.Equal(expectedResult, result);
        }
        
        [Theory]
        [InlineData("", "e3b0c4", "6b86b2", "")]
        [InlineData(null, "e3b0c4", "6b86b2", "jdhfi")]
        [InlineData("", "e3b0c4", "6b86b2", null)]
        public async Task CreateShortUrlAsync_IfKeyExistsAndIncorrectUrl(string url, string firstResult, string expectedKey, string? userId)
        {
            var mockRepo = new Mock<IShortUrlRepository>(); 
            
            mockRepo.Setup(repo => repo.ExistKeyAsync(It.Is<string>(key => key == firstResult)))
                .Returns(Task.FromResult(true));

            mockRepo.Setup(repo => repo.ExistKeyAsync(It.Is<string>(key => key != firstResult)))
                .Returns(Task.FromResult(false));

            var service = new UrlShortenerService(mockRepo.Object);
            ShortUrl expectedResult = new() { Key = expectedKey, OriginalUrl = url, UserId = userId };

            var result = await service.CreateShortUrlAsync(url, userId);

            Assert.NotNull(result);
            Assert.Equal(expectedResult, result);
        }

    }
}
