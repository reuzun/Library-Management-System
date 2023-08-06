using System;
using LibMS.Data.Dtos;
using LibMS.Data.Entities;

namespace LibMS.Business.Abstracts
{
	public interface IUserService
	{
		Task<User> ReadUser(Guid id);
        Task<User> AddUser(UserDTO userDto);
        Task<User> BorrowBook(Guid userId, Guid bookId, ushort maxAllowedBookLoanCount = 3);
	}
}

