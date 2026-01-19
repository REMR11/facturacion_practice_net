using Fatura.Models.Inventario;
using Fatura.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Fatura.Controllers
{
    [Route("Inventario")]
    public class InventarioController : Controller
    {
        private readonly IProductoService _productoService;

        public InventarioController(IProductoService productoService)
        {
            _productoService = productoService;
        }

        // ===============================
        // LISTADO DE INVENTARIO
        // ===============================
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var productos = await _productoService.GetProductosActivosAsync();

            var inventario = productos.Select(p => new Inventario
            {
                IdProducto = p.IdProducto,
                Producto = p,
                Stock = p.Stock,
                StockMinimo = p.StockMinimo,
                FechaActualizacion = DateTime.Now
            }).ToList();

            return View(inventario);
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
        public IActionResult Create(Inventario inventario)
        {
            if (!ModelState.IsValid)
                return View(inventario);

            // 🔴 NO SE CREA INVENTARIO DIRECTO
            // El stock se maneja en Producto
           

            return RedirectToAction(nameof(Index));
        }

        // ===============================
        // GET: EDIT
        // ===============================
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var producto = await _productoService.GetByIdAsync(id);

            if (producto == null)
                return NotFound();

            var inventario = new Inventario
            {
                IdProducto = producto.IdProducto,
                Producto = producto,
                Stock = producto.Stock,
                StockMinimo = producto.StockMinimo
            };

            return View(inventario);
        }

        // ===============================
        // POST: EDIT
        // ===============================
        [HttpPost("Edit")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Inventario inventario)
        {
            if (!ModelState.IsValid)
                return View(inventario);

           
            return RedirectToAction(nameof(Index));
        }
    }
}
