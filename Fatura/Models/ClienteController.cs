using Fatura.Models.Facturacion;
using Fatura.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Fatura.Controllers
{
    [Route("Clientes")]
    public class ClienteController : Controller
    {
        private readonly IClienteService _clienteService;

        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        // ===============================
        // LISTADO DE CLIENTES
        // ===============================
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var clientes = await _clienteService.GetClientesActivosAsync();
            return View(clientes);
        }

        // ===============================
        // PERFIL / DETALLE DEL CLIENTE
        // ===============================
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var cliente = await _clienteService.GetByIdAsync(id);

            if (cliente == null)
                return NotFound();

            return View(cliente);
        }

        // ===============================
        // GET: CREATE
        // ===============================
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        // ===============================
        // POST: CREATE
        // ===============================
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cliente cliente)
        {
            if (!ModelState.IsValid)
                return View(cliente);

            await _clienteService.CreateAsync(cliente);
            return RedirectToAction(nameof(Index));
        }

        // ===============================
        // GET: EDIT
        // ===============================
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var cliente = await _clienteService.GetByIdAsync(id);

            if (cliente == null)
                return NotFound();

            return View(cliente);
        }

        // ===============================
        // POST: EDIT
        // ===============================
        [HttpPost("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Cliente cliente)
        {
            if (!ModelState.IsValid)
                return View(cliente);

            /*await _clienteService.UpdateAsync(cliente);*/
            return RedirectToAction(nameof(Index));
        }

        // ===============================
        // SOFT DELETE
        // ===============================
        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _clienteService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
