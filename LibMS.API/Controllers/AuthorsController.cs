using System;
using LibMS.Data.Dtos;
using LibMS.Business.Abstracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibMS.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorsController : ControllerBase
	{
		IAuthorService _authorService;

		public AuthorsController(IAuthorService authorService)
		{
			_authorService = authorService;
		}

		[HttpGet("")]
        public async Task<IActionResult> GetAllAuthors()
        {
            return Ok(await _authorService.GetAuthorsAsync(b => b.Include(author => author.WrittenBooks)));
        }

        [HttpGet("{guid}")]
        public async Task<IActionResult> GetAuthorById(Guid guid)
        {
            var author = await _authorService.GetAuthorByIdAsync(guid);
            if(author != null)
            {
                return Ok(author);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> AddAuthor([FromBody] AuthorDTO authorDto)
        {
            return Created("", await _authorService.AddAuthorAsync(authorDto));
        }

        [HttpPut("{guid}")]
        public async Task<IActionResult> UpdateAuthor(Guid guid, [FromBody] AuthorDTO authorDto)
        {
            var author = await _authorService.UpdateAuthorAsync(guid, authorDto);
            if(author != null)
            {
                return Ok(author);
            }else
            {
                return NotFound();
            }
        }
	}
}

