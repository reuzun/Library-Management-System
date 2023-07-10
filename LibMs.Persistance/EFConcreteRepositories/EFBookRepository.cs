using System;
using LibMs.Data.Dtos;
using LibMs.Data.Entities;
using LibMs.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LibMs.Persistance.EFConcreteRepositories
{
    public class EFBookRepository : IBookRepository
	{
        LibMSContext _context;

		public EFBookRepository(LibMSContext context)
		{
            _context = context;
		}

        public void AsyncCreate(Book book)
        {
            _context.Set<Book>().AddAsync(book);
            _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Book>> AsyncReadAll(Func<IQueryable<Book>, IQueryable<Book>>? query = null)
        {
            IQueryable<Book> books = _context.Set<Book>();

            if(query != null)
            {
                books = query(books);
            }

            return await books.ToListAsync();
        }

        public async Task<Book?> AsyncReadFirst(Func<IQueryable<Book>, IQueryable<Book>>? query = null)
        {
            IQueryable<Book> books = _context.Set<Book>();

            if (query != null)
            {
                books = query(books);
            }

            return await books.FirstOrDefaultAsync();
        }

        public async void AsyncRemove(Guid objId)
        {
            var book = await _context.Set<Book>().FindAsync(objId);
            if (book != null)
            {
                _context.Set<Book>().Remove(book);
                await _context.SaveChangesAsync();
            }
        }

        public async void AsyncUpdate(Guid objId, object obj)
        {
            var existingBook = await _context.Set<Book>().FindAsync(objId);
            BookDTO bookDTO = (BookDTO)obj;

            if (existingBook != null)
            {
                // Güncelleme işlemleri yapılabilir
                existingBook.BookName = bookDTO.BookName;
                existingBook.Description = bookDTO.Description;
                existingBook.PageCount = bookDTO.PageCount;
                existingBook.PublishDate = bookDTO.PublishDate;
                existingBook.LoanableCount = bookDTO.LoanableCount;
                await _context.SaveChangesAsync();
            }
        }
    }
}

