using Microsoft.AspNetCore.Mvc;

namespace InventoryFinal.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            var nombreUsuario = HttpContext.Session.GetString("NombreUsuario");
            var rol = HttpContext.Session.GetString("Rol");
            Console.WriteLine($"Nombre de usuario: {nombreUsuario}");
            Console.WriteLine($"Rol: {rol}");

            ViewBag.NombreUsuario = nombreUsuario;

            // Redirección por rol
            if (rol == "Administrador")
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Administrador" });
            }
            else if (rol == "Empleado")
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Empleado" });
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
