using Fatura.Models.Facturacion;
using Fatura.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Fatura.Controllers
{
    /// <summary>
    /// Controlador para gestionar clientes.
    /// Proporciona funcionalidad para listar, buscar, crear, editar y eliminar clientes.
    /// </summary>
    public class ClientesController : Controller
    {
        private readonly IClienteService _clienteService;

        public ClientesController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        /// <summary>
        /// Lista todos los clientes (vista principal).
        /// </summary>
        public async Task<IActionResult> Index(string? searchTerm = null)
        {
            IEnumerable<Cliente> clientes;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                clientes = await _clienteService.SearchAsync(searchTerm);
            }
            else
            {
                clientes = await _clienteService.GetClientesActivosAsync();
            }

            ViewBag.SearchTerm = searchTerm;
            return View(clientes);
        }

        /// <summary>
        /// Endpoint API para búsqueda de clientes (para AJAX).
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Search(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                var todos = await _clienteService.GetAllAsync();
                return Json(todos);
            }

            var resultados = await _clienteService.SearchAsync(term);
            return Json(resultados);
        }

        /// <summary>
        /// Obtiene los detalles de un cliente.
        /// </summary>
        [HttpGet]
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

        /// <summary>
        /// Muestra el formulario para crear un nuevo cliente.
        /// </summary>
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Crea un nuevo cliente.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cliente cliente)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(cliente);
                }

                // Validaciones adicionales
                if (string.IsNullOrWhiteSpace(cliente.Nombre))
                {
                    ModelState.AddModelError("Nombre", "El nombre del cliente es requerido.");
                    return View(cliente);
                }

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

        /// <summary>
        /// Muestra el formulario para editar un cliente.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var cliente = await _clienteService.GetByIdAsync(id);
                if (cliente == null)
                {
                    TempData["Error"] = "El cliente no fue encontrado.";
                    return RedirectToAction(nameof(Index));
                }
                return View(cliente);
            }
            catch (Fatura.Exceptions.EntityNotFoundException)
            {
                TempData["Error"] = "El cliente no fue encontrado.";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Actualiza un cliente existente.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cliente cliente)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _clienteService.UpdateAsync(id, cliente);
                    TempData["Success"] = "Cliente actualizado exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
                return View(cliente);
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

        /// <summary>
        /// Muestra la confirmación para eliminar un cliente.
        /// Redirige al Index ya que la eliminación se hace directamente desde allí.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var cliente = await _clienteService.GetByIdAsync(id);
                if (cliente == null)
                {
                    TempData["Error"] = "El cliente no fue encontrado.";
                    return RedirectToAction(nameof(Index));
                }
                // Redirigir al Index ya que la eliminación se confirma con JavaScript
                return RedirectToAction(nameof(Index));
            }
            catch (Fatura.Exceptions.EntityNotFoundException)
            {
                TempData["Error"] = "El cliente no fue encontrado.";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Elimina un cliente.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, IFormCollection collection)
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

        /// <summary>
        /// Endpoint API para obtener estadísticas de un cliente.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetEstadisticas(int id)
        {
            try
            {
                var totalFacturas = await _clienteService.GetTotalFacturasAsync(id);
                var totalFacturado = await _clienteService.GetTotalFacturadoAsync(id);

                return Json(new
                {
                    totalFacturas,
                    totalFacturado
                });
            }
            catch (Fatura.Exceptions.EntityNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
