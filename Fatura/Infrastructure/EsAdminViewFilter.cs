using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Fatura.Infrastructure
{
    /// <summary>
    /// Establece ViewBag.EsAdmin en todas las vistas según la sesión.
    /// </summary>
    public class EsAdminViewFilter : IActionFilter
    {
        private const string SessionKey = "EsAdmin";

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var esAdmin = context.HttpContext.Session.GetString(SessionKey) == "1";
            context.HttpContext.Items["EsAdmin"] = esAdmin;
            if (context.Controller is Controller c)
                c.ViewBag.EsAdmin = esAdmin;
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
