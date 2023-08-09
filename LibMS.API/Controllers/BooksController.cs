using LibMS.API.Attributes;
using LibMS.API.Enums;
using LibMS.Data.Dtos;
using LibMS.Business.Abstracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibMS.API.Controllers
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
        [ResponseCache(Duration = 60)]
        [Cache(Duration = CacheDuration.NODURATION)]
        public async Task<IActionResult> GetAllBooks()
        {
            return Ok(await _bookService.GetBooksAsync(b => b.Include(book => book.Authors)));
        }

        [HttpGet("{guid}")]
        public async Task<IActionResult> GetBookById(Guid guid)
        {
            var book = await _bookService.GetBookById(guid);
            if(book != null)
            {
                return Ok(book);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("")]
        [InvalidateCache("/Books")]
        public async Task<IActionResult> AddBook([FromBody] BookDTO bookDto)
        {
            var book = await _bookService.AddBookAsync(bookDto);
            return Created("/Books/" + book.BookId, book);
        }

        [HttpPut("{guid}")]
        [InvalidateCache("/Books")]
        public async Task<IActionResult> UpdateBook(Guid guid, [FromBody] BookDTO bookDto)
        {
            var book = await _bookService.UpdateBookAsync(guid, bookDto);
            if(book != null)
            {
                return Ok(book);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete("{guid}")]
        [InvalidateCache("/Books")]
        public async Task<IActionResult> DeleteBook(Guid bookId)
        {
            var book = await _bookService.DeleteBookAsync(bookId);
            if(book != null)
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }
    }
}

