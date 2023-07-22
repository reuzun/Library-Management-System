using System;
using LibMs.Data.Entities;
using LibMs.Data.Repositories;
using LibMS.Business.Abstracts;

namespace LibMS.Business.Concretes
{
    public class BookService : IBookService
	{
        IRepository<Book> _bookContext;

		public BookService(IRepository<Book> bookRepository)
		{
            _bookContext = bookRepository;
        }

        public IQueryable<Book> GetBooks(Func<IQueryable<Book>, IQueryable<Book>>? query = null)
        {
            return _bookContext.ReadAll();
        }

        public async Task<IEnumerable<Book>> GetBooksAsync(Func<IQueryable<Book>, IQueryable<Book>>? query = null)
        {
            return await _bookContext.AsyncReadAll(query);
        }
    }
}

