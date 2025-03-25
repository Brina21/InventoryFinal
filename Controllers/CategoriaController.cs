using InventoryFinal.Models;
using InventoryFinal.Service;
using Microsoft.AspNetCore.Mvc;

namespace InventoryFinal.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly GenericoService<Categoria> genericoService;

        public CategoriaController(GenericoService<Categoria> genericoService)
        {
            this.genericoService = genericoService;
        }

        // Lista todas las categorías
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var (exito, mensaje, categorias) = await genericoService.ObtenerTodos();

            if (!exito)
            {
                TempData["Error"] = mensaje;
                return View(new List<Categoria>());
            }

            return View(categorias);
        }

        // Obtener Detalle Categoría por id
        [HttpGet]
        public async Task<IActionResult> Detalles(int id)
        {
            var (exito, mensaje, categoria) = await genericoService.ObtenerPorId(id);

            if (!exito)
            {
                return NotFound();
            }
            return View(categoria);
        }

        // Crear Categoría (Abre el formulario para crear)
        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        // Enviar formulario
        [HttpPost]
        [ValidateAntiForgeryToken] // Protege contra ataques CSRF
        public async Task<IActionResult> Crear(Categoria categoria)
        {
            if (!ModelState.IsValid)
            {
                // Recarga la vista mostrando el error
                return View(categoria);
            }
            
            var (exito, mensaje, nuevaCategoria) = await genericoService.Crear(categoria);

            if (!exito)
            {
                ModelState.AddModelError("", mensaje);
                return View(categoria);
            }

            // Se redirige al Index
            return RedirectToAction("Detalles", new { id = nuevaCategoria.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var (exito, mensaje, categoria) = await genericoService.ObtenerPorId(id);

            if (!exito)
            {
                return NotFound();
            }

            return View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar (int id, Categoria categoria)
        {
            if (id != categoria.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(categoria);
            }

            var (exito, mensaje) = await genericoService.Actualizar(categoria);

            if (!exito)
            {
                ModelState.AddModelError("", mensaje);

                return View(categoria);
            }

            return RedirectToAction("Detalles", new { id = categoria.Id});
        }

        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            var (exito, mensaje, categoria) = await genericoService.ObtenerPorId(id);

            if (!exito)
            {
                return NotFound();
            }

            return View(categoria);
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
