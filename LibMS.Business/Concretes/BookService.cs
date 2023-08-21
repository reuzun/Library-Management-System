using AutoMapper;
using LibMS.Business.Abstracts;
using LibMS.Data.Dtos;
using LibMS.Data.Entities;
using LibMS.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LibMS.Business.Concretes
{
    public class BookService : IBookService
    {
        IRepository<Book> _bookRepository;
        IMapper _mapper;

        public BookService(IRepository<Book> bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public Task<Book> AddBookAsync(BookDTO bookDto)
        {
            return _bookRepository.AsyncCreate(_mapper.Map<Book>(bookDto));
        }

        public async Task<BookDTO?> DeleteBookAsync(Guid bookId)
        {
            var book = await GetBookById(bookId);
            if (book != null)
            {
                await _bookRepository.AsyncRemove(book.BookId);
                return new BookDTO { };
            }
            else
            {
                return null;
            }
        }

        public IQueryable<Book> GetBooks(Func<IQueryable<Book>, IQueryable<Book>>? query = null)
        {
            if (query != null)
            {
                return query(_bookRepository.ReadAll().Include(book => book.Authors));
            }
            return _bookRepository.ReadAll().Include(book => book.Authors);
        }

        public async Task<IEnumerable<Book>> GetBooksAsync(Func<IQueryable<Book>, IQueryable<Book>>? query = null)
        {
            if (query != null)
            {
                return (await _bookRepository.AsyncReadAll(b => query(b.Include(book => book.Authors))));
            }
            return (await _bookRepository.AsyncReadAll(b => b.Include(book => book.Authors)));
        }

        public async Task<Book?> GetBookById(Guid bookId)
        {
            var book = await _bookRepository.AsyncReadFirst(b => b.Where(book => book.BookId == bookId));
            if (book != null)
            {
                return book;
            }
            else
            {
                return null;
            }
        }

        public async Task<Book?> UpdateBookAsync(Guid guid, BookDTO bookDto)
        {
            return await _bookRepository.AsyncUpdate(guid, _mapper.Map(bookDto, new Book()));
        }
    }
}

