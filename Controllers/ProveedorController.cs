using InventoryFinal.Models;
using InventoryFinal.Service;
using Microsoft.AspNetCore.Mvc;

namespace InventoryFinal.Controllers
{
    public class ProveedorController : Controller
    {
        private readonly GenericoService<Proveedor> genericoService;

        public ProveedorController(GenericoService<Proveedor> genericoService)
        {
            this.genericoService = genericoService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var (exito, mensaje, proveedores) = await genericoService.ObtenerTodos();

            if (!exito)
            {
                TempData["Error"] = mensaje;

                return View("Views/Administrador/Proveedor/Index.cshtml", new List<Proveedor>());
            }

            return View("Views/Administrador/Proveedor/Index.cshtml", proveedores);
        }

        [HttpGet]
        public async Task<IActionResult> Detalles(int id)
        {
            var (exito, mensaje, proveedor) = await genericoService.ObtenerPorId(id);

            if (!exito)
            {
                return NotFound();
            }

            return View("Views/Administrador/Proveedor/Detalles.cshtml", proveedor);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View("Views/Administrador/Proveedor/Crear.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Proveedor proveedor)
        {
            if (!ModelState.IsValid)
            {
                return View("Views/Administrador/Proveedor/Crear.cshtml", proveedor);
            }

            var (exito, mensaje, nuevoProveedor) = await genericoService.Crear(proveedor);

            if (!exito)
            {
                ModelState.AddModelError("", mensaje);

                return View("Views/Administrador/Proveedor/Crear.cshtml", proveedor);
            }

            return RedirectToAction("Views/Administrador/Proveedor/Detalles.cshtml", new { id = nuevoProveedor.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var (exito, mensaje, proveedor) = await genericoService.ObtenerPorId(id);

            if (!exito)
            {
                return NotFound();
            }

            return View("Views/Administrador/Proveedor/Editar.cshtml", proveedor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, Proveedor proveedor)
        {
            if (id != proveedor.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View("Views/Administrador/Proveedor/Editar.cshtml", proveedor);
            }

            var (exito, mensaje) = await genericoService.Actualizar(proveedor);

            if (!exito)
            {
                ModelState.AddModelError("", mensaje);

                return View("Views/Administrador/Proveedor/Editar.cshtml", proveedor);
            }

            return RedirectToAction("Views/Administrador/Proveedor/Detalles.cshtml", new { id = proveedor.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            var (exito, mensaje, proveedor) = await genericoService.ObtenerPorId(id);

            if (!exito)
            {
                return NotFound();
            }

            return View("Views/Administrador/Proveedor/Eliminar.cshtml", proveedor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ELiminarConfirmado(int id)
        {
            var (exito, mensaje) = await genericoService.Eliminar(id);

            if (!exito)
            {
                TempData["Error"] = mensaje;
            }

            return RedirectToAction("Views/Administrador/Proveedor/Index.cshtml");
        }
    }
}
