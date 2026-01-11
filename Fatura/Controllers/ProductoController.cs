using Fatura.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fatura.Controllers
{
    public class ProductoController : Controller
    {
        public xstoreContext context;
        public ProductoController(xstoreContext xstorecontext)
        {
            context = xstorecontext;
        }

        // GET: ProductoController
        public async Task<ActionResult> Index()
        {
            var Producto = await context.Productos.ToListAsync();
            return View(Producto);
        }

        // GET: ProductoController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var Producto = await context.Productos.FirstOrDefaultAsync((c) => c.Idproducto == id);
            return View(Producto);
        }

        // GET: ProductoController/Create 
        public async Task<ActionResult> Create()
        {
            ViewBag.Marca = await context.Marcas.ToListAsync();
            ViewBag.Categoria = await context.Categoria.ToListAsync();

            return View();
        }

        // POST: ProductoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Producto producto)
        {
            try
            {
                var Producto = context.Productos.Add(producto);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductoController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var Producto = await context.Productos.FirstOrDefaultAsync((c) => c.Idproducto == id);
            return View(Producto);
        }

        // POST: ProductoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Producto producto)
        {
            try
            {
                producto.Idproducto = id;
                context.Productos.Update(producto);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductoController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            if (id == null || context.Productos == null)
            {
                return NotFound();
            }

            var Producto = await context.Productos
                .FirstOrDefaultAsync(m => m.Idproducto == id);
            if (Producto == null)
            {
                return NotFound();
            }

            return View(Producto);
        }

        // POST: ProductoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, Producto producto)
        {
            if (context.Productos == null)
            {
                return Problem("Entity set 'xstoreContext.Productos'  is null.");
            }
            var Producto = await context.Productos.FindAsync();
            if (producto != null)
            {
                context.Productos.Remove(producto);
            }

            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductosExists(int id)
        {
            return context.Productos.Any(e => e.Idproducto == id);
        }
    }
}
