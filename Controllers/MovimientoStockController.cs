using InventoryFinal.Models;
using InventoryFinal.Service;
using Microsoft.AspNetCore.Mvc;

namespace InventoryFinal.Controllers
{
    public class MovimientoStockController : Controller
    {
        private readonly GenericoService<MovimientoStock> genericoService;

        public MovimientoStockController(GenericoService<MovimientoStock> genericoService)
        {
            this.genericoService = genericoService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var (exito, mensaje, movimientoStocks) = await genericoService.ObtenerTodos();

            if (!exito)
            {
                TempData["Error"] = mensaje;

                return View(new List<MovimientoStock>());
            }

            return View(movimientoStocks);
        }

        [HttpGet]
        public async Task<IActionResult> Detalles(int id)
        {
            var (exito, mensaje, movimientoStocks) = await genericoService.ObtenerPorId(id);

            if (!exito)
            {
                return NotFound();
            }

            return View(movimientoStocks);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(MovimientoStock movimientoStock)
        {
            if (!ModelState.IsValid)
            {
                return View(movimientoStock);
            }

            var (exito, mensaje, nuevoMovimientoStock) = await genericoService.Crear(movimientoStock);

            if (!exito)
            {
                ModelState.AddModelError("", mensaje);

                return View(movimientoStock);
            }

            return RedirectToAction("Detalles", new { id = nuevoMovimientoStock.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var (exito, mensaje, movimientoStock) = await genericoService.ObtenerPorId(id);

            if (!exito)
            {
                return NotFound();
            }

            return View(movimientoStock);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, MovimientoStock movimientoStock)
        {
            if (id != movimientoStock.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(movimientoStock);
            }

            var (exito, mensaje) = await genericoService.Actualizar(movimientoStock);

            if (!exito)
            {
                ModelState.AddModelError("", mensaje);

                return View(movimientoStock);
            }

            return RedirectToAction("Detalles", new { id = movimientoStock.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            var (exito, mensaje, movimientoStock) = await genericoService.ObtenerPorId(id);

            if (!exito)
            {
                return NotFound();
            }

            return View(movimientoStock);
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
