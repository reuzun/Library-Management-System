using System;
using Microsoft.AspNetCore.Mvc;

namespace LibMs.API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ServiceController : ControllerBase
    {
        AppSettings _settings;

		public ServiceController(AppSettings settings)
		{
            _settings = settings;;
		}


        [HttpGet("")]
        public IActionResult GetAllBooks()
        {
            return Ok(_settings);
        }
    }
}

