using Microsoft.AspNetCore.Mvc;

namespace InventoryFinal.Areas.Administrador.Controllers
{
    [Area("Administrador")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.NombreUsuario = HttpContext.Session.GetString("NombreUsuario");
            return View(); //Areas/Administrador/Views/Dashboard/Index.cshtml
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}
