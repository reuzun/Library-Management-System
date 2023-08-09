using System;
using System.Runtime;
using LibMS.Data.Dtos;
using LibMS.Business.Abstracts;
using Microsoft.AspNetCore.Mvc;

namespace LibMS.API.Controllers
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
            var user = await _userService.ReadUserAsync(guid);
            if(user != null)
            {
                return Ok(user);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> AddUser([FromBody]UserDTO userDto)
        {
            return Ok(await _userService.AddUserAsync(userDto));
        }

        [HttpPost("{guid}/books")]
        public async Task<IActionResult> RegisterBorrowBook(Guid guid, Guid bookId)
        {
            return Created("", await _userService.BorrowBookAsync(guid, bookId, _settings.AllowedBookLoanCount));
        }
    }
}

