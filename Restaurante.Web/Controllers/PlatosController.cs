using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Restaurante.Web.Controllers
{
    [Authorize]
    public class PlatosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
