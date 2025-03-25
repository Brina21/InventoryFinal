using InventoryFinal.Models;
using InventoryFinal.Service;
using Microsoft.AspNetCore.Mvc;

namespace InventoryFinal.Controllers
{
    public class DetalleCompraController : Controller
    {
        private readonly GenericoService<DetalleCompra> genericoService;

        public DetalleCompraController(GenericoService<DetalleCompra> genericoService)
        {
            this.genericoService = genericoService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var (exito, mensaje, detalleCompras) = await genericoService.ObtenerTodos();

            if (!exito)
            {
                TempData["Error"] = mensaje;

                return View(new List<DetalleCompra>());
            }

            return View(detalleCompras);
        }

        [HttpGet]
        public async Task<IActionResult> Detalles(int id)
        {
            var (exito, mensaje, detalleCompras) = await genericoService.ObtenerPorId(id);

            if (!exito)
            {
                return NotFound();
            }

            return View(detalleCompras);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(DetalleCompra detalleCompra)
        {
            if (!ModelState.IsValid)
            {
                return View(detalleCompra);
            }

            var (exito, mensaje, nuevoDetalleCompra) = await genericoService.Crear(detalleCompra);

            if (!exito)
            {
                ModelState.AddModelError("", mensaje);

                return View(detalleCompra);
            }

            return RedirectToAction("Detalles", new { id = nuevoDetalleCompra.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var (exito, mensaje, detalleCompra) = await genericoService.ObtenerPorId(id);

            if (!exito)
            {
                return NotFound();
            }

            return View(detalleCompra);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, DetalleCompra detalleCompra)
        {
            if (id != detalleCompra.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(detalleCompra);
            }

            var (exito, mensaje) = await genericoService.Actualizar(detalleCompra);

            if (!exito)
            {
                ModelState.AddModelError("", mensaje);

                return View(detalleCompra);
            }

            return RedirectToAction("Detalles", new { id = detalleCompra.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            var (exito, mensaje, detalleCompra) = await genericoService.ObtenerPorId(id);

            if (!exito)
            {
                return NotFound();
            }

            return View(detalleCompra);
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