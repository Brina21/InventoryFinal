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

                return View("Views/Administrador/Producto/Index.cshtml", new List<Producto>());
            }

            return View("Views/Administrador/Producto/Index.cshtml", productos);
        }

        [HttpGet]
        public async Task<IActionResult> Detalles(int id)
        {
            var (exito, mensaje, producto) = await productoService.ObtenerProductoPorId(id);

            if (!exito)
            {
                return NotFound();
            }

            return View("Views/Administrador/Producto/Detalles.cshtml", producto);
        }

        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            // Obtener todas las Categorias
            var (exitoC, mensajeC, categorias) = await categoriaService.ObtenerTodos();
            if (!exitoC)
            {
                TempData["Error"] = mensajeC;
                return View("Views/Administrador/Producto/Crear.cshtml");
            }
            // SelectList de Categorias para el dropdown (valor = Id, texto = Nombre)
            ViewBag.Categorias = new SelectList(categorias, "Id", "Nombre");

            // Obtener todos los Proveedores
            var (exitoP, mensajeP, proveedores) = await proveedorService.ObtenerTodos();
            if (!exitoP)
            {
                TempData["Error"] = mensajeP;
                return View("Views/Administrador/Producto/Crear.cshtml");
            }
            // SelectList de Proveedores para el dropdown (valor = Id, texto = Nombre)
            ViewBag.Proveedores = new SelectList(proveedores, "Id", "Nombre");


            return View("Views/Administrador/Producto/Crear.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Producto producto)
        {
            if (!ModelState.IsValid)
            {
                return View("Views/Administrador/Producto/Crear.cshtml", producto);
            }

            var (exito, mensaje, nuevoProducto) = await genericoService.Crear(producto);

            if (!exito)
            {
                ModelState.AddModelError("", mensaje);

                return View("Views/Administrador/Producto/Crear.cshtml", producto);
            }

            return RedirectToAction("Views/Administrador/Producto/Detalles.cshtml", new { id = nuevoProducto.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            // Obtener todas las Categorias
            var (exitoC, mensajeC, categorias) = await categoriaService.ObtenerTodos();
            if (!exitoC)
            {
                TempData["Error"] = mensajeC;
                return View("Views/Administrador/Producto/Editar.cshtml");
            }

            // SelectList de Categorias para el dropdown (valor = Id, texto = Nombre)
            ViewBag.Categorias = new SelectList(categorias, "Id", "Nombre");

            // Obtener todos los Proveedores
            var (exitoP, mensajeP, proveedores) = await proveedorService.ObtenerTodos();
            if (!exitoP)
            {
                TempData["Error"] = mensajeP;
                return View("Views/Administrador/Producto/Editar.cshtml");
            }

            // SelectList de Proveedores para el dropdown (valor = Id, texto = Nombre)
            ViewBag.Proveedores = new SelectList(proveedores, "Id", "Nombre");


            var (exito, mensaje, producto) = await productoService.ObtenerProductoPorId(id);

            if (!exito)
            {
                return NotFound();
            }

            return View("Views/Administrador/Producto/Editar.cshtml", producto);
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
                return View("Views/Administrador/Producto/Editar.cshtml", producto);
            }

            var (exito, mensaje) = await genericoService.Actualizar(producto);

            if (!exito)
            {
                ModelState.AddModelError("", mensaje);

                return View("Views/Administrador/Producto/Editar.cshtml", producto);
            }

            return RedirectToAction("Views/Administrador/Producto/Detalles.cshtml", new { id = producto.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            var (exito, mensaje, producto) = await productoService.ObtenerProductoPorId(id);

            if (!exito)
            {
                return NotFound();
            }

            return View("Views/Administrador/Producto/Eliminar.cshtml", producto);
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

            return RedirectToAction("Views/Administrador/Producto/Index.cshtml");
        }
    }
}
