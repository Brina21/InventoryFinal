using InventoryFinal.Models;
using InventoryFinal.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace InventoryFinal.Controllers
{
    public class ProductoController : Controller
    {
        private readonly GenericoService<Producto> genericoService;
        private readonly GenericoService<Categoria> categoriaService;
        private readonly GenericoService<Proveedor> proveedorService;
        private readonly ProductoService productoService;

        public ProductoController(GenericoService<Producto> genericoService, ProductoService productoService, GenericoService<Categoria> categoriaService, GenericoService<Proveedor> proveedorService)
        {
            this.genericoService = genericoService;
            this.productoService = productoService;
            this.categoriaService = categoriaService;
            this.proveedorService = proveedorService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var (exito, mensaje, productos) = await productoService.ObtenerTodosProductos();

            if (!exito)
            {
                TempData["Error"] = mensaje;

                return View(new List<Producto>());
            }

            return View(productos);
        }

        [HttpGet]
        public async Task<IActionResult> Detalles(int id)
        {
            var (exito, mensaje, producto) = await productoService.ObtenerProductoPorId(id);

            if (!exito)
            {
                return NotFound();
            }

            return View(producto);
        }

        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            // Obtener todas las Categorias
            var (exitoC, mensajeC, categorias) = await categoriaService.ObtenerTodos();
            if (!exitoC)
            {
                TempData["Error"] = mensajeC;
                return View();
            }
            // SelectList de Categorias para el dropdown (valor = Id, texto = Nombre)
            ViewBag.Categorias = new SelectList(categorias, "Id", "Nombre");

            // Obtener todos los Proveedores
            var (exitoP, mensajeP, proveedores) = await proveedorService.ObtenerTodos();
            if (!exitoP)
            {
                TempData["Error"] = mensajeP;
                return View();
            }
            // SelectList de Proveedores para el dropdown (valor = Id, texto = Nombre)
            ViewBag.Proveedores = new SelectList(proveedores, "Id", "Nombre");


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Producto producto)
        {
            if (!ModelState.IsValid)
            {
                return View(producto);
            }

            var (exito, mensaje, nuevoProducto) = await genericoService.Crear(producto);

            if (!exito)
            {
                ModelState.AddModelError("", mensaje);

                return View(producto);
            }

            return RedirectToAction("Detalles", new { id = nuevoProducto.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            // Obtener todas las Categorias
            var (exitoC, mensajeC, categorias) = await categoriaService.ObtenerTodos();
            if (!exitoC)
            {
                TempData["Error"] = mensajeC;
                return View();
            }

            // SelectList de Categorias para el dropdown (valor = Id, texto = Nombre)
            ViewBag.Categorias = new SelectList(categorias, "Id", "Nombre");

            // Obtener todos los Proveedores
            var (exitoP, mensajeP, proveedores) = await proveedorService.ObtenerTodos();
            if (!exitoP)
            {
                TempData["Error"] = mensajeP;
                return View();
            }

            // SelectList de Proveedores para el dropdown (valor = Id, texto = Nombre)
            ViewBag.Proveedores = new SelectList(proveedores, "Id", "Nombre");


            var (exito, mensaje, producto) = await productoService.ObtenerProductoPorId(id);

            if (!exito)
            {
                return NotFound();
            }

            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, Producto producto)
        {
            if (id != producto.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(producto);
            }

            var (exito, mensaje) = await genericoService.Actualizar(producto);

            if (!exito)
            {
                ModelState.AddModelError("", mensaje);

                return View(producto);
            }

            return RedirectToAction("Detalles", new { id = producto.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            var (exito, mensaje, producto) = await productoService.ObtenerProductoPorId(id);

            if (!exito)
            {
                return NotFound();
            }

            return View(producto);
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
