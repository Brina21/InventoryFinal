using InventoryFinal.DTO;
using InventoryFinal.Models;
using InventoryFinal.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InventoryFinal.Controllers
{
    [Area("Administrador")]
    public class VentaController : Controller
    {
        private readonly VentaService ventaService;
        private readonly GenericoService<Cliente> clienteService;
        private readonly ProductoService productoService;

        public VentaController(VentaService ventaService, GenericoService<Cliente> clienteService, ProductoService productoService)
        {
            this.ventaService = ventaService;
            this.clienteService = clienteService;
            this.productoService = productoService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var ventas = await ventaService.ObtenerTodasVentas();

            if (ventas == null)
            {
                ventas = new List<VentaConDetallesDTO>();
            }

            return View("Index", ventas);
        }

        [HttpGet]
        public async Task<IActionResult> Detalles(int id)
        {
            var venta = await ventaService.ObtenerVentaPorId(id);

            if (venta == null)
            {
                return NotFound();
            }

            return View("Detalles", venta);
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
                ModelState.AddModelError("", $"Error al cargar productos: {mensajeProductos}");
                productos = new List<Producto>();
            }

            ViewBag.Productos = productos;
            ViewBag.Clientes = clientes;

            ViewBag.Usuario = HttpContext.Session.GetString("NombreUsuario");

            return View("Crear", new VentaConDetallesDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(VentaConDetallesDTO ventaDto)
        {
            if (!ModelState.IsValid)
            {
                var (exitoProductos, mensajeProductos, productos) = await productoService.ObtenerTodosProductos();
                var (exitoClientes, mensajeClientes, clientes) = await clienteService.ObtenerTodos();

                ViewBag.Productos = productos;
                ViewBag.Clientes = clientes;
                return View("Crear", ventaDto);
            }

            var usuario = HttpContext.Session.GetString("NombreUsuario");
            ventaDto.NombreUsuario = usuario;

            if (ventaDto.DetalleVentas != null && ventaDto.DetalleVentas.Count > 0)
            {
                foreach (var detalle in ventaDto.DetalleVentas)
                {
                    detalle.SubTotal = detalle.Unidades * detalle.PrecioUnitario;
                }

                ventaDto.Total = ventaDto.DetalleVentas.Sum(d => d.SubTotal);
            }

            var (exito, mensaje, nuevaVenta) = await ventaService.CrearVentaDTO(ventaDto);

            if (!exito)
            {
                var (exitoProductos, mensajeProductos, productos) = await productoService.ObtenerTodosProductos();
                var (exitoClientes, mensajeClientes, clientes) = await clienteService.ObtenerTodos();

                ViewBag.Productos = productos;
                ViewBag.Clientes = clientes;

                ModelState.AddModelError("", mensaje);
                return View("Crear", ventaDto);
            }

            return View("Detalles", ventaDto);
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var venta = await ventaService.ObtenerVentaPorId(id);

            if (venta == null)
            {
                return NotFound();
            }

            var (exitoProductos, mensajeProductos, productos) = await productoService.ObtenerTodosProductos();
            var (exitoClientes, mensajeClientes, clientes) = await clienteService.ObtenerTodos();

            if (!exitoClientes || clientes == null)
            {
                ModelState.AddModelError("", $"Error al cargar clientes: {mensajeClientes}");
                clientes = new List<Cliente>();
            }

            if (!exitoProductos || productos == null)
            {
                ModelState.AddModelError("", $"Error al cargar productos: {mensajeProductos}");
                productos = new List<Producto>();
            }

            ViewBag.Productos = productos;
            ViewBag.Clientes = clientes;

            return View("Editar", venta);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, VentaConDetallesDTO venta)
        {
            if (id != venta.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                var (exitoProductos, mensajeProductos, productos) = await productoService.ObtenerTodosProductos();
                var (exitoClientes, mensajeClientes, clientes) = await clienteService.ObtenerTodos();
                
                ViewBag.Productos = productos;
                ViewBag.Clientes = clientes;
             
                return View("Editar", venta);
            }

            var usuario = HttpContext.Session.GetString("NombreUsuario");
            venta.NombreUsuario = usuario;

            if(venta.DetalleVentas != null && venta.DetalleVentas.Count > 0)
            {
                foreach (var detalle in venta.DetalleVentas)
                {
                    detalle.SubTotal = detalle.Unidades * detalle.PrecioUnitario;
                }
                venta.Total = venta.DetalleVentas.Sum(d => d.SubTotal);
            }

            await ventaService.ActualizarVenta(venta);
            return RedirectToAction("Detalles", new { id = venta.Id });
        }
    }
}
