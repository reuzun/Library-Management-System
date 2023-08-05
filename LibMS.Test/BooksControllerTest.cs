using System.Collections;
using LibMs.API.Controllers;
using LibMs.Data.Dtos;
using LibMs.Data.Entities;
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
            OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(actionResult);

            // Assert
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
            OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(actionResult);

            // Assert
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
        public async Task AddBook_AddsBookToBooksList()
        {
            // Arrange
            _mockService
                .Setup(service => service.AddBookAsync(It.IsAny<BookDTO>()))
                .ReturnsAsync(MockBooksData.AddedBook);

            // Act
            IActionResult actionResult = await _controller.AddBook(MockBooksData.BookToAdd);
            CreatedResult createdResult = Assert.IsType<CreatedResult>(actionResult);

            // Assert
            var actualBook = createdResult.Value as Book;
            Assert.NotNull(actualBook);

            Assert.Equal(MockBooksData.BookToAdd.BookName, actualBook.BookName);
            Assert.Equal(MockBooksData.BookToAdd.Description, actualBook.Description);
            Assert.Equal(MockBooksData.BookToAdd.PageCount, actualBook.PageCount);
            Assert.Equal(MockBooksData.BookToAdd.PublishDate, actualBook.PublishDate);
            Assert.Equal(MockBooksData.BookToAdd.LoanableCount, actualBook.LoanableCount);
        }
    }
}
