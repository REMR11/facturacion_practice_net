using Fatura.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Fatura.Controllers
{
    /// <summary>
    /// Acceso exclusivo para el administrador (dueño). Los clientes no pueden ver Factura ni Inventario.
    /// </summary>
    public class AccesoAdminController : Controller
    {
        private readonly IConfiguration _config;
        private const string SessionKey = "EsAdmin";

        public AccesoAdminController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public IActionResult Index(string returnUrl = null)
        {
            if (EsAdmin())
            {
                return Redirect(returnUrl ?? "/Dashboard");
            }
            ViewBag.ReturnUrl = returnUrl ?? "/Dashboard";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(string usuario, string clave, string returnUrl = null)
        {
            var usuarioCorrecto = _config["AdminUsuario"] ?? "admin";
            var claveCorrecta = _config["AdminClave"] ?? "admin";
            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(clave) ||
                usuario.Trim() != usuarioCorrecto.Trim() || clave != claveCorrecta)
            {
                ViewBag.Error = "Usuario o contraseña incorrectos.";
                ViewBag.ReturnUrl = returnUrl ?? "/Dashboard";
                return View();
            }
            HttpContext.Session.SetString(SessionKey, "1");
            return Redirect(returnUrl ?? "/Dashboard");
        }

        [HttpGet]
        public IActionResult Salir()
        {
            HttpContext.Session.Remove(SessionKey);
            return RedirectToAction("Index", "Home");
        }

        private bool EsAdmin()
        {
            return HttpContext.Session.GetString(SessionKey) == "1";
        }
    }
}
