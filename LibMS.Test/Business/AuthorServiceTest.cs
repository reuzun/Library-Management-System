using AutoMapper;
using LibMS.Business.Concretes;
using LibMS.Data.Dtos;
using LibMS.Data.Entities;
using LibMS.Data.Repositories;
using Moq;
using System.ComponentModel.DataAnnotations;

namespace LibMS.Test.Business
{
    public class AuthorServiceTests
    {
        private readonly Mock<IRepository<Author>> _mockAuthorRepository;
        private readonly IMapper _mapper;

        public AuthorServiceTests()
        {
            _mockAuthorRepository = new Mock<IRepository<Author>>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<LibMS.Data.Mappings.AutoMapperConfiguration>());
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task AddAuthorAsync_ValidAuthorDto_ReturnsAddedAuthor()
        {
            // Arrange
            _mockAuthorRepository.Setup(rep => rep.AsyncCreate(It.IsAny<Author>())).ReturnsAsync(new Author() { AuthorName = "John Doe" });
            var authorDto = new AuthorDTO { AuthorName = "John Doe" };
            var authorService = new AuthorService(_mockAuthorRepository.Object, _mapper);

            // Act
            var addedAuthor = await authorService.AddAuthorAsync(authorDto);

            // Assert
            Assert.NotNull(addedAuthor);
            Assert.Equal(authorDto.AuthorName, addedAuthor.AuthorName);
        }

        [Fact]
        public async Task AddAuthorAsync_NullAuthorName_ThrowsValidationException()
        {
            // Arrange
            var authorDto = new AuthorDTO { AuthorName = null };
            var authorService = new AuthorService(_mockAuthorRepository.Object, _mapper);

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => authorService.AddAuthorAsync(authorDto));
        }

        [Fact]
        public async Task GetAuthorByIdAsync_ExistingAuthorId_ReturnsAuthor()
        {
            // Arrange
            var existingAuthorId = Guid.NewGuid();
            var expectedAuthor = new Author { AuthorId = existingAuthorId };
            _mockAuthorRepository.Setup(repo => repo.AsyncReadFirst(It.IsAny<Func<IQueryable<Author>, IQueryable<Author>>>()))
                .ReturnsAsync(expectedAuthor);

            var authorService = new AuthorService(_mockAuthorRepository.Object, _mapper);

            // Act
            var author = await authorService.GetAuthorByIdAsync(existingAuthorId);

            // Assert
            Assert.NotNull(author);
            Assert.Equal(existingAuthorId, author.AuthorId);
        }

