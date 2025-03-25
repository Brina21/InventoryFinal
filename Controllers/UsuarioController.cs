using InventoryFinal.Models;
using InventoryFinal.Service;
using Microsoft.AspNetCore.Mvc;

namespace InventoryFinal.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly GenericoService<Usuario> genericoService;

        public UsuarioController(GenericoService<Usuario> genericoService)
        {
            this.genericoService = genericoService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var (exito, mensaje, usuarios) = await genericoService.ObtenerTodos();

            if (!exito)
            {
                TempData["Error"] = mensaje;
                // Muestra lista vacía
                return View(new List<Usuario>());
            }

            return View(usuarios);
        }

        [HttpGet]
        public async Task<IActionResult> Detalles(int id)
        {
            var (exito, mensaje, usuario) = await genericoService.ObtenerPorId(id);

            if (!exito)
            {
                // Si no se encuentra el usuario
                return NotFound();
            }

            return View(usuario);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return View(usuario);
            }

            usuario.FechaCreacion = DateTime.Now;
            var (exito, mensaje, nuevoUsuario) = await genericoService.Crear(usuario);

            if (!exito)
            {
                ModelState.AddModelError("", mensaje);
                // Recargar vista con mensaje de error
                return View(usuario);
            }

            // Redirigir a detalles del nuevo usuario
            return RedirectToAction("Detalles", new { id = nuevoUsuario.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var (exito, mensaje, usuario) = await genericoService.ObtenerPorId(id);

            if (!exito)
            {
                return NotFound();
            }

            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(usuario);
            }

            var (exito, mensaje) = await genericoService.Actualizar(usuario);

            if (!exito)
            {
                ModelState.AddModelError("", mensaje);
                // Mantener en la vista de edición
                return View(usuario);
            }

            return RedirectToAction("Detalles", new { id = usuario.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            var (exito, mensaje, usuario) = await genericoService.ObtenerPorId(id);

            if (!exito)
            {
                return NotFound();
            }

            return View(usuario);
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