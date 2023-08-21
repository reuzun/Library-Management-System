using LibMS.API.Attributes;
using LibMS.API.Cache;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace LibMS.Test.Attributes
{
    public class CacheInvalidateAttributeTest
    {

        Mock<ICacheService> _mockCacheService;
        ActionExecutingContext _context;
        ActionExecutedContext _executedContext;

        public CacheInvalidateAttributeTest()
        {
            _mockCacheService = new Mock<ICacheService>();

            var serviceProvider = new ServiceCollection()
               .AddSingleton<ICacheService>(_mockCacheService.Object)
               .BuildServiceProvider();

            var httpContext = new DefaultHttpContext();
            _context = new ActionExecutingContext(
                new ActionContext(httpContext, new RouteData(), new ActionDescriptor()),
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                new Mock<Controller>().Object);


            _context.HttpContext.RequestServices = serviceProvider;

            _executedContext = new ActionExecutedContext(
            new ActionContext(httpContext, new RouteData(), new ActionDescriptor()),
            new List<IFilterMetadata>(), new Mock<Controller>().Object)
            {
                Result = new OkObjectResult("Action Result")
            };
        }

        [Fact]
        public async Task RemoveFromCache()
        {
            _mockCacheService.Setup(mock => mock.GetAsync<string>(It.IsAny<string>())).ReturnsAsync("Cached Data");

            var attribute = new InvalidateCacheAttribute("");

            // Act
            await attribute.OnActionExecutionAsync(_context, () => Task.FromResult(_executedContext));

            // Assert
            _mockCacheService.Verify(ser => ser.RemoveAsync(It.IsAny<string>()), Times.Once);
        }
    }
}
