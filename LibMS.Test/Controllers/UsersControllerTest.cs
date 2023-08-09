using System;
using System.Runtime;
using LibMS.API;
using LibMS.API.Controllers;
using LibMS.Business.Abstracts;
using LibMS.Data.Dtos;
using LibMS.Data.Entities;
using LibMS.Test.MockData;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LibMS.Test
{
    public class UsersControllerTest
    {
        Mock<AppSettings> _settings;
        Mock<IUserService> _mockService;
        UsersController _usersController;

        public UsersControllerTest()
        {
            // Arrange
            _mockService = new Mock<IUserService>();
            _settings = new Mock<AppSettings>();
            _usersController = new UsersController(_mockService.Object, _settings.Object);
        }

        [Fact]
        public async Task GetUserById_ReturnsSingleUser()
        {
            _mockService
                .Setup(service => service.ReadUserAsync(It.IsAny<Guid>()))
                .ReturnsAsync(MockUsersData.user);

            var userResponse = await _usersController.GetUser(new Guid());

            var userResponseObject = Assert.IsType<OkObjectResult>(userResponse);
            var user = userResponseObject.Value as User;

            Assert.NotNull(user.UserId);
            Assert.Equal(user.UserId, MockUsersData.user.UserId);
            Assert.Equal(user.Username, MockUsersData.user.Username);
            Assert.Equal(user.Email, MockUsersData.user.Email);
        }

        [Fact]
        public async Task GetUserById_ReturnsSingleUser_NotFound()
        {
            _mockService
                .Setup(service => service.ReadUserAsync(It.IsAny<Guid>()))
                .ReturnsAsync((User) null);

            var userResponse = await _usersController.GetUser(new Guid());

            var userResponseObject = Assert.IsType<NotFoundResult>(userResponse);
        }

        [Fact]
        public async Task AddUser_ReturnsAddedUser()
        {
            _mockService
                .Setup(service => service.AddUserAsync(It.IsAny<UserDTO>()))
                .ReturnsAsync(MockUsersData.user);

            var userResponse = await _usersController.AddUser(MockUsersData.userToAdd);

            var userResponseObject = Assert.IsType<OkObjectResult>(userResponse);
            var user = userResponseObject.Value as User;

            Assert.NotNull(user.UserId);
        }

        [Fact]
        public async Task RegisterBorrowBook_ReturnsUser()
        {
            _mockService
                .Setup(service => service.BorrowBookAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<ushort>()))
                .ReturnsAsync(MockUsersData.user);

            var userResponse = await _usersController.RegisterBorrowBook(new Guid(), new Guid());

            var userResponseObject = Assert.IsType<CreatedResult>(userResponse);
            var user = userResponseObject.Value as User;

            Assert.NotNull(user.UserId);
            Assert.Equal(user.UserId, MockUsersData.user.UserId);
            Assert.Equal(user.Username, MockUsersData.user.Username);
            Assert.Equal(user.Email, MockUsersData.user.Email);
        }
    }
}

