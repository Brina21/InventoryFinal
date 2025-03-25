using InventoryFinal.Models;
using InventoryFinal.Service;
using Microsoft.AspNetCore.Mvc;

namespace InventoryFinal.Controllers
{
    public class CompraController : Controller
    {
        private readonly GenericoService<Compra> genericoService;

        public CompraController(GenericoService<Compra> genericoService)
        {
            this.genericoService = genericoService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var (exito, mensaje, compras) = await genericoService.ObtenerTodos();

            if (!exito)
            {
                TempData["Error"] = mensaje;

                return View(new List<Compra>());
            }

            return View(compras);
        }

        [HttpGet]
        public async Task<IActionResult> Detalles(int id)
        {
            var (exito, mensaje, compras) = await genericoService.ObtenerPorId(id);

            if (!exito)
            {
                return NotFound();
            }

            return View(compras);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Compra compra)
        {
            if (!ModelState.IsValid)
            {
                return View(compra);
            }

            var (exito, mensaje, nuevaCompra) = await genericoService.Crear(compra);

            if (!exito)
            {
                ModelState.AddModelError("", mensaje);

                return View(compra);
            }

            return RedirectToAction("Detalles", new { id = nuevaCompra.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var (exito, mensaje, compra) = await genericoService.ObtenerPorId(id);

            if (!exito)
            {
                return NotFound();
            }

            return View(compra);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, Compra compra)
        {
            if (id != compra.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(compra);
            }

            var (exito, mensaje) = await genericoService.Actualizar(compra);

            if (!exito)
            {
                ModelState.AddModelError("", mensaje);

                return View(compra);
            }

            return RedirectToAction("Detalles", new { id = compra.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            var (exito, mensaje, compra) = await genericoService.ObtenerPorId(id);

            if (!exito)
            {
                return NotFound();
            }

            return View(compra);
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