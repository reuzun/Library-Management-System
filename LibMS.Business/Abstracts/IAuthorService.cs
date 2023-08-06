using System;
using LibMS.Data.Dtos;
using LibMS.Data.Entities;

namespace LibMS.Business.Abstracts
{
	public interface IAuthorService
	{
        Task<IEnumerable<Author>> GetAuthorsAsync(Func<IQueryable<Author>, IQueryable<Author>>? query = null);
        IQueryable<Author> GetAuthors(Func<IQueryable<Author>, IQueryable<Author>>? query = null);
        Task<Author> GetAuthorById(Guid bookId);
        Task<Author> AddAuthorAsync(AuthorDTO authorDto);
		Task<Author> UpdateAuthorAsync(Guid id, AuthorDTO authorDto);
	}
}

