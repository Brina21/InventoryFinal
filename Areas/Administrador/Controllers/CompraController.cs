using InventoryFinal.DTO;
using InventoryFinal.Models;
using InventoryFinal.Service;
using Microsoft.AspNetCore.Mvc;

namespace InventoryFinal.Controllers
{
    [Area("Administrador")]
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

            return View("Index", compras);
        }

        [HttpGet]
        public async Task<IActionResult> Detalles(int id)
        {
            var compra = await compraService.ObtenerCompraConDetallesPorId(id);

            if (compra == null)
            {
                return NotFound();
            }

            return View("Detalles", compra);
        }

        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            var (exitoProductos, mensajeProductos, productos) = await productoService.ObtenerTodosProductos();
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

            ViewBag.Usuario = HttpContext.Session.GetString("NombreUsuario");

            return View("Crear", new CompraConDetallesDTO());
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

                return View("Crear", compraDto);
            }

            var usuario = HttpContext.Session.GetString("NombreUsuario");
            compraDto.NombreUsuario = usuario;

            if (compraDto.DetalleCompras != null && compraDto.DetalleCompras.Count > 0)
            {
                foreach (var detalle in compraDto.DetalleCompras)
                {
                    // Calculamos el subtotal de cada producto
                    detalle.SubTotal = detalle.Unidades * detalle.PrecioUnitario;
                }

                // Establecemos el total de la compra
                compraDto.Total = compraDto.DetalleCompras.Sum(d => d.SubTotal);
            }

            var (exito, mensaje, nuevaCompra) = await compraService.CrearCompraDTO(compraDto);

            if (!exito)
            {
                var (exitoProductos, _, productos) = await productoService.ObtenerTodosProductos();
                var (exitoClientes, mensajeClientes, clientes) = await clienteService.ObtenerTodos();

                ViewBag.Productos = productos ?? new List<Producto>();
                ViewBag.Clientes = clientes ?? new List<Cliente>();

                ModelState.AddModelError("", mensaje);
                return View("Crear", compraDto);
            }

            return View("Detalles", compraDto);
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

            return View("Editar", compra);
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

                return View("Editar", dto);
            }

            dto.NombreUsuario = HttpContext.Session.GetString("NombreUsuario");

            if (dto.DetalleCompras != null && dto.DetalleCompras.Count > 0)
            {
                foreach (var detalle in dto.DetalleCompras)
                {
                    // Calculamos el subtotal de cada producto
                    detalle.SubTotal = detalle.Unidades * detalle.PrecioUnitario;
                }

                // Establecemos el total de la compra
                dto.Total = dto.DetalleCompras.Sum(d => d.SubTotal);
            }

            await compraService.ActualizarCompra(dto);
            return RedirectToAction("Detalles", new { id = dto.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            var compra = await compraService.ObtenerCompraConDetallesPorId(id);

            if (compra == null)
            {
                return NotFound();
            }

            return View("Eliminar", compra);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            await compraService.EliminarCompra(id);

            return RedirectToAction("Index");
        }
    }
}