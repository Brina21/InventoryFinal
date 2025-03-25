using InventoryFinal.Models;
using InventoryFinal.Service;
using Microsoft.AspNetCore.Mvc;

namespace InventoryFinal.Controllers
{
    public class VentaController : Controller
    {
        private readonly GenericoService<Venta> genericoService;

        public VentaController(GenericoService<Venta> genericoService)
        {
            this.genericoService = genericoService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var (exito, mensaje, ventas) = await genericoService.ObtenerTodos();

            if (!exito)
            {
                TempData["Error"] = mensaje;

                return View(new List<Venta>());
            }

            return View(ventas);
        }

        [HttpGet]
        public async Task<IActionResult> Detalles(int id)
        {
            var (exito, mensaje, ventas) = await genericoService.ObtenerPorId(id);

            if (!exito)
            {
                return NotFound();
            }

            return View(ventas);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Venta venta)
        {
            if (!ModelState.IsValid)
            {
                return View(venta);
            }

            var (exito, mensaje, nuevaVenta) = await genericoService.Crear(venta);

            if (!exito)
            {
                ModelState.AddModelError("", mensaje);

                return View(venta);
            }

            return RedirectToAction("Detalles", new { id = nuevaVenta.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var (exito, mensaje, venta) = await genericoService.ObtenerPorId(id);

            if (!exito)
            {
                return NotFound();
            }

            return View(venta);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, Venta venta)
        {
            if (id != venta.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(venta);
            }

            var (exito, mensaje) = await genericoService.Actualizar(venta);

            if (!exito)
            {
                ModelState.AddModelError("", mensaje);

                return View(venta);
            }

            return RedirectToAction("Detalles", new { id = venta.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            var (exito, mensaje, venta) = await genericoService.ObtenerPorId(id);

            if (!exito)
            {
                return NotFound();
            }

            return View(venta);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
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
