using System;
using LibMs.Data.Dtos;
using LibMs.Data.Entities;
using LibMs.Data.Repositories;
using LibMS.Business.Abstracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            return Ok(await _bookService.GetBooksAsync(b => b.Include(book => book.Authors)));
        }

        [HttpGet("{guid}")]
        public async Task<IActionResult> GetBookById(Guid guid)
        {
            return Ok(await _bookService.GetBookById(guid));
        }

        [HttpPost("")]
        public async Task<IActionResult> AddBook([FromBody] BookDTO bookDto)
        {
            return Created("", await _bookService.AddBookAsync(bookDto));
        }

        [HttpPut("{guid}")]
        public async Task<IActionResult> UpdateBook(Guid guid, [FromBody] BookDTO bookDto)
        {
            return Ok(await _bookService.UpdateBookAsync(guid, bookDto));
        }

        [HttpDelete("{guid}")]
        public async Task<IActionResult> DeleteBook(Guid bookId)
        {
            await _bookService.DeleteBookAsync(bookId);
            return Ok();
        }
    }
}

