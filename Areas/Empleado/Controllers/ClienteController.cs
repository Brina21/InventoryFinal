using InventoryFinal.Models;
using InventoryFinal.Service;
using Microsoft.AspNetCore.Mvc;

namespace InventoryFinal.Areas.Empleado.Controllers
{
    [Area("Empleado")]
    public class ClienteController : Controller
    {
        private readonly GenericoService<Cliente> genericoService;

        public ClienteController(GenericoService<Cliente> genericoService)
        {
            this.genericoService = genericoService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var (exito, mensaje, clientes) = await genericoService.ObtenerTodos();

            if (!exito)
            {
                TempData["Error"] = mensaje;

                return View("Index", new List<Cliente>());
            }

            return View("Index", clientes);
        }

        [HttpGet]
        public async Task<IActionResult> Detalles(int id)
        {
            var (exito, mensaje, clientes) = await genericoService.ObtenerPorId(id);

            if (!exito)
            {
                return NotFound();
            }

            return View("Detalles", clientes);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View("Crear");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Cliente cliente)
        {
            if (!ModelState.IsValid)
            {
                return View("Crear", cliente);
            }

            var (exito, mensaje, nuevoCliente) = await genericoService.Crear(cliente);

            if (!exito)
            {
                ModelState.AddModelError("", mensaje);

                return View("Crear", cliente);
            }

            return RedirectToAction("Detalles", new { id = nuevoCliente.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var (exito, mensaje, cliente) = await genericoService.ObtenerPorId(id);

            if (!exito)
            {
                return NotFound();
            }

            return View("Editar", cliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View("Editar", cliente);
            }

            var (exito, mensaje) = await genericoService.Actualizar(cliente);

            if (!exito)
            {
                ModelState.AddModelError("", mensaje);

                return View("Editar", cliente);
            }

            return RedirectToAction("Detalles", new { id = cliente.Id });
        }
    }
}
