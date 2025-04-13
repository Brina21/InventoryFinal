using InventoryFinal.Models;
using InventoryFinal.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InventoryFinal.Controllers
{
    public class CompraController : Controller
    {
        private readonly GenericoService<Compra> genericoService;
        private readonly CompraService compraService;
        private readonly GenericoService<Cliente> clienteService;
        private readonly GenericoService<Usuario> usuarioService;

        public CompraController(GenericoService<Compra> genericoService, CompraService compraService, GenericoService<Cliente> clienteService, GenericoService<Usuario> usuarioService)
        {
            this.genericoService = genericoService;
            this.compraService = compraService;
            this.clienteService = clienteService;
            this.usuarioService = usuarioService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var (exito, mensaje, compras) = await compraService.ObtenerTodasCompras();

            if (!exito)
            {
                TempData["Error"] = mensaje;

                return View(new List<Compra>());
            }

            return View(compras);
        }

        [HttpGet]
        public async Task<IActionResult> Detalles(int id)
        {
            var (exito, mensaje, compras) = await compraService.ObtenerCompraPorId(id);

            if (!exito)
            {
                return NotFound();
            }

            return View(compras);
        }

        [HttpGet]
        public async Task<IActionResult> Crear()
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
        public async Task<IActionResult> Crear(Compra compra)
        {
            
            if (!ModelState.IsValid)
            {
                return View(compra);
            }

            var (exito, mensaje, nuevaCompra) = await genericoService.Crear(compra);

            if (!exito)
            {
                ModelState.AddModelError("", mensaje);

                return View(compra);
            }

            return RedirectToAction("Detalles", new { id = nuevaCompra.Id });
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


            var (exito, mensaje, compra) = await compraService.ObtenerCompraPorId(id);

            if (!exito)
            {
                return NotFound();
            }

            return View(compra);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, Compra compra)
        {
            if (id != compra.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(compra);
            }

            var (exito, mensaje) = await genericoService.Actualizar(compra);

            if (!exito)
            {
                ModelState.AddModelError("", mensaje);

                return View(compra);
            }

            return RedirectToAction("Detalles", new { id = compra.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            var (exito, mensaje, compra) = await compraService.ObtenerCompraPorId(id);

            if (!exito)
            {
                return NotFound();
            }

            return View(compra);
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