using System;
using System.Runtime;
using LibMs.Data.Dtos;
using LibMS.Business.Abstracts;
using Microsoft.AspNetCore.Mvc;

namespace LibMs.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
	{
        IUserService _userService;
        AppSettings _settings;

		public UsersController(IUserService userService, AppSettings settings)
		{
            _userService = userService;
            _settings = settings;
		}

        [HttpGet("{guid}")]
        public async Task<IActionResult> GetUser(Guid guid)
        {
            return Ok(await _userService.ReadUser(guid));
        }

        [HttpPost("")]
        public async Task<IActionResult> AddUser([FromBody]UserDTO userDto)
        {
            return Ok(await _userService.AddUser(userDto));
        }

        [HttpPost("{guid}/books")]
        public async Task<IActionResult> RegisterBorrowBook(Guid guid, Guid bookId)
        {
            return Created("", await _userService.BorrowBook(guid, bookId, _settings.AllowedBookLoanCount));
        }
    }
}

