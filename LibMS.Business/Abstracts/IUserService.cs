using System;
using LibMS.Data.Dtos;
using LibMS.Data.Entities;

namespace LibMS.Business.Abstracts
{
	public interface IUserService
	{
		Task<User?> ReadUserAsync(Guid id);
        Task<User> AddUserAsync(UserDTO userDto);
        Task<User> BorrowBookAsync(Guid userId, Guid bookId, ushort maxAllowedBookLoanCount = 3);
	}
}

