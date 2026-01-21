using Fatura.Models.Facturacion;
using Fatura.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Fatura.Exceptions;

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
            try
            {
                var cliente = await _clienteService.GetByIdAsync(id);

                if (cliente == null)
                {
                    TempData["Error"] = "El cliente no fue encontrado.";
                    return RedirectToAction(nameof(Index));
                }

                // Obtener estadísticas del cliente
                ViewBag.TotalFacturas = await _clienteService.GetTotalFacturasAsync(id);
                ViewBag.TotalFacturado = await _clienteService.GetTotalFacturadoAsync(id);

                return View(cliente);
            }
            catch (Fatura.Exceptions.EntityNotFoundException)
            {
                TempData["Error"] = "El cliente no fue encontrado.";
                return RedirectToAction(nameof(Index));
            }
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
            try
            {
                if (!ModelState.IsValid)
                    return View(cliente);

                await _clienteService.CreateAsync(cliente);
                TempData["Success"] = "Cliente creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Fatura.Exceptions.BusinessRuleException ex)
            {
                ModelState.AddModelError("", ex.Message);
                TempData["Error"] = ex.Message;
                return View(cliente);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al crear el cliente: {ex.Message}");
                TempData["Error"] = $"Error al crear el cliente: {ex.Message}";
                return View(cliente);
            }
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
            try
            {
                if (!ModelState.IsValid)
                    return View(cliente);

                await _clienteService.UpdateAsync(cliente.Id, cliente);
                TempData["Success"] = "Cliente actualizado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Fatura.Exceptions.EntityNotFoundException)
            {
                TempData["Error"] = "El cliente no fue encontrado.";
                return RedirectToAction(nameof(Index));
            }
            catch (Fatura.Exceptions.BusinessRuleException ex)
            {
                ModelState.AddModelError("", ex.Message);
                TempData["Error"] = ex.Message;
                return View(cliente);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al actualizar el cliente: {ex.Message}");
                TempData["Error"] = $"Error al actualizar el cliente: {ex.Message}";
                return View(cliente);
            }
        }

        // ===============================
        // SOFT DELETE
        // ===============================
        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _clienteService.DeleteAsync(id);
                TempData["Success"] = "Cliente eliminado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Fatura.Exceptions.EntityNotFoundException)
            {
                TempData["Error"] = "El cliente no fue encontrado.";
                return RedirectToAction(nameof(Index));
            }
            catch (Fatura.Exceptions.BusinessRuleException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al eliminar el cliente: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
