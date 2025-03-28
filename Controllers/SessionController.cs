using Microsoft.AspNetCore.Mvc;

namespace InventoryFinal.Controllers
{
    public class SessionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
