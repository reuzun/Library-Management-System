using System;
using LibMs.Data.Entities;
using LibMs.Data.Repositories;
using LibMS.Business.Abstracts;
using Microsoft.AspNetCore.Mvc;

namespace LibMs.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BooksController : ControllerBase
	{
        IBookService _bookService;

		public BooksController(IBookService bookService)
		{
            _bookService = bookService;
		}

        [HttpGet("")]
        public async Task<IActionResult> GetAllBooks()
        {
            return Ok(await _bookService.GetBooksAsync());
        }

        [HttpGet("{guid}")]
		public async Task<IActionResult> GetBookById(Guid guid)
		{
            return Ok(await _bookService.GetBooksAsync(query => query.Where(b => b.BookId == guid)));
		}
    }
}