        [Fact]
        public async Task GetAuthorByIdAsync_NonExistingAuthorId_ThrowsException()
        {
            // Arrange
            var nonExistingAuthorId = Guid.NewGuid();
            _mockAuthorRepository.Setup(repo => repo.AsyncReadFirst(It.IsAny<Func<IQueryable<Author>, IQueryable<Author>>>()))
                .ReturnsAsync((Author)null);

            var authorService = new AuthorService(_mockAuthorRepository.Object, _mapper);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => authorService.GetAuthorByIdAsync(nonExistingAuthorId));
        }

        [Fact]
        public async Task UpdateAuthorAsync_ExistingAuthorId_ReturnsUpdatedAuthor()
        {
            // Arrange
            var existingAuthorId = Guid.NewGuid();
            var updatedAuthorDto = new AuthorDTO { AuthorName = "Updated Author" };
            var originalAuthor = new Author { AuthorId = existingAuthorId, AuthorName = "Original Author" };
            var expectedUpdatedAuthor = new Author { AuthorId = existingAuthorId, AuthorName = "Updated Author" };

            _mockAuthorRepository.Setup(repo => repo.AsyncUpdate(It.IsAny<Guid>(), It.IsAny<Author>()))
                .ReturnsAsync(expectedUpdatedAuthor);

            var authorService = new AuthorService(_mockAuthorRepository.Object, _mapper);

            // Act
            var updatedAuthor = await authorService.UpdateAuthorAsync(existingAuthorId, updatedAuthorDto);

            // Assert
            Assert.NotNull(updatedAuthor);
            Assert.Equal(expectedUpdatedAuthor.AuthorName, updatedAuthor.AuthorName);
        }

        [Fact]
        public async Task UpdateAuthorAsync_NonExistingAuthorId_ReturnsNull()
        {
            // Arrange
            var nonExistingAuthorId = Guid.NewGuid();
            var updatedAuthorDto = new AuthorDTO { AuthorName = "Updated Author" };

            _mockAuthorRepository.Setup(repo => repo.AsyncUpdate(It.IsAny<Guid>(), It.IsAny<Author>()))
                .ReturnsAsync((Author)null);

            var authorService = new AuthorService(_mockAuthorRepository.Object, _mapper);

            // Act
            var updatedAuthor = await authorService.UpdateAuthorAsync(nonExistingAuthorId, updatedAuthorDto);

            // Assert
            Assert.Null(updatedAuthor);
        }

        [Fact]
        public void GetAuthors_NoQuery_ReturnsAllAuthors()
        {
            // Arrange
            var authors = new List<Author>
            {
                new Author { AuthorId = Guid.NewGuid(), AuthorName = "Author 1" },
                new Author { AuthorId = Guid.NewGuid(), AuthorName = "Author 2" }
            }.AsQueryable();

            _mockAuthorRepository.Setup(repo => repo.ReadAll()).Returns(authors);

            var authorService = new AuthorService(_mockAuthorRepository.Object, _mapper);

            // Act
            var result = authorService.GetAuthors();

            // Assert
            Assert.Equal(authors.Count(), result.Count());
        }

        [Fact]
        public void GetAuthors_Query_ReturnsAllAuthors()
        {
            // Arrange
            var authors = new List<Author>
            {
                new Author { AuthorId = Guid.NewGuid(), AuthorName = "Author 1" },
            }.AsQueryable();

            _mockAuthorRepository.Setup(repo => repo.ReadAll()).Returns(authors);

            var authorService = new AuthorService(_mockAuthorRepository.Object, _mapper);

            // Act
            var result = authorService.GetAuthors((e => e.Where(aut => aut.AuthorName.Contains("1"))));

            // Assert
            Assert.Equal(1, result.Count());
        }

        [Fact]
        public async Task GetAuthorsAsync_NoQuery_ReturnsAllAuthors()
        {
            // Arrange
            var authors = new List<Author>
            {
                new Author { AuthorId = Guid.NewGuid(), AuthorName = "Author 1" },
                new Author { AuthorId = Guid.NewGuid(), AuthorName = "Author 2" }
            }.AsQueryable();

            _mockAuthorRepository.Setup(repo => repo.AsyncReadAll(It.IsAny<Func<IQueryable<Author>, IQueryable<Author>>>()))
                .ReturnsAsync(authors);

            var authorService = new AuthorService(_mockAuthorRepository.Object, _mapper);

            // Act
            var result = await authorService.GetAuthorsAsync();

            // Assert
            Assert.Equal(authors.Count(), result.Count());
        }

        [Fact]
        public async Task GetAuthorsAsync_Query_ReturnsAllAuthors()
        {
            // Arrange
            var authors = new List<Author>
            {
                new Author { AuthorId = Guid.NewGuid(), AuthorName = "Author 1" },
            }.AsQueryable();

            _mockAuthorRepository.Setup(repo => repo.AsyncReadAll(It.IsAny<Func<IQueryable<Author>, IQueryable<Author>>>()))
                .ReturnsAsync(authors);

            var authorService = new AuthorService(_mockAuthorRepository.Object, _mapper);

            // Act
            var result = await authorService.GetAuthorsAsync(e => e.Where(aut => aut.AuthorName.Contains("1")));

            // Assert
            Assert.Equal(1, result.Count());
        }
    }
}
