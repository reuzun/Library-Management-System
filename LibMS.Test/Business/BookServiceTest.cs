using AutoMapper;
using LibMS.Business.Concretes;
using LibMS.Data.Dtos;
using LibMS.Data.Entities;
using LibMS.Data.Repositories;
using Moq;

namespace LibMS.Test.Business
{
    public class BookServiceTests
    {
        private readonly Mock<IRepository<Book>> _mockBookRepository;
        private readonly IMapper _mapper;

        public BookServiceTests()
        {
            _mockBookRepository = new Mock<IRepository<Book>>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<LibMS.Data.Mappings.AutoMapperConfiguration>());
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task AddBookAsync_ValidBookDto_ReturnsAddedBook()
        {
            // Arrange
            _mockBookRepository.Setup(bookRep => bookRep.AsyncCreate(It.IsAny<Book>())).ReturnsAsync(new Book { BookId = Guid.NewGuid(), BookName = "Book 1" });
            var bookDto = new BookDTO { BookName = "Book 1" };
            var bookService = new BookService(_mockBookRepository.Object, _mapper);

            // Act
            var addedBook = await bookService.AddBookAsync(bookDto);

            // Assert
            Assert.NotNull(addedBook);
            Assert.Equal(bookDto.BookName, addedBook.BookName);
        }

        [Fact]
        public async Task GetBooksAsync_NoQuery_ReturnsAllBooks()
        {
            // Arrange
            var books = new List<Book>
            {
                new Book { BookId = Guid.NewGuid(), BookName = "Book 1" },
                new Book { BookId = Guid.NewGuid(), BookName = "Book 2" }
            }.AsQueryable();

            _mockBookRepository.Setup(repo => repo.AsyncReadAll(It.IsAny<Func<IQueryable<Book>, IQueryable<Book>>>()))
                .ReturnsAsync(books);

            var bookService = new BookService(_mockBookRepository.Object, _mapper);

            // Act
            var result = await bookService.GetBooksAsync();

            // Assert
            Assert.Equal(books.Count(), result.Count());
        }

        [Fact]
        public async Task GetBooksAsync_Query_ReturnsAllBooks()
        {
            // Arrange
            var books = new List<Book>
            {
                new Book { BookId = Guid.NewGuid(), BookName = "Book 1" },
            }.AsQueryable();

            _mockBookRepository.Setup(repo => repo.AsyncReadAll(It.IsAny<Func<IQueryable<Book>, IQueryable<Book>>>()))
                .ReturnsAsync(books);

            var bookService = new BookService(_mockBookRepository.Object, _mapper);

            // Act
            var result = await bookService.GetBooksAsync(q => q.Where(b => b.BookName.Contains("1")));

            // Assert
            Assert.Equal(books.Count(), result.Count());
        }

        [Fact]
        public void GetBooks_NoQuery_ReturnsAllBooks()
        {
            // Arrange
            var books = new List<Book>
            {
                new Book { BookId = Guid.NewGuid(), BookName = "Book 1" },
                new Book { BookId = Guid.NewGuid(), BookName = "Book 2" }
            }.AsQueryable();

            _mockBookRepository.Setup(repo => repo.ReadAll()).Returns(books);

            var bookService = new BookService(_mockBookRepository.Object, _mapper);

            // Act
            var result = bookService.GetBooks();

            // Assert
            Assert.Equal(books.Count(), result.Count());
        }

        [Fact]
        public void GetBooks_Query_ReturnsAllBooks()
        {
            // Arrange
            var books = new List<Book>
            {
                new Book { BookId = Guid.NewGuid(), BookName = "Book 1" },
            }.AsQueryable();

            _mockBookRepository.Setup(repo => repo.ReadAll()).Returns(books);

            var bookService = new BookService(_mockBookRepository.Object, _mapper);

            // Act
            var result = bookService.GetBooks(q => q.Where(b => b.BookName.Contains("1")));

            // Assert
            Assert.Equal(books.Count(), result.Count());
        }

