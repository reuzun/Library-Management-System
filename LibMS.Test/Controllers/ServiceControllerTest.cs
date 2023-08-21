using LibMS.API;
using LibMS.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LibMS.Test.Controllers
{
    public class ServiceControllerTest
    {
        Mock<AppSettings> _mockSettings;
        ServiceController _serviceController;

        public ServiceControllerTest()
        {
            // Arrange
            _mockSettings = new Mock<AppSettings>();
            _serviceController = new ServiceController(_mockSettings.Object);
        }

        [Fact]
        public void GetInfo_ShouldReturnServiceInfo()
        {
            var result = _serviceController.GetInfo();

            var answer = Assert.IsType<OkObjectResult>(result);

            Assert.NotNull(answer);
            //Assert.Equal(answer.Value, "LibMS");
            Assert.Equal(new { Service = "LibMS" }.ToString(), answer.Value.ToString());
        }


    }
}
