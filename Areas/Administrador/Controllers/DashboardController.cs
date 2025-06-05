using InventoryFinal.Data;
using InventoryFinal.Models;
using InventoryFinal.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryFinal.Areas.Administrador.Controllers
{
    [Area("Administrador")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly GenericoService<Producto> productoService;
        private readonly GenericoService<Categoria> categoriaService;
        private readonly GenericoService<Cliente> clienteService;
        private readonly GenericoService<MovimientoStock> movimientoService;

        public DashboardController(ApplicationDbContext _context, GenericoService<Producto> productoService, GenericoService<Categoria> categoriaService, GenericoService<Cliente> clienteService, GenericoService<MovimientoStock> movimientoService)
        {
            this._context = _context;
            this.productoService = productoService;
            this.categoriaService = categoriaService;
            this.clienteService = clienteService;
            this.movimientoService = movimientoService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.NombreUsuario = HttpContext.Session.GetString("NombreUsuario");

            var productos = await productoService.ObtenerTodos();
            var categorias = await categoriaService.ObtenerTodos();
            var clientes = await clienteService.ObtenerTodos();

            ViewBag.TotalProductos = productos.entidades.Count;
            ViewBag.TotalCategorias = categorias.entidades.Count;
            ViewBag.TotalClientes = clientes.entidades.Count;

            // Obtener todos los movimientos ordenados por fecha descendente
            var movimientos = await _context.MovimientoStocks
                   .Include(m => m.Usuario)
                   .Include(m => m.Producto)
                   .OrderByDescending(m => m.FechaMovimiento)
                   .ToListAsync();

            ViewBag.MovimientosStock = movimientos;

            return View(); //Areas/Administrador/Views/Dashboard/Index.cshtml
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}
