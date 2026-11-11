using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Restaurante.Web.Controllers
{
    [Authorize]
    public class PedidosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
