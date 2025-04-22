using InventoryFinal.Models;
using InventoryFinal.Service;
using Microsoft.AspNetCore.Mvc;

namespace InventoryFinal.Controllers
{
    public class DetalleVentaController : Controller
    {
        private readonly GenericoService<DetalleVenta> genericoService;

        public DetalleVentaController(GenericoService<DetalleVenta> genericoService)
        {
            this.genericoService = genericoService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var (exito, mensaje, detalleVentas) = await genericoService.ObtenerTodos();

            if (!exito)
            {
                TempData["Error"] = mensaje;

                return View("Views/Administrador/DetalleVenta/Index.cshtml", new List<DetalleVenta>());
            }

            return View("Views/Administrador/DetalleVenta/Index.cshtml", detalleVentas);
        }

        [HttpGet]
        public async Task<IActionResult> Detalles(int id)
        {
            var (exito, mensaje, detalleVentas) = await genericoService.ObtenerPorId(id);

            if (!exito)
            {
                return NotFound();
            }

            return View("Views/Administrador/DetalleVenta/Detalles.cshtml", detalleVentas);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View("Views/Administrador/DetalleVenta/Crear.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(DetalleVenta detalleVenta)
        {
            if (!ModelState.IsValid)
            {
                return View("Views/Administrador/DetalleVenta/Crear.cshtml", detalleVenta);
            }

            var (exito, mensaje, nuevoDetalleVenta) = await genericoService.Crear(detalleVenta);

            if (!exito)
            {
                ModelState.AddModelError("", mensaje);

                return View("Views/Administrador/DetalleVenta/Crear.cshtml", detalleVenta);
            }

            return RedirectToAction("Views/Administrador/DetalleVenta/Detalles.cshtml", new { id = nuevoDetalleVenta.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var (exito, mensaje, detalleVenta) = await genericoService.ObtenerPorId(id);

            if (!exito)
            {
                return NotFound();
            }

            return View("Views/Administrador/DetalleVenta/Editar.cshtml", detalleVenta);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, DetalleVenta detalleVenta)
        {
            if (id != detalleVenta.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View("Views/Administrador/DetalleVenta/Editar.cshtml", detalleVenta);
            }

            var (exito, mensaje) = await genericoService.Actualizar(detalleVenta);

            if (!exito)
            {
                ModelState.AddModelError("", mensaje);

                return View("Views/Administrador/DetalleVenta/Editar.cshtml", detalleVenta);
            }

            return RedirectToAction("Views/Administrador/DetalleVenta/Detalles.cshtml", new { id = detalleVenta.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            var (exito, mensaje, detalleVenta) = await genericoService.ObtenerPorId(id);

            if (!exito)
            {
                return NotFound();
            }

            return View("Views/Administrador/DetalleVenta/Eliminar.cshtml", detalleVenta);
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

            return RedirectToAction("Views/Administrador/DetalleVenta/Index.cshtml");
        }
    }
}
