using System;
using System.Transactions;
using AutoMapper;
using LibMs.Data.Dtos;
using LibMs.Data.Entities;
using LibMs.Data.Repositories;
using LibMS.Business.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace LibMS.Business.Concretes
{
	public class UserService : IUserService
	{
        IRepository<User> _userRepository;
        IRepository<Book> _bookRepository;
        IMapper _mapper;

		public UserService(IRepository<User> userRepository, IRepository<Book> bookRepository, IMapper mapper)
		{
            _userRepository = userRepository;
            _bookRepository = bookRepository;
            _mapper = mapper;
		}

        public async Task<User> AddUser(UserDTO userDto)
        {
            return await _userRepository.AsyncCreate(_mapper.Map(userDto, new User()));
        }

        public async Task<User> BorrowBook(Guid userId, Guid bookId)
        {
            User userToReturn;
            using (var scope = new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.Serializable
                },
                TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var user = await _userRepository.AsyncReadFirst(q => q.Include(user => user.LoanedBooks).Where(user => user.UserId == userId));
                    var book = await _bookRepository.AsyncReadFirst(q => q.Where(book => book.BookId == bookId));

                    if (user == null)
                    {
                        throw new Exception("UserNotFound");
                    }

                    if (user.LoanedBooks == null)
                    {
                        user.LoanedBooks = new List<Book>();
                    }

                    if (user.LoanedBooks.Count() > 3)
                    {
                        throw new Exception("User is not allowed to loan more books.");
                    }

                    if (book != null)
                    {
                        user.LoanedBooks.Add(book);
                    }

                    userToReturn = await _userRepository.AsyncUpdate(user.UserId, user);

                    scope.Complete(); // Commit the transaction only if everything is successful
                }
                catch (Exception ex)
                {
                    // Handle the exception, log it, or take appropriate action
                    // The transaction will be rolled back automatically when the using block ends.
                    throw ex;
                }
                return userToReturn;
            }

        }

        public async Task<User> ReadUser(Guid id)
        {
            var user = await _userRepository.AsyncReadFirst(q => q.Include(user => user.LoanedBooks).Where(user => user.UserId == id));
            if(user == null)
            {
                throw new Exception("User not Found!");
            }
            return user;
        }
    }
}

