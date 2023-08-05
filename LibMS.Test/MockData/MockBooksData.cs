using System;
using LibMs.Data.Dtos;
using LibMs.Data.Entities;

namespace LibMS.Test.MockData
{
    public class MockBooksData
    {
        public static IEnumerable<Book> BookList = new List<Book>() {
            new Book
            {
                BookId = new Guid("00d470c3-e87b-4b7a-a359-e81eaad86ca0"),
                BookName = "Book01",
                Description = "--",
                PageCount = 123,
                PublishDate = 1801,
                TotalCount = 1,
                LoanableCount = 1,
                Authors = new List<Author>() { new Author { AuthorId = new Guid(), AuthorName = "Author01"} },
                Users = null
            },
            new Book
            {
                BookId = new Guid("18404e6b-73a4-4788-a271-41e14f25bdb8"),
                BookName = "Book02",
                Description = "--",
                PageCount = 10,
                PublishDate = 1800,
                TotalCount = 0,
                LoanableCount = 10,
                Authors = new List<Author>() { new Author { AuthorId = new Guid(), AuthorName = "Author02"} },
                Users = null
            },
        };

        public static BookDTO BookToAdd = new BookDTO
        {
            BookName = "Book03",
            Description = "--",
            PageCount = 1,
            PublishDate = 1802,
            LoanableCount = 1,
        };

        public static Book AddedBook = new Book
        {
            BookId = new Guid(),
            BookName = "Book03",
            Description = "--",
            PageCount = 1,
            PublishDate = 1802,
            LoanableCount = 1,
            Authors = new List<Author>() { new Author { AuthorId = new Guid(), AuthorName = "Author02" } },
            Users = null
        };
    }
}

