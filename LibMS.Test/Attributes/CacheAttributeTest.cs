using Xunit;
using Moq;
using LibMS.API.Cache;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using LibMS.API.Attributes;
using Microsoft.AspNetCore.Routing;
using System;
using Microsoft.EntityFrameworkCore;

namespace LibMS.Test
{
    public class CacheAttributeTest
    {

        Mock<ICacheService> _mockCacheService;
        ActionExecutingContext _context;
        ActionExecutedContext _executedContext;

        public CacheAttributeTest() {
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
        public async Task OnActionExecutionAsync_CachedDataFound_ReturnsCachedResult()
        {
            _mockCacheService.Setup(mock => mock.GetAsync<string>(It.IsAny<string>())).ReturnsAsync("Cached Data");


            var attribute = new CacheAttribute();
            attribute.Duration = 10;

            // Act
            await attribute.OnActionExecutionAsync(_context, () => Task.FromResult(_executedContext));

            // Assert
            Assert.IsType<ContentResult>(_context.Result);
            var contentResult = _context.Result as ContentResult;
            Assert.Equal("Cached Data", contentResult.Content);
        }

        [Fact]
        public async Task OnActionExecutionAsync_CachedDataNotFound_SavesAndReturnsActionResult()
        {
            // Arrange
            _mockCacheService.Setup(mock => mock.GetAsync<string>(It.IsAny<string>())).ReturnsAsync((string)null);

            var attribute = new CacheAttribute();
            attribute.Duration = 10;

            // Act
            await attribute.OnActionExecutionAsync(_context, () => Task.FromResult(_executedContext));

            // Assert
            _mockCacheService.Verify(mock => mock.SetAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TimeSpan?>()), Times.Once);
            Assert.IsType<OkObjectResult>(_executedContext.Result);
        }
    }
}
