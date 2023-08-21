using Microsoft.AspNetCore.Mvc;

namespace LibMS.API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ServiceController : ControllerBase
    {
        AppSettings _settings;

        public ServiceController(AppSettings settings)
        {
            _settings = settings; ;
        }

        [HttpGet("info")]
        public IActionResult GetInfo()
        {
            return Ok(new { Service = "LibMS" });
        }
    }
}

