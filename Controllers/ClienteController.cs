using InventoryFinal.Models;
using InventoryFinal.Service;
using Microsoft.AspNetCore.Mvc;

namespace InventoryFinal.Controllers
{
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

                return View("Views/Administrador/Cliente/Index.cshtml", new List<Cliente>());
            }

            return View("Views/Administrador/Cliente/Index.cshtml", clientes);
        }

        [HttpGet]
        public async Task<IActionResult> Detalles(int id)
        {
            var (exito, mensaje, clientes) = await genericoService.ObtenerPorId(id);

            if (!exito)
            {
                return NotFound();
            }

            return View("Views/Administrador/Cliente/Detalles.cshtml", clientes);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View("Views/Administrador/Cliente/Crear.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Cliente cliente)
        {
            if (!ModelState.IsValid)
            {
                return View("Views/Administrador/Cliente/Crear.cshtml", cliente);
            }

            var (exito, mensaje, nuevoCliente) = await genericoService.Crear(cliente);

            if (!exito)
            {
                ModelState.AddModelError("", mensaje);

                return View("Views/Administrador/Cliente/Crear.cshtml", cliente);
            }

            return RedirectToAction("Views/Administrador/Cliente/Detalles.cshtml", new { id = nuevoCliente.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var (exito, mensaje, cliente) = await genericoService.ObtenerPorId(id);

            if (!exito)
            {
                return NotFound();
            }

            return View("Views/Administrador/Cliente/Editar.cshtml", cliente);
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
                return View("Views/Administrador/Cliente/Editar.cshtml", cliente);
            }

            var (exito, mensaje) = await genericoService.Actualizar(cliente);

            if (!exito)
            {
                ModelState.AddModelError("", mensaje);

                return View("Views/Administrador/Cliente/Editar.cshtml", cliente);
            }

            return RedirectToAction("Views/Administrador/Cliente/Detalles.cshtml", new { id = cliente.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            var (exito, mensaje, cliente) = await genericoService.ObtenerPorId(id);

            if (!exito)
            {
                return NotFound();
            }

            return View("Views/Administrador/Cliente/Eliminar.cshtml", cliente);
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

            return RedirectToAction("Views/Administrador/Cliente/Index.cshtml");
        }
    }
}
