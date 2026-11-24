using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Restaurante.Web.Controllers.Api
{
    [Route("api/health")]
    public class HealthController : ApiController
    {
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            return Ok(new { status = "API is working", utcTime = DateTime.UtcNow });
        }
    }
}