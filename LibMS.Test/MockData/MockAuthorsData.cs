using System;
using LibMS.Data.Entities;

namespace LibMS.Test.MockData
{
    public class MockAuthorsData
    {
        public static IEnumerable<Author> Authors = new List<Author>(){
            new Author {
                AuthorId = new Guid(),
                AuthorName = "Test_Name_01",
                WrittenBooks = MockBooksData.BookList
            },
            new Author {
                AuthorId = new Guid(),
                AuthorName = "Test_Name_02",
            }
        };

        public static Author author = new Author {
            AuthorId = new Guid(),
            AuthorName = "Test_Name_03",
            WrittenBooks = MockBooksData.BookList
        };
    }
}

