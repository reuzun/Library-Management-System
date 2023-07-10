using System;
using LibMs.Data.Entities;
using LibMs.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LibMs.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BooksController : ControllerBase
	{
		IBookRepository _bookRepository;

		public BooksController(IBookRepository bookRepository)
		{
			_bookRepository = bookRepository;
		}

        [HttpGet("")]
        public async Task<IActionResult> GetAllBooks()
        {
            return Ok(await _bookRepository.AsyncReadAll());
        }

        [HttpGet("{guid}")]
		public async Task<IActionResult> GetBookById(Guid guid)
		{
            return Ok(await _bookRepository.AsyncReadFirst(query => query.Where(b => b.BookId == guid)));
		}
    }
}

