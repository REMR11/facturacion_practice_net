using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Fatura.Infrastructure
{
    /// <summary>
    /// Restringe el acceso a la acción o controlador solo para el administrador (sesión activa).
    /// Si no hay sesión de admin, redirige a la página de acceso.
    /// </summary>
    public class SoloAdminAttribute : TypeFilterAttribute
    {
        public SoloAdminAttribute() : base(typeof(SoloAdminFilter))
        {
        }
    }

    public class SoloAdminFilter : IActionFilter
    {
        private const string SessionKey = "EsAdmin";

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var esAdmin = context.HttpContext.Session.GetString(SessionKey) == "1";
            context.HttpContext.Items["EsAdmin"] = esAdmin;
            if (context.Controller is Controller c)
                c.ViewBag.EsAdmin = esAdmin;

            if (!esAdmin)
            {
                if (context.HttpContext.Request.Path.StartsWithSegments("/AccesoAdmin"))
                    return;
                var path = context.HttpContext.Request.Path.Value ?? "";
                var query = context.HttpContext.Request.QueryString.Value ?? "";
                var returnUrl = path + query;
                var urlAcceso = string.IsNullOrEmpty(returnUrl) ? "/AccesoAdmin" : "/AccesoAdmin?returnUrl=" + Uri.EscapeDataString(returnUrl);
                context.Result = new RedirectResult(urlAcceso);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
