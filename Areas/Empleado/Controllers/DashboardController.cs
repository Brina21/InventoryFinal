using Microsoft.AspNetCore.Mvc;

namespace InventoryFinal.Areas.Empleado.Controllers
{
    [Area("Empleado")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.NombreUsuario = HttpContext.Session.GetString("NombreUsuario");
            return View(); // Areas/Administrador/Views/Dashboard/Index.cshtml
        }
    }
}
