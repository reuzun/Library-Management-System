using System;
using LibMS.API.Controllers;
using LibMS.Business.Abstracts;
using LibMS.Data.Dtos;
using LibMS.Data.Entities;
using LibMS.Test.MockData;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LibMS.Test
{
    public class AuthorControllerTest
    {
        Mock<IAuthorService> _mockService;
        AuthorsController _authorsController;

        public AuthorControllerTest()
        {
            _mockService = new Mock<IAuthorService>();
            _authorsController = new AuthorsController(_mockService.Object);
        }

        [Fact]
        public async Task GetAllAuthors_ReturnsListOfAuthors()
        {
            _mockService
                .Setup(service => service.GetAuthorsAsync(It.IsAny<Func<IQueryable<Author>, IQueryable<Author>>>()))
                .ReturnsAsync(MockAuthorsData.Authors);

            var authorsResponse = await _authorsController.GetAllAuthors();

            var authorsResponseObject = Assert.IsType<OkObjectResult>(authorsResponse);
            var authorsList = authorsResponseObject.Value as List<Author>;
            Assert.Equal(MockAuthorsData.Authors.Count(), authorsList.Count);
        }

        [Fact]
        public async Task GetAuthorById_ReturnsSingleAuthor()
        {
            _mockService
                .Setup(service => service.GetAuthorByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(MockAuthorsData.author);

            var authorResponse = await _authorsController.GetAuthorById(new Guid());

            var authorsResponseObject = Assert.IsType<OkObjectResult>(authorResponse);
            var author = authorsResponseObject.Value as Author;
            Assert.Equal(MockAuthorsData.author.AuthorId, author.AuthorId);
            Assert.Equal(MockAuthorsData.author.AuthorName, author.AuthorName);
            Assert.Equal(MockAuthorsData.author.BirthYear, author.BirthYear);
        }

        [Fact]
        public async Task GetAuthorById_ReturnsSingleAuthor_NotFound()
        {
            _mockService
                 .Setup(service => service.GetAuthorByIdAsync(It.IsAny<Guid>()))
                 .ReturnsAsync((Author) null);

            var authorResponse = await _authorsController.GetAuthorById(new Guid());

            var authorsResponseObject = Assert.IsType<NotFoundResult>(authorResponse);
        }

        [Fact]
        public async Task AddAuthors_ReturnsAddedAuthor()
        {
            _mockService
                 .Setup(service => service.AddAuthorAsync(It.IsAny<AuthorDTO>()))
                 .ReturnsAsync(MockAuthorsData.author);

            var authorResponse = await _authorsController.AddAuthor(new AuthorDTO());


            var authorsResponseObject = Assert.IsType<CreatedResult>(authorResponse);
            var author = authorsResponseObject.Value as Author;

            Assert.NotNull(author.AuthorId);
            Assert.Equal(MockAuthorsData.author.AuthorName, author.AuthorName);
            Assert.Equal(MockAuthorsData.author.BirthYear, author.BirthYear);
        }

        [Fact]
        public async Task UpdateAuthor_ReturnsUpdatedAuthor()
        {
            _mockService
                 .Setup(service => service.UpdateAuthorAsync(It.IsAny<Guid>(), It.IsAny<AuthorDTO>()))
                 .ReturnsAsync(MockAuthorsData.author);


            var authorResponse = await _authorsController.UpdateAuthor(new Guid(), new AuthorDTO());

            var authorsResponseObject = Assert.IsType<OkObjectResult>(authorResponse);
            var updatedAuthor = authorsResponseObject.Value as Author;
            Assert.Equal(MockAuthorsData.author.AuthorId, updatedAuthor.AuthorId);
            Assert.Equal(MockAuthorsData.author.AuthorName, updatedAuthor.AuthorName);
            Assert.Equal(MockAuthorsData.author.BirthYear, updatedAuthor.BirthYear);
        }

        [Fact]
        public async Task UpdateAuthor_ReturnsUpdatedAuthor_NotFound()
        {
            _mockService
                 .Setup(service => service.UpdateAuthorAsync(It.IsAny<Guid>(), It.IsAny<AuthorDTO>()))
                 .ReturnsAsync((Author) null);


            var authorResponse = await _authorsController.UpdateAuthor(new Guid(), new AuthorDTO());

            var authorsResponseObject = Assert.IsType<NotFoundResult>(authorResponse);
        }
    }
}

