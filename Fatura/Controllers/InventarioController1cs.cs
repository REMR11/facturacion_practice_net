using Fatura.Models.Catalogos;
using Fatura.Models.Enums;
using Fatura.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Fatura.Controllers
{
    /// <summary>
    /// Controlador para la gestión del INVENTARIO.
    /// Permite listar, buscar, filtrar, crear, editar y eliminar productos del inventario.
    /// </summary>
    [Route("Inventario")]
    public class InventarioController : Controller
    {
        private readonly IProductoService _productoService;
        private readonly ICategoriaService _categoriaService;
        private readonly IMarcaService _marcaService;

        public InventarioController(
            IProductoService productoService,
            ICategoriaService categoriaService,
            IMarcaService marcaService)
        {
            _productoService = productoService;
            _categoriaService = categoriaService;
            _marcaService = marcaService;
        }

        // ===============================
        // LISTADO DE INVENTARIO
        // ===============================
        [HttpGet("")]
        public async Task<IActionResult> Index(string? searchTerm = null, TipoProducto? tipo = null)
        {
            IEnumerable<Producto> inventario;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                inventario = await _productoService.SearchAsync(searchTerm);
            }
            else
            {
                inventario = await _productoService.GetAllAsync();
            }

            if (tipo.HasValue)
            {
                inventario = inventario.Where(p => p.Tipo == tipo.Value);
            }

            ViewBag.SearchTerm = searchTerm;
            ViewBag.Tipo = tipo;
            ViewBag.Categorias = await _categoriaService.GetAllAsync();
            ViewBag.Marcas = await _marcaService.GetAllAsync();

            return View(inventario);
        }

        // ===============================
        // API BÚSQUEDA INVENTARIO (AJAX)
        // ===============================
        [HttpGet("api/search")]
        public async Task<IActionResult> Search([FromQuery] string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                var todos = await _productoService.GetAllAsync();
                return Json(todos);
            }

            var resultados = await _productoService.SearchAsync(term);
            return Json(resultados);
        }

        // ===============================
        // DETALLE DE INVENTARIO
        // ===============================
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var producto = await _productoService.GetByIdAsync(id);
                if (producto == null)
                    return NotFound();

                return View(producto);
            }
            catch (Fatura.Exceptions.EntityNotFoundException)
            {
                return NotFound();
            }
        }

        // ===============================
        // CREAR REGISTRO DE INVENTARIO
        // ===============================
        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Marcas = await _marcaService.GetAllAsync();
            ViewBag.Categorias = await _categoriaService.GetAllAsync();
            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Producto producto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _productoService.CreateAsync(producto);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Fatura.Exceptions.BusinessRuleException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            ViewBag.Marcas = await _marcaService.GetAllAsync();
            ViewBag.Categorias = await _categoriaService.GetAllAsync();
            return View(producto);
        }

        // ===============================
        // EDITAR INVENTARIO
        // ===============================
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var producto = await _productoService.GetByIdAsync(id);
                if (producto == null)
                    return NotFound();

                ViewBag.Marcas = await _marcaService.GetAllAsync();
                ViewBag.Categorias = await _categoriaService.GetAllAsync();
                return View(producto);
            }
            catch (Fatura.Exceptions.EntityNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Producto producto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _productoService.UpdateAsync(id, producto);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Fatura.Exceptions.BusinessRuleException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            ViewBag.Marcas = await _marcaService.GetAllAsync();
            ViewBag.Categorias = await _categoriaService.GetAllAsync();
            return View(producto);
        }

        // ===============================
        // ELIMINAR INVENTARIO
        // ===============================
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var producto = await _productoService.GetByIdAsync(id);
                if (producto == null)
                    return NotFound();

                return View(producto);
            }
            catch (Fatura.Exceptions.EntityNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _productoService.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Fatura.Exceptions.BusinessRuleException ex)
            {
                ModelState.AddModelError("", ex.Message);
                var producto = await _productoService.GetByIdAsync(id);
                return View("Delete", producto);
            }
        }
    }
}
