using InventoryFinal.Data;
using InventoryFinal.Models;
using InventoryFinal.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryFinal.Areas.Empleado.Controllers
{
    [Area("Empleado")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly GenericoService<Producto> productoService;
        private readonly GenericoService<Categoria> categoriaService;
        private readonly GenericoService<Cliente> clienteService;

        public DashboardController(ApplicationDbContext _context, GenericoService<Producto> productoService, GenericoService<Categoria> categoriaService, GenericoService<Cliente> clienteService)
        {
            this._context = _context;
            this.productoService = productoService;
            this.categoriaService = categoriaService;
            this.clienteService = clienteService;
        }

        public async Task<IActionResult> Index()
        {
            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "Empleado")
            {
                return RedirectToAction("AccesoDenegado", "Home", new { area = "" });
            }

            ViewBag.NombreUsuario = HttpContext.Session.GetString("NombreUsuario");

            var productos = await productoService.ObtenerTodos();
            var categorias = await categoriaService.ObtenerTodos();
            var clientes = await clienteService.ObtenerTodos();

            ViewBag.TotalProductos = productos.entidades.Count;
            ViewBag.TotalCategorias = categorias.entidades.Count;
            ViewBag.TotalClientes = clientes.entidades.Count;

            return View(); //Areas/Administrador/Views/Dashboard/Index.cshtml
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}