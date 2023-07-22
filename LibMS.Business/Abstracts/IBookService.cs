using System;
using LibMs.Data.Entities;

namespace LibMS.Business.Abstracts
{
	public interface IBookService
	{
		Task<IEnumerable<Book>> GetBooksAsync(Func<IQueryable<Book>, IQueryable<Book>>? query = null);
		IQueryable<Book> GetBooks(Func<IQueryable<Book>, IQueryable<Book>>? query = null);
    }
}

