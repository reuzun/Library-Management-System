using System;
using System.Data;
using AutoMapper;
using LibMS.Data.Dtos;
using LibMS.Data.Entities;
using LibMS.Data.Repositories;
using LibMS.Persistance;
using LibMS.Business.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace LibMS.Business.Concretes
{
	public class UserService : IUserService
	{
        IRepository<User> _userRepository;
        IRepository<Book> _bookRepository;
        ITransactionManager _transactionManager;
        IMapper _mapper;

		public UserService(IRepository<User> userRepository, IRepository<Book> bookRepository, IMapper mapper, ITransactionManager transactionManager)
		{
            _userRepository = userRepository;
            _bookRepository = bookRepository;
            _mapper = mapper;
            _transactionManager = transactionManager;
		}

        public async Task<User> AddUserAsync(UserDTO userDto)
        {
            return await _userRepository.AsyncCreate(_mapper.Map(userDto, new User()));
        }

        public async Task<User> BorrowBookAsync(Guid userId, Guid bookId, ushort maxAllowedBookLoanCount = 3)
        {
            User userToReturn;

            await _transactionManager.BeginTransactionAsync(IsolationLevel.Serializable);
            
            var user = await _userRepository.AsyncReadFirst(q => q.Include(user => user.LoanedBooks).Where(user => user.UserId == userId));
            var book = await _bookRepository.AsyncReadFirst(q => q.Where(book => book.BookId == bookId));

            if (user == null)
            {
                throw new Exception("UserNotFound");
            }

            if (user.LoanedBooks == null)
            {
                user.LoanedBooks = new List<Book>();

                if (book != null)
                {
                    user.LoanedBooks.Add(book);
                }
            }
            else
            {
                if (user.LoanedBooks.Count() >= maxAllowedBookLoanCount)
                {
                    throw new Exception("User is not allowed to loan more books.");
                }

                if (book != null && user.LoanedBooks.Contains(book))
                {
                    throw new Exception("User already loaned this book!");
                }

                if (book != null)
                {
                    user.LoanedBooks.Add(book);
                }
            }

            userToReturn = await _userRepository.AsyncUpdate(user.UserId, user);

            await _transactionManager.CommitTransactionAsync();

            return userToReturn;
        }

        public async Task<User?> ReadUserAsync(Guid id)
        {
            var user = await _userRepository.AsyncReadFirst(q => q.Include(user => user.LoanedBooks).Where(user => user.UserId == id));
            if(user == null)
            {
                return null;
            }
            return user;
        }
    }
}