        [Fact]
        public async Task GetBookById_ExistingBookId_ReturnsBook()
        {
            // Arrange
            var existingBookId = Guid.NewGuid();
            var expectedBook = new Book { BookId = existingBookId };
            _mockBookRepository.Setup(repo => repo.AsyncReadFirst(It.IsAny<Func<IQueryable<Book>, IQueryable<Book>>>()))
                .ReturnsAsync(expectedBook);

            var bookService = new BookService(_mockBookRepository.Object, _mapper);

            // Act
            var book = await bookService.GetBookById(existingBookId);

            // Assert
            Assert.NotNull(book);
            Assert.Equal(existingBookId, book.BookId);
        }

        [Fact]
        public async Task GetBookById_NonExistingBookId_ReturnsNull()
        {
            // Arrange
            var nonExistingBookId = Guid.NewGuid();
            _mockBookRepository.Setup(repo => repo.AsyncReadFirst(It.IsAny<Func<IQueryable<Book>, IQueryable<Book>>>()))
                .ReturnsAsync((Book)null);

            var bookService = new BookService(_mockBookRepository.Object, _mapper);

            // Act
            var book = await bookService.GetBookById(nonExistingBookId);

            // Assert
            Assert.Null(book);
        }

        [Fact]
        public async Task UpdateBookAsync_ExistingBookId_ReturnsUpdatedBook()
        {
            // Arrange
            var existingBookId = Guid.NewGuid();
            var updatedBookDto = new BookDTO { BookName = "Updated Book" };
            var originalBook = new Book { BookId = existingBookId, BookName = "Original Book" };
            var expectedUpdatedBook = new Book { BookId = existingBookId, BookName = "Updated Book" };

            _mockBookRepository.Setup(repo => repo.AsyncUpdate(It.IsAny<Guid>(), It.IsAny<Book>()))
                .ReturnsAsync(expectedUpdatedBook);

            var bookService = new BookService(_mockBookRepository.Object, _mapper);

            // Act
            var updatedBook = await bookService.UpdateBookAsync(existingBookId, updatedBookDto);

            // Assert
            Assert.NotNull(updatedBook);
            Assert.Equal(expectedUpdatedBook.BookName, updatedBook.BookName);
        }

        [Fact]
        public async Task UpdateBookAsync_NonExistingBookId_ReturnsNull()
        {
            // Arrange
            var nonExistingBookId = Guid.NewGuid();
            var updatedBookDto = new BookDTO { BookName = "Updated Book" };

            _mockBookRepository.Setup(repo => repo.AsyncUpdate(It.IsAny<Guid>(), It.IsAny<Book>()))
                .ReturnsAsync((Book)null);

            var bookService = new BookService(_mockBookRepository.Object, _mapper);

            // Act
            var updatedBook = await bookService.UpdateBookAsync(nonExistingBookId, updatedBookDto);

            // Assert
            Assert.Null(updatedBook);
        }

        [Fact]
        public async Task DeleteBookAsync_ExistingBookId_ReturnsDeletedBookDto()
        {
            // Arrange
            var existingBookId = Guid.NewGuid();
            var existingBook = new Book { BookId = existingBookId };
            _mockBookRepository.Setup(repo => repo.AsyncReadFirst(It.IsAny<Func<IQueryable<Book>, IQueryable<Book>>>()))
                .ReturnsAsync(existingBook);

            var bookService = new BookService(_mockBookRepository.Object, _mapper);

            // Act
            var deletedBookDto = await bookService.DeleteBookAsync(existingBookId);

            // Assert
            Assert.NotNull(deletedBookDto);
        }

        [Fact]
        public async Task DeleteBookAsync_NonExistingBookId_ReturnsNull()
        {
            // Arrange
            var nonExistingBookId = Guid.NewGuid();
            _mockBookRepository.Setup(repo => repo.AsyncReadFirst(It.IsAny<Func<IQueryable<Book>, IQueryable<Book>>>()))
                .ReturnsAsync((Book)null);

            var bookService = new BookService(_mockBookRepository.Object, _mapper);

            // Act
            var deletedBookDto = await bookService.DeleteBookAsync(nonExistingBookId);

            // Assert
            Assert.Null(deletedBookDto);
        }
    }
}
