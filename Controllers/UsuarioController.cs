using InventoryFinal.Models;
using InventoryFinal.Service;
using Microsoft.AspNetCore.Mvc;

namespace InventoryFinal.Controllers
{
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
                return View("Views/Administrador/Usuario/Index.cshtml", new List<Usuario>());
            }

            return View("Views/Administrador/Usuario/Index.cshtml", usuarios);
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

            return View("Views/Administrador/Usuario/Detalles.cshtml", usuario);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View("Views/Administrador/Usuario/Crear.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return View("Views/Administrador/Usuario/Crear.cshtml", usuario);
            }

            usuario.FechaCreacion = DateTime.Now;
            var (exito, mensaje, nuevoUsuario) = await genericoService.Crear(usuario);

            if (!exito)
            {
                ModelState.AddModelError("", mensaje);
                // Recargar vista con mensaje de error
                return View("Views/Administrador/Usuario/Crear.cshtml", usuario);
            }

            // Redirigir a detalles del nuevo usuario
            return RedirectToAction("Views/Administrador/Usuario/Detalles.cshtml", new { id = nuevoUsuario.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var (exito, mensaje, usuario) = await genericoService.ObtenerPorId(id);

            if (!exito)
            {
                return NotFound();
            }

            return View("Views/Administrador/Usuario/Editar.cshtml", usuario);
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
                return View("Views/Administrador/Usuario/Editar.cshtml", usuario);
            }

            var (exito, mensaje) = await genericoService.Actualizar(usuario);

            if (!exito)
            {
                ModelState.AddModelError("", mensaje);
                // Mantener en la vista de edición
                return View("Views/Administrador/Usuario/Editar.cshtml", usuario);
            }

            return RedirectToAction("Views/Administrador/Usuario/Detalles.cshtml", new { id = usuario.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            var (exito, mensaje, usuario) = await genericoService.ObtenerPorId(id);

            if (!exito)
            {
                return NotFound();
            }

            return View("Views/Administrador/Usuario/Eliminar.cshtml", usuario);
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

            return RedirectToAction("Views/Administrador/Usuario/Index.cshtml");
        }

        
        // GET: Usuario/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        /*
        // POST: Usuario/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string nombre, string contrasenya)
        {
            var (exito, mensaje, usuarios) = await genericoService.ObtenerTodos();

            if (!exito)
            {
                TempData["Error"] = mensaje;
                return View();
            }

            var usuario = usuarios.FirstOrDefault(u => u.Nombre == nombre && u.Contrasenya == contrasenya);

            if (usuario == null)
            {
                ModelState.AddModelError("", "Nombre o contraseña incorrectos");
                return View();
            }

            // Guardar usuario en sesión
            HttpContext.Session.SetInt32("UsuarioId", usuario.Id);
            HttpContext.Session.SetString("UsuarioNombre", usuario.Nombre);
            HttpContext.Session.SetString("UsuarioRol", usuario.Rol.ToString());

            // Redirigir por rol
            if (usuario.Rol == Cargo.Administrador)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // Cerrar sesión
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Limpia toda la sesión
            return RedirectToAction("Login");
        }
        */
    }
}