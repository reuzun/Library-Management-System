using AutoMapper;
using LibMS.Business.Concretes;
using LibMS.Data.Dtos;
using LibMS.Data.Entities;
using LibMS.Data.Repositories;
using LibMS.Persistance;
using Moq;

namespace LibMS.Test.Business
{
    public class UserServiceTests
    {
        private readonly Mock<IRepository<User>> _mockUserRepository;
        private readonly Mock<IRepository<Book>> _mockBookRepository;
        private readonly Mock<ITransactionManager> _mockTransactionManager;
        private readonly IMapper _mapper;

        public UserServiceTests()
        {
            _mockUserRepository = new Mock<IRepository<User>>();
            _mockBookRepository = new Mock<IRepository<Book>>();
            _mockTransactionManager = new Mock<ITransactionManager>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<LibMS.Data.Mappings.AutoMapperConfiguration>());
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task AddUserAsync_ValidUserDto_ReturnsAddedUser()
        {
            _mockUserRepository.Setup(repo => repo.AsyncCreate(It.IsAny<User>())).ReturnsAsync(new User() { Username = "testuser" });
            // Arrange
            var userDto = new UserDTO { Username = "testuser" };
            var userService = new UserService(_mockUserRepository.Object, _mockBookRepository.Object, _mapper, _mockTransactionManager.Object);

            // Act
            var addedUser = await userService.AddUserAsync(userDto);

            // Assert
            Assert.NotNull(addedUser);
            Assert.Equal(userDto.Username, addedUser.Username);
        }

        [Fact]
        public async Task BorrowBookAsync_UserNotFound_ThrowsException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var bookId = Guid.NewGuid();
            _mockUserRepository.Setup(repo => repo.AsyncReadFirst(It.IsAny<Func<IQueryable<User>, IQueryable<User>>>()))
                .ReturnsAsync((User)null);

            var userService = new UserService(_mockUserRepository.Object, _mockBookRepository.Object, _mapper, _mockTransactionManager.Object);

            // Act and Assert
            await Assert.ThrowsAsync<Exception>(() => userService.BorrowBookAsync(userId, bookId));
        }

        [Fact]
        public async Task BorrowBookAsync_UserReachedMaxLoanCount_ThrowsException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var bookId = Guid.NewGuid();
            ushort maxAllowedBookLoanCount = 2;
            var user = new User { UserId = userId, LoanedBooks = new List<Book> { new Book(), new Book() } };

            _mockUserRepository.Setup(repo => repo.AsyncReadFirst(It.IsAny<Func<IQueryable<User>, IQueryable<User>>>()))
                .ReturnsAsync(user);

            var userService = new UserService(_mockUserRepository.Object, _mockBookRepository.Object, _mapper, _mockTransactionManager.Object);

            // Act and Assert
            await Assert.ThrowsAsync<Exception>(() => userService.BorrowBookAsync(userId, bookId, maxAllowedBookLoanCount));
        }

        [Fact]
        public async Task BorrowBookAsync_ValidIds_ExistingBookList_ReturnsUpdatedUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var bookId = Guid.NewGuid();
            ushort maxAllowedBookLoanCount = 3;
            var user = new User { UserId = userId, LoanedBooks = new List<Book>() };
            var book = new Book { BookId = bookId };
            _mockUserRepository.Setup(repo => repo.AsyncReadFirst(It.IsAny<Func<IQueryable<User>, IQueryable<User>>>()))
                .ReturnsAsync(user);
            _mockBookRepository.Setup(repo => repo.AsyncReadFirst(It.IsAny<Func<IQueryable<Book>, IQueryable<Book>>>()))
                .ReturnsAsync(book);
            _mockUserRepository.Setup(repo => repo.AsyncUpdate(It.IsAny<Guid>(), It.IsAny<User>()))
                .ReturnsAsync(user);

            var userService = new UserService(_mockUserRepository.Object, _mockBookRepository.Object, _mapper, _mockTransactionManager.Object);

            // Act
            var updatedUser = await userService.BorrowBookAsync(userId, bookId, maxAllowedBookLoanCount);

            // Assert
            Assert.NotNull(updatedUser);
            Assert.Contains(book, updatedUser.LoanedBooks);
        }

        [Fact]
        public async Task BorrowBookAsync_ValidIds_NoBookList_ReturnsUpdatedUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var bookId = Guid.NewGuid();
            ushort maxAllowedBookLoanCount = 3;
            var user = new User { UserId = userId };
            var book = new Book { BookId = bookId };
            _mockUserRepository.Setup(repo => repo.AsyncReadFirst(It.IsAny<Func<IQueryable<User>, IQueryable<User>>>()))
                .ReturnsAsync(user);
            _mockBookRepository.Setup(repo => repo.AsyncReadFirst(It.IsAny<Func<IQueryable<Book>, IQueryable<Book>>>()))
                .ReturnsAsync(book);
            _mockUserRepository.Setup(repo => repo.AsyncUpdate(It.IsAny<Guid>(), It.IsAny<User>()))
                .ReturnsAsync(user);

            var userService = new UserService(_mockUserRepository.Object, _mockBookRepository.Object, _mapper, _mockTransactionManager.Object);

            // Act
            var updatedUser = await userService.BorrowBookAsync(userId, bookId, maxAllowedBookLoanCount);

            // Assert
            Assert.NotNull(updatedUser);
            Assert.Contains(book, updatedUser.LoanedBooks);
        }

        [Fact]
        public async Task BorrowBookAsync_UserAlreadyLoanedBook_ThrowsException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var bookId = Guid.NewGuid();
            var book = new Book { BookId = bookId };
            var user = new User { UserId = userId, LoanedBooks = new List<Book> { book } };

            _mockUserRepository.Setup(repo => repo.AsyncReadFirst(It.IsAny<Func<IQueryable<User>, IQueryable<User>>>()))
                .ReturnsAsync(user);

            _mockBookRepository.Setup(repo => repo.AsyncReadFirst(It.IsAny<Func<IQueryable<Book>, IQueryable<Book>>>()))
                .ReturnsAsync(book);

            var userService = new UserService(_mockUserRepository.Object, _mockBookRepository.Object, _mapper, _mockTransactionManager.Object);

            // Act and Assert
            await Assert.ThrowsAsync<Exception>(() => userService.BorrowBookAsync(userId, bookId));
        }

        [Fact]
        public async Task ReadUserAsync_ExistingUserId_ReturnsUser()
        {
            // Arrange
            var existingUserId = Guid.NewGuid();
            var expectedUser = new User { UserId = existingUserId };
            _mockUserRepository.Setup(repo => repo.AsyncReadFirst(It.IsAny<Func<IQueryable<User>, IQueryable<User>>>()))
                .ReturnsAsync(expectedUser);

            var userService = new UserService(_mockUserRepository.Object, _mockBookRepository.Object, _mapper, _mockTransactionManager.Object);

            // Act
            var user = await userService.ReadUserAsync(existingUserId);

            // Assert
            Assert.NotNull(user);
            Assert.Equal(existingUserId, user.UserId);
        }

        [Fact]
        public async Task ReadUserAsync_NonExistingUserId_ReturnsNull()
        {
            // Arrange
            var nonExistingUserId = Guid.NewGuid();
            _mockUserRepository.Setup(repo => repo.AsyncReadFirst(It.IsAny<Func<IQueryable<User>, IQueryable<User>>>()))
                .ReturnsAsync((User)null);

            var userService = new UserService(_mockUserRepository.Object, _mockBookRepository.Object, _mapper, _mockTransactionManager.Object);

            // Act
            var user = await userService.ReadUserAsync(nonExistingUserId);

            // Assert
            Assert.Null(user);
        }
    }
}
