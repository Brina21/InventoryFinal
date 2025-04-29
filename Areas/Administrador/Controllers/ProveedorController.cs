using InventoryFinal.Models;
using InventoryFinal.Service;
using Microsoft.AspNetCore.Mvc;

namespace InventoryFinal.Controllers
{
    [Area("Administrador")]
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

                return View("Index", new List<Proveedor>());
            }

            return View("Index", proveedores);
        }

        [HttpGet]
        public async Task<IActionResult> Detalles(int id)
        {
            var (exito, mensaje, proveedor) = await genericoService.ObtenerPorId(id);

            if (!exito)
            {
                return NotFound();
            }

            return View("Detalles", proveedor);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View("Crear");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Proveedor proveedor)
        {
            if (!ModelState.IsValid)
            {
                return View("Crear", proveedor);
            }

            var (exito, mensaje, nuevoProveedor) = await genericoService.Crear(proveedor);

            if (!exito)
            {
                ModelState.AddModelError("", mensaje);

                return View("Crear", proveedor);
            }

            return RedirectToAction("Detalles", new { id = nuevoProveedor.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var (exito, mensaje, proveedor) = await genericoService.ObtenerPorId(id);

            if (!exito)
            {
                return NotFound();
            }

            return View("Editar", proveedor);
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
                return View("Editar", proveedor);
            }

            var (exito, mensaje) = await genericoService.Actualizar(proveedor);

            if (!exito)
            {
                ModelState.AddModelError("", mensaje);

                return View("Editar", proveedor);
            }

            return RedirectToAction("Detalles", new { id = proveedor.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            var (exito, mensaje, proveedor) = await genericoService.ObtenerPorId(id);

            if (!exito)
            {
                return NotFound();
            }

            return View("Eliminar", proveedor);
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

            return RedirectToAction("Index");
        }
    }
}
