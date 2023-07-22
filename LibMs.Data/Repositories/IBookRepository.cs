using System;
using LibMs.Data.Dtos;
using LibMs.Data.Entities;

namespace LibMs.Data.Repositories
{
	public interface IBookRepository : IRepository<Book>, IODataRepository<Book>
    {
    }
}

