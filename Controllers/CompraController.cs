using InventoryFinal.DTO;
using InventoryFinal.Models;
using InventoryFinal.Service;
using Microsoft.AspNetCore.Mvc;

namespace InventoryFinal.Controllers
{
    public class CompraController : Controller
    {
        private readonly CompraService compraService;
        private readonly ProductoService productoService;
        private readonly GenericoService<Cliente> clienteService;

        public CompraController(CompraService compraService, ProductoService productoService, GenericoService<Cliente> clienteService)
        {
            this.compraService = compraService;
            this.productoService = productoService;
            this.clienteService = clienteService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var compras = await compraService.ObtenerTodasLasComprasDTO();

            if (compras == null)
            {
                compras = new List<CompraConDetallesDTO>();
            }

            return View("Views/Administrador/Compra/Index.cshtml", compras);
        }

        [HttpGet]
        public async Task<IActionResult> Detalles(int id)
        {
            var compra = await compraService.ObtenerCompraConDetallesPorId(id);

            if (compra == null)
            {
                return NotFound();
            }

            return View("Views/Administrador/Compra/Detalles.cshtml", compra);
        }

        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            var (exitoProductos, _, productos) = await productoService.ObtenerTodosProductos();
            var (exitoClientes, mensajeClientes, clientes) = await clienteService.ObtenerTodos();

            if (!exitoClientes || clientes == null)
            {
                ModelState.AddModelError("", $"Error al cargar clientes: {mensajeClientes}");
                clientes = new List<Cliente>();
            }

            if (!exitoProductos || productos == null)
            {
                ModelState.AddModelError("", "Error al cargar productos");
                productos = new List<Producto>();
            }

            ViewBag.Productos = productos;
            ViewBag.Clientes = clientes;

            return View("Views/Administrador/Compra/Crear.cshtml", new CompraConDetallesDTO());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(CompraConDetallesDTO compraDto)
        {
            if (!ModelState.IsValid)
            {
                var (exitoProductos, _, productos) = await productoService.ObtenerTodosProductos();
                var (exitoClientes, mensajeClientes, clientes) = await clienteService.ObtenerTodos();
                ViewBag.Productos = productos ?? new List<Producto>();
                ViewBag.Clientes = clientes ?? new List<Cliente>();

                return View("Views/Administrador/Compra/Crear.cshtml", compraDto);
            }

            var (exito, mensaje, nuevaCompra) = await compraService.CrearCompraDTO(compraDto);

            if (!exito)
            {
                var (exitoProductos, _, productos) = await productoService.ObtenerTodosProductos();
                var (exitoClientes, mensajeClientes, clientes) = await clienteService.ObtenerTodos();

                ViewBag.Productos = productos ?? new List<Producto>();
                ViewBag.Clientes = clientes ?? new List<Cliente>();

                ModelState.AddModelError("", mensaje);
                return View("Views/Administrador/Compra/Crear.cshtml", compraDto);
            }

            return RedirectToAction("Views/Administrador/Compra/Detalles.cshtml", new { id = nuevaCompra.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var compra = await compraService.ObtenerCompraConDetallesPorId(id);

            if (compra == null)
            {
                return NotFound();
            }

            var (exitoProductos, _, productos) = await productoService.ObtenerTodosProductos();
            var (exitoClientes, mensajeClientes, clientes) = await clienteService.ObtenerTodos();

            if (!exitoClientes || clientes == null)
            {
                ModelState.AddModelError("", $"Error al cargar clientes: {mensajeClientes}");
                clientes = new List<Cliente>();
            }

            if (!exitoProductos || productos == null)
            {
                ModelState.AddModelError("", "Error al cargar productos");
                productos = new List<Producto>();
            }

            ViewBag.Productos = productos;
            ViewBag.Clientes = clientes;

            return View("Views/Administrador/Compra/Editar.cshtml", compra);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, CompraConDetallesDTO dto)
        {
            if (id != dto.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                var (exitoProductos, _, productos) = await productoService.ObtenerTodosProductos();
                var (exitoClientes, mensajeClientes, clientes) = await clienteService.ObtenerTodos();
                ViewBag.Productos = productos ?? new List<Producto>();
                ViewBag.Clientes = clientes ?? new List<Cliente>();

                return View("Views/Administrador/Compra/Editar.cshtml", dto);
            }

            await compraService.ActualizarCompra(dto);
            return RedirectToAction("Views/Administrador/Compra/Detalles.cshtml", new { id = dto.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            var compra = await compraService.ObtenerCompraConDetallesPorId(id);

            if (compra == null)
            {
                return NotFound();
            }

            return View("Views/Administrador/Compra/Eliminar.cshtml", compra);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            await compraService.EliminarCompra(id);

            return RedirectToAction("Views/Administrador/Compra/Index.cshtml");
        }
    }
}