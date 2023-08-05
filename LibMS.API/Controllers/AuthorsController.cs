using System;
using LibMs.Data.Dtos;
using LibMS.Business.Abstracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibMs.API.Controllers
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
            return Ok(await _authorService.GetAuthorById(guid));
        }

        [HttpPost("")]
        public async Task<IActionResult> AddAuthor([FromBody] AuthorDTO authorDto)
        {
            return Created("", await _authorService.AddAuthorAsync(authorDto));
        }

        [HttpPut("{guid}")]
        public async Task<IActionResult> UpdateAuthor(Guid guid, [FromBody] AuthorDTO authorDto)
        {
            return Ok(await _authorService.UpdateAuthorAsync(guid, authorDto));
        }
	}
}

