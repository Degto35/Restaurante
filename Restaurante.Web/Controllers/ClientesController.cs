using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Restaurante.Web.Controllers
{
    [Authorize]
    public class ClientesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
