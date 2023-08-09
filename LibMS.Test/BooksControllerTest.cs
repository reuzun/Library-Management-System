using System.Collections;
using LibMS.API.Controllers;
using LibMS.Data.Dtos;
using LibMS.Data.Entities;
using LibMS.Business.Abstracts;
using LibMS.Test.MockData;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LibMS.Test
{
    public class BooksControllerTests
    {
        Mock<IBookService> _mockService;
        BooksController _controller;

        public BooksControllerTests()
        {
            // Arrange
            _mockService = new Mock<IBookService>();
            _controller = new BooksController(_mockService.Object);
        }

        [Fact]
        public async Task GetAllBooks_ReturnsListOfBooks()
        {
            // Arrange
            _mockService
                .Setup(service => service.GetBooksAsync(It.IsAny<Func<IQueryable<Book>, IQueryable<Book>>>()))
                .ReturnsAsync(MockBooksData.BookList);

            // Act
            IActionResult actionResult = await _controller.GetAllBooks();

            // Assert
            OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(actionResult);
            var actualBooks = okObjectResult.Value as List<Book>;
            Assert.NotNull(actualBooks);
            Assert.Equal(MockBooksData.BookList.Count(), actualBooks.Count);
        }

        [Fact]
        public async Task GetBookById_ReturnsSingleBookWithGivenId()
        {
            // Arrange
            Book bookById = MockBooksData.BookList.First();

            _mockService
                .Setup(service => service.GetBookById(It.IsAny<Guid>()))
                .ReturnsAsync(bookById);

            // Act
            IActionResult actionResult = await _controller.GetBookById(bookById.BookId);

            // Assert
            OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(actionResult);
            var actualBook = okObjectResult.Value as Book;
            Assert.NotNull(actualBook);
            Assert.Equal(bookById.BookId, actualBook.BookId);
            Assert.Equal(bookById.BookName, actualBook.BookName);
            Assert.Equal(bookById.Description, actualBook.Description);
            Assert.Equal(bookById.PageCount, actualBook.PageCount);
            Assert.Equal(bookById.PublishDate, actualBook.PublishDate);
            Assert.Equal(bookById.LoanableCount, actualBook.LoanableCount);
        }

        [Fact]
        public async Task GetBookById_ReturnsSingleBookWithGivenId_NotFoundCase()
        {
            // Arrange
            Book bookById = MockBooksData.BookList.First();

            _mockService
                .Setup(service => service.GetBookById(It.IsAny<Guid>()))
                .ReturnsAsync((Book)null);

            // Act
            IActionResult actionResult = await _controller.GetBookById(bookById.BookId);

            // Assert
            NotFoundResult okObjectResult = Assert.IsType<NotFoundResult>(actionResult);
        }

        [Fact]
        public async Task AddBook_AddsBookToBooksList()
        {
            // Arrange
            _mockService
                .Setup(service => service.AddBookAsync(It.IsAny<BookDTO>()))
                .ReturnsAsync(MockBooksData.AddedBook);

            // Act
            IActionResult actionResult = await _controller.AddBook(MockBooksData.BookToAdd);

            // Assert
            CreatedResult createdResult = Assert.IsType<CreatedResult>(actionResult);
            var actualBook = createdResult.Value as Book;
            Assert.NotNull(actualBook);

            Assert.Equal(MockBooksData.BookToAdd.BookName, actualBook.BookName);
            Assert.Equal(MockBooksData.BookToAdd.Description, actualBook.Description);
            Assert.Equal(MockBooksData.BookToAdd.PageCount, actualBook.PageCount);
            Assert.Equal(MockBooksData.BookToAdd.PublishDate, actualBook.PublishDate);
            Assert.Equal(MockBooksData.BookToAdd.LoanableCount, actualBook.LoanableCount);
        }

        [Fact]
        public async Task DeleteBook_DeletesSingleBookWithGivenId()
        {
            // Arrange
            _mockService
                .Setup(service => service.DeleteBookAsync(It.IsAny<Guid>()))
                .ReturnsAsync(MockBooksData.BookToAdd);

            Book bookById = MockBooksData.BookList.First();

            // Act
            IActionResult actionResult = await _controller.DeleteBook(bookById.BookId);

            // Assert
            NoContentResult createdResult = Assert.IsType<NoContentResult>(actionResult);
        }

        [Fact]
        public async Task DeleteBook_DeletesSingleBookWithGivenId_NotFoundCase()
        {
            // Arrange
            _mockService
                .Setup(service => service.DeleteBookAsync(It.IsAny<Guid>()))
                .ReturnsAsync((BookDTO) null);

            Book bookById = MockBooksData.BookList.First();

            // Act
            IActionResult actionResult = await _controller.DeleteBook(bookById.BookId);

            // Assert
            NotFoundResult createdResult = Assert.IsType<NotFoundResult>(actionResult);
        }

        [Fact]
        public async Task UpdateBook_UpdatesSingleBookWithGivenId()
        {
            // Arrange
            _mockService
                .Setup(service => service.UpdateBookAsync(It.IsAny<Guid>(), It.IsAny<BookDTO>()))
                .ReturnsAsync(MockBooksData.UpdatedBook);

            Book bookById = MockBooksData.BookList.First();

            // Act
            IActionResult actionResult = await _controller.UpdateBook(bookById.BookId, MockBooksData.UpdateBookDto);

            // Assert
            OkObjectResult createdResult = Assert.IsType<OkObjectResult>(actionResult);
            var updatedBook = createdResult.Value as Book;
            Assert.NotNull(updatedBook);
            Assert.Equal(bookById.BookId, updatedBook.BookId);
            Assert.Equal(MockBooksData.UpdateBookDto.BookName, updatedBook.BookName);
        }

        [Fact]
        public async Task UpdateBook_UpdatesSingleBookWithGivenId_NotFoundCase()
        {
            // Arrange
            _mockService
                .Setup(service => service.UpdateBookAsync(It.IsAny<Guid>(), It.IsAny<BookDTO>()))
                .ReturnsAsync((Book) null);

            Book bookById = MockBooksData.BookList.First();

            // Act
            IActionResult actionResult = await _controller.UpdateBook(bookById.BookId, MockBooksData.UpdateBookDto);

            // Assert
            NotFoundResult createdResult = Assert.IsType<NotFoundResult>(actionResult);
            
        }
    }
}
