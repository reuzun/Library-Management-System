using System;
using LibMs.Data.Dtos;
using LibMs.Data.Entities;

namespace LibMS.Business.Abstracts
{
	public interface IUserService
	{
		Task<User> ReadUser(Guid id);
        Task<User> AddUser(UserDTO userDto);
        Task<User> BorrowBook(Guid userId, Guid bookId);
	}
}

