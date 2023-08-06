using System;
using LibMS.Data.Dtos;
using LibMS.Data.Entities;

namespace LibMS.Business.Abstracts
{
	public interface IBookService
	{
		Task<IEnumerable<Book>> GetBooksAsync(Func<IQueryable<Book>, IQueryable<Book>>? query = null);
		IQueryable<Book> GetBooks(Func<IQueryable<Book>, IQueryable<Book>>? query = null);
		Task<Book> GetBookById(Guid bookId);
		Task<Book> AddBookAsync(BookDTO bookDto);
		Task<BookDTO> DeleteBookAsync(Guid bookId);
		Task<Book?> GetBookByBookNameAndAuthorName(string bookName, string authorName);
        Task<Book?> UpdateBookAsync(Guid guid, BookDTO bookDto);
    }
}

