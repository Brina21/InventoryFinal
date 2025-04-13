using InventoryFinal.Models;
using InventoryFinal.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InventoryFinal.Controllers
{
    public class VentaController : Controller
    {
        private readonly GenericoService<Venta> genericoService;
        private readonly VentaService ventaService;
        private readonly GenericoService<Cliente> clienteService;
        private readonly GenericoService<Usuario> usuarioService;
        private readonly GenericoService<Producto> productoService;

        public VentaController(GenericoService<Venta> genericoService, VentaService ventaService, GenericoService<Cliente> clienteService, GenericoService<Usuario> usuarioService, GenericoService<Producto> productoService)
        {
            this.genericoService = genericoService;
            this.ventaService = ventaService;
            this.clienteService = clienteService;
            this.usuarioService = usuarioService;
            this.productoService = productoService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var (exito, mensaje, ventas) = await ventaService.ObtenerTodasVentas();

            if (!exito)
            {
                TempData["Error"] = mensaje;

                return View(new List<Venta>());
            }

            return View(ventas);
        }

        [HttpGet]
        public async Task<IActionResult> Detalles(int id)
        {
            var (exito, mensaje, ventas) = await ventaService.ObtenerVentaPorId(id);

            if (!exito)
            {
                return NotFound();
            }

            return View(ventas);
        }

        [HttpGet]
        public async Task<IActionResult> CrearAsync()
        {
            // Obtener todos los Usuario
            var (exitoU, mensajeU, usuarios) = await usuarioService.ObtenerTodos();
            if (!exitoU)
            {
                TempData["Error"] = mensajeU;
                return View();
            }
            // Dropdown usuarios (valor = id, texto = nombre)
            ViewBag.Usuarios = new SelectList(usuarios, "Id", "Nombre");

            // Obtener todos los Clientes
            var (exitoC, mensajeC, clientes) = await clienteService.ObtenerTodos();
            if (!exitoC)
            {
                TempData["Error"] = mensajeC;
                return View();
            }
            // Dropdown clientes (valor = id, texto = nombre)
            ViewBag.Clientes = new SelectList(clientes, "Id", "Nombre");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Venta venta)
        {
            if (!ModelState.IsValid)
            {
                return View(venta);
            }

            var (exito, mensaje, nuevaVenta) = await genericoService.Crear(venta);

            if (!exito)
            {
                ModelState.AddModelError("", mensaje);

                return View(venta);
            }

            return RedirectToAction("Detalles", new { id = nuevaVenta.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            // Obtener todos los Usuario
            var (exitoU, mensajeU, usuarios) = await usuarioService.ObtenerTodos();
            if (!exitoU)
            {
                TempData["Error"] = mensajeU;
                return View();
            }
            // Dropdown usuarios (valor = id, texto = nombre)
            ViewBag.Usuarios = new SelectList(usuarios, "Id", "Nombre");

            // Obtener todos los Clientes
            var (exitoC, mensajeC, clientes) = await clienteService.ObtenerTodos();
            if (!exitoC)
            {
                TempData["Error"] = mensajeC;
                return View();
            }
            // Dropdown clientes (valor = id, texto = nombre)
            ViewBag.Clientes = new SelectList(clientes, "Id", "Nombre");


            var (exito, mensaje, venta) = await ventaService.ObtenerVentaPorId(id);

            if (!exito)
            {
                return NotFound();
            }

            return View(venta);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, Venta venta)
        {
            if (id != venta.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(venta);
            }

            var (exito, mensaje) = await genericoService.Actualizar(venta);

            if (!exito)
            {
                ModelState.AddModelError("", mensaje);

                return View(venta);
            }

            return RedirectToAction("Detalles", new { id = venta.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            var (exito, mensaje, venta) = await ventaService.ObtenerVentaPorId(id);

            if (!exito)
            {
                return NotFound();
            }

            return View(venta);
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
