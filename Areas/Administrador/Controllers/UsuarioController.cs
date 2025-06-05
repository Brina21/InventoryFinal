using InventoryFinal.Models;
using InventoryFinal.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InventoryFinal.Controllers
{
    [Area("Administrador")]
    public class UsuarioController : Controller
    {
        private readonly GenericoService<Usuario> genericoService;

        public UsuarioController(GenericoService<Usuario> genericoService)
        {
            this.genericoService = genericoService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var (exito, mensaje, usuarios) = await genericoService.ObtenerTodos();

            if (!exito)
            {
                TempData["Error"] = mensaje;
                // Muestra lista vacía
                return View("Index", new List<Usuario>());
            }

            return View("Index", usuarios);
        }

        [HttpGet]
        public async Task<IActionResult> Detalles(int id)
        {
            var (exito, mensaje, usuario) = await genericoService.ObtenerPorId(id);

            if (!exito)
            {
                // Si no se encuentra el usuario
                return NotFound();
            }

            return View("Detalles", usuario);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            ViewBag.Cargos = Enum.GetValues(typeof(Cargo))
                                 .Cast<Cargo>()
                                 .Select(c => new SelectListItem
                                 {
                                     Text = c.ToString(),
                                     Value = c.ToString(),
                                     Selected = c == Cargo.Empleado // Por defecto, seleccionar Empleado
                                 }).ToList();

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Cargos = Enum.GetValues(typeof(Cargo))
                                     .Cast<Cargo>()
                                     .Select(c => new SelectListItem
                                     {
                                         Text = c.ToString(),
                                         Value = c.ToString()
                                     }).ToList();

                return View("Crear", usuario);
            }

            usuario.FechaCreacion = DateTime.Now;
            var (exito, mensaje, nuevoUsuario) = await genericoService.Crear(usuario);

            if (!exito)
            {
                ViewBag.Cargos = Enum.GetValues(typeof(Cargo))
                                     .Cast<Cargo>()
                                     .Select(c => new SelectListItem
                                     {
                                         Text = c.ToString(),
                                         Value = c.ToString()
                                     }).ToList();

                ModelState.AddModelError("", mensaje);
                return View("Crear", usuario);
            }

            return RedirectToAction("Detalles", new { id = nuevoUsuario.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var (exito, mensaje, usuario) = await genericoService.ObtenerPorId(id);

            if (!exito)
            {
                return NotFound();
            }

            ViewBag.Cargos = Enum.GetValues(typeof(Cargo))
                                 .Cast<Cargo>()
                                 .Select(c => new SelectListItem
                                 {
                                     Text = c.ToString(),
                                     Value = c.ToString()
                                 }).ToList();

            return View("Editar", usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Cargos = Enum.GetValues(typeof(Cargo))
                                     .Cast<Cargo>()
                                     .Select(c => new SelectListItem
                                     {
                                         Text = c.ToString(),
                                         Value = c.ToString()
                                     }).ToList();

                return View("Editar", usuario);
            }

            var (exito, mensaje) = await genericoService.Actualizar(usuario);

            if (!exito)
            {
                ViewBag.Cargos = Enum.GetValues(typeof(Cargo))
                                     .Cast<Cargo>()
                                     .Select(c => new SelectListItem
                                     {
                                         Text = c.ToString(),
                                         Value = c.ToString()
                                     }).ToList();

                ModelState.AddModelError("", mensaje);
                return View("Editar", usuario);
            }

            return RedirectToAction("Detalles", new { id = usuario.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            var (exito, mensaje, usuario) = await genericoService.ObtenerPorId(id);

            if (!exito)
            {
                return NotFound();
            }

            return View("Eliminar", usuario);
        }

        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            var (exito, mensaje) = await genericoService.Eliminar(id);

            if (!exito)
                TempData["Error"] = mensaje;
            else
                TempData["Exito"] = mensaje;

            return RedirectToAction("Index");
        }


        // GET: Usuario/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
    }
}