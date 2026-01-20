using Fatura.Models.Catalogos;
using Fatura.Models.Enums;
using Fatura.Models.ViewModels;
using Fatura.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Fatura.Controllers
{
    /// <summary>
    /// Controlador para gestionar productos y servicios.
    /// Proporciona funcionalidad para listar, buscar, filtrar, crear, editar y eliminar productos.
    /// </summary>
    [Route("Productos")]
    public class ProductosController : Controller
    {
        private readonly IProductoService _productoService;
        private readonly ICategoriaService _categoriaService;
        private readonly IMarcaService _marcaService;
        private readonly IUnidadMedidaService _unidadMedidaService;

        public ProductosController(
            IProductoService productoService,
            ICategoriaService categoriaService,
            IMarcaService marcaService,
            IUnidadMedidaService unidadMedidaService)
        {
            _productoService = productoService;
            _categoriaService = categoriaService;
            _marcaService = marcaService;
            _unidadMedidaService = unidadMedidaService;
        }

        /// <summary>
        /// Lista todos los productos con opciones de búsqueda y filtros.
        /// </summary>
        public async Task<IActionResult> Index(string? searchTerm = null, TipoProducto? tipo = null, bool showModal = false, int? editId = null)
        {
            IEnumerable<Producto> productos;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                productos = await _productoService.SearchAsync(searchTerm);
            }
            else
            {
                productos = await _productoService.GetAllAsync();
            }

            if (tipo.HasValue)
            {
                productos = await _productoService.FilterByTipoAsync(tipo);
                productos = productos.Where(p => p.Tipo == tipo.Value);
            }

            ViewBag.SearchTerm = searchTerm;
            ViewBag.Tipo = tipo;
            ViewBag.Categorias = await _categoriaService.GetAllAsync();
            ViewBag.Marcas = await _marcaService.GetAllAsync();
            ViewBag.UnidadesMedida = await _unidadMedidaService.GetAllAsync();
            ViewBag.ShowModal = showModal;
            if (editId.HasValue)
            {
                ViewBag.ShowEditModal = true;
                ViewBag.EditProductoId = editId.Value;
            }

            var viewModel = new ProductoIndexViewModel
            {
                Productos = productos,
                NuevoProducto = new Producto
                {
                    Tipo = TipoProducto.Servicio
                }
            };

            return View(viewModel);
        }

        /// <summary>
        /// Endpoint API para búsqueda de productos (para AJAX).
        /// </summary>
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

        /// <summary>
        /// Obtiene los detalles de un producto.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var producto = await _productoService.GetByIdAsync(id);
                if (producto == null)
                {
                    return NotFound();
                }
                return View(producto);
            }
            catch (Fatura.Exceptions.EntityNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Muestra el formulario para crear un nuevo producto.
        /// </summary>
        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Marcas = await _marcaService.GetAllAsync();
            ViewBag.Categorias = await _categoriaService.GetAllAsync();
            ViewBag.UnidadesMedida = await _unidadMedidaService.GetAllAsync();

            return RedirectToAction(nameof(Index), new { showModal = true });
        }

        /// <summary>
        /// Crea un nuevo producto.
        /// </summary>
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind(Prefix = "NuevoProducto")] Producto producto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _productoService.CreateAsync(producto);
                    return RedirectToAction(nameof(Index));
                }
                var productos = await _productoService.GetAllAsync();
                ViewBag.Marcas = await _marcaService.GetAllAsync();
                ViewBag.Categorias = await _categoriaService.GetAllAsync();
                ViewBag.UnidadesMedida = await _unidadMedidaService.GetAllAsync();
                ViewBag.ShowModal = true;
                return View("Index", new ProductoIndexViewModel
                {
                    Productos = productos,
                    NuevoProducto = producto
                });
            }
            catch (Fatura.Exceptions.BusinessRuleException ex)
            {
                ModelState.AddModelError("", ex.Message);
                var productos = await _productoService.GetAllAsync();
                ViewBag.Marcas = await _marcaService.GetAllAsync();
                ViewBag.Categorias = await _categoriaService.GetAllAsync();
                ViewBag.UnidadesMedida = await _unidadMedidaService.GetAllAsync();
                ViewBag.ShowModal = true;
                return View("Index", new ProductoIndexViewModel
                {
                    Productos = productos,
                    NuevoProducto = producto
                });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al crear el producto: {ex.Message}");
                var productos = await _productoService.GetAllAsync();
                ViewBag.Marcas = await _marcaService.GetAllAsync();
                ViewBag.Categorias = await _categoriaService.GetAllAsync();
                ViewBag.UnidadesMedida = await _unidadMedidaService.GetAllAsync();
                ViewBag.ShowModal = true;
                return View("Index", new ProductoIndexViewModel
                {
                    Productos = productos,
                    NuevoProducto = producto
                });
            }
        }

        /// <summary>
        /// Muestra el formulario para editar un producto.
        /// </summary>
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var producto = await _productoService.GetByIdAsync(id);
                if (producto == null)
                {
                    return NotFound();
                }
                ViewBag.Marcas = await _marcaService.GetAllAsync();
                ViewBag.Categorias = await _categoriaService.GetAllAsync();
                ViewBag.UnidadesMedida = await _unidadMedidaService.GetAllAsync();
                return RedirectToAction(nameof(Index), new { editId = id });
            }
            catch (Fatura.Exceptions.EntityNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Actualiza un producto existente.
        /// </summary>
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
                producto.IdProducto = id;
                ViewBag.Marcas = await _marcaService.GetAllAsync();
                ViewBag.Categorias = await _categoriaService.GetAllAsync();
                ViewBag.UnidadesMedida = await _unidadMedidaService.GetAllAsync();
                ViewBag.ShowEditModal = true;
                ViewBag.EditProductoId = id;
                var productos = await _productoService.GetAllAsync();
                var productosActualizados = productos
                    .Select(p => p.IdProducto == id ? producto : p)
                    .ToList();
                return View("Index", new ProductoIndexViewModel
                {
                    Productos = productosActualizados,
                    NuevoProducto = new Producto
                    {
                        Tipo = TipoProducto.Servicio
                    }
                });
            }
            catch (Fatura.Exceptions.EntityNotFoundException)
            {
                return NotFound();
            }
            catch (Fatura.Exceptions.BusinessRuleException ex)
            {
                ModelState.AddModelError("", ex.Message);
                producto.IdProducto = id;
                ViewBag.Marcas = await _marcaService.GetAllAsync();
                ViewBag.Categorias = await _categoriaService.GetAllAsync();
                ViewBag.UnidadesMedida = await _unidadMedidaService.GetAllAsync();
                ViewBag.ShowEditModal = true;
                ViewBag.EditProductoId = id;
                var productos = await _productoService.GetAllAsync();
                var productosActualizados = productos
                    .Select(p => p.IdProducto == id ? producto : p)
                    .ToList();
                return View("Index", new ProductoIndexViewModel
                {
                    Productos = productosActualizados,
                    NuevoProducto = new Producto
                    {
                        Tipo = TipoProducto.Servicio
                    }
                });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al actualizar el producto: {ex.Message}");
                producto.IdProducto = id;
                ViewBag.Marcas = await _marcaService.GetAllAsync();
                ViewBag.Categorias = await _categoriaService.GetAllAsync();
                ViewBag.UnidadesMedida = await _unidadMedidaService.GetAllAsync();
                ViewBag.ShowEditModal = true;
                ViewBag.EditProductoId = id;
                var productos = await _productoService.GetAllAsync();
                var productosActualizados = productos
                    .Select(p => p.IdProducto == id ? producto : p)
                    .ToList();
                return View("Index", new ProductoIndexViewModel
                {
                    Productos = productosActualizados,
                    NuevoProducto = new Producto
                    {
                        Tipo = TipoProducto.Servicio
                    }
                });
            }
        }

        /// <summary>
        /// Muestra la confirmación para eliminar un producto.
        /// </summary>
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var producto = await _productoService.GetByIdAsync(id);
                if (producto == null)
                {
                    return NotFound();
                }
                return View(producto);
            }
            catch (Fatura.Exceptions.EntityNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Elimina un producto.
        /// </summary>
        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                await _productoService.DeleteAsync(id);
                TempData["Success"] = "Producto eliminado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Fatura.Exceptions.EntityNotFoundException)
            {
                TempData["Error"] = "El producto no fue encontrado.";
                return RedirectToAction(nameof(Index));
            }
            catch (Fatura.Exceptions.BusinessRuleException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al eliminar el producto: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
