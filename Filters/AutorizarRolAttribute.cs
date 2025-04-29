using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace InventoryFinal.Filters
{
    public class AutorizarRolAttribute : ActionFilterAttribute
    {
        private readonly string _rol;
        public AutorizarRolAttribute(string rol)
        {
            _rol = rol;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var usuarioRol = context.HttpContext.Session.GetString("Rol");
            if (usuarioRol != _rol)
            {
                context.Result = new RedirectToActionResult("Index", "Home", null);
            }
        }
    }
}
