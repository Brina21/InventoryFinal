using InventoryFinal.Models;
using InventoryFinal.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InventoryFinal.Areas.Empleado.Controllers
{
    [Area("Empleado")]
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

                return View("Index", new List<Producto>());
            }

            return View("Index", productos);
        }

        [HttpGet]
        public async Task<IActionResult> Detalles(int id)
        {
            var (exito, mensaje, producto) = await productoService.ObtenerProductoPorId(id);

            if (!exito)
            {
                return NotFound();
            }

            return View("Detalles", producto);
        }

        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            // Obtener todas las Categorias
            var (exitoC, mensajeC, categorias) = await categoriaService.ObtenerTodos();
            if (!exitoC)
            {
                TempData["Error"] = mensajeC;
                return View("Crear");
            }
            // SelectList de Categorias para el dropdown (valor = Id, texto = Nombre)
            ViewBag.Categorias = new SelectList(categorias, "Id", "Nombre");

            // Obtener todos los Proveedores
            var (exitoP, mensajeP, proveedores) = await proveedorService.ObtenerTodos();
            if (!exitoP)
            {
                TempData["Error"] = mensajeP;
                return View("Crearl");
            }
            // SelectList de Proveedores para el dropdown (valor = Id, texto = Nombre)
            ViewBag.Proveedores = new SelectList(proveedores, "Id", "Nombre");


            return View("Crear");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Producto producto)
        {
            if (!ModelState.IsValid)
            {
                return View("Crear", producto);
            }

            var (exito, mensaje, nuevoProducto) = await genericoService.Crear(producto);

            if (!exito)
            {
                ModelState.AddModelError("", mensaje);

                return View("Crear", producto);
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
                return View("Editar");
            }

            // SelectList de Categorias para el dropdown (valor = Id, texto = Nombre)
            ViewBag.Categorias = new SelectList(categorias, "Id", "Nombre");

            // Obtener todos los Proveedores
            var (exitoP, mensajeP, proveedores) = await proveedorService.ObtenerTodos();
            if (!exitoP)
            {
                TempData["Error"] = mensajeP;
                return View("Editar");
            }

            // SelectList de Proveedores para el dropdown (valor = Id, texto = Nombre)
            ViewBag.Proveedores = new SelectList(proveedores, "Id", "Nombre");


            var (exito, mensaje, producto) = await productoService.ObtenerProductoPorId(id);

            if (!exito)
            {
                return NotFound();
            }

            return View("Editar", producto);
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
                return View("Editar", producto);
            }

            var (exito, mensaje) = await genericoService.Actualizar(producto);

            if (!exito)
            {
                ModelState.AddModelError("", mensaje);

                return View("Editar", producto);
            }

            return RedirectToAction("Detalles", new { id = producto.Id });
        }
    }
}
