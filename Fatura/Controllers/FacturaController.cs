using Fatura.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Fatura.Controllers
{
    public class FacturaController : Controller
    {
        private xstoreContext context;
        public FacturaController(xstoreContext xstoreContext)
        {
            context = xstoreContext;
        }

        // GET: FacturaController
        public async Task<ActionResult> Index()
        {
            var factura = await context.Facturas.ToListAsync();
            return View(factura);
        }

        // GET: FacturaController/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null || context.Facturas == null)
            {
                return NotFound();
            }

            var Factura = await context.Facturas
                .FirstOrDefaultAsync(m => m.Idfactura == id);
            if (Factura == null)
            {
                return NotFound();
            }

            return View(Factura);
        }

        // GET: FacturaController/Create
        public async Task<ActionResult> Create(int? id)
        {
            if (id == null)
            {
                var newFactura = new Factura()
                {
                    Total = 0,
                    FechaCreacion = DateTime.Now
                };

                var Factura = context.Facturas.Add(newFactura);
                await context.SaveChangesAsync();

                ViewBag.Producto = await context.Productos.ToListAsync();

                return View(newFactura);
            }
            else
            {
                ViewBag.Producto = await context.Productos.ToListAsync();
                ViewBag.DetalleFacturas = await context.DetalleFacturas.Include((c) => c.IdproductoNavigation)
                                                                        .Where((df) => df.Idfactura == id)
                                                                        .ToListAsync();
                var Factura = context.Facturas.FirstOrDefault((c) => c.Idfactura == id);
                return View(Factura);
            }
        }

        // POST: FacturaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Factura factura)
        {
            try
            {
                var Factura = context.Facturas.Add(factura);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        public async Task<ActionResult> AgregarProducto(int idProducto, int idFactura)
        {
            var newDetalleFactura = new DetalleFactura()
            {
                Idfactura = idFactura,
                Idproducto = idProducto,
                FechaCompra = DateTime.Now,
                Total = 1
            };

            context.DetalleFacturas.Add(newDetalleFactura);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Create), new { id = idFactura });
        }




        // GET: FacturaController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var Factura = await context.Facturas.FirstOrDefaultAsync((c) => c.Idfactura == id);
            return View(Factura);
        }

        // POST: FacturaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Factura factura)
        {
            try
            {
                factura.Idfactura = id;
                context.Facturas.Update(factura);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FacturaController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: FacturaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, Factura factura)
        {
            if (context.Facturas == null)
            {
                return Problem("Entity set 'xstoreContext.Facturacion'  is null.");
            }
            var Factura = await context.Facturas.FindAsync();
            if (factura != null)
            {
                context.Facturas.Remove(factura);
            }

            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool ProductosExists(int id)
        {
            return context.Facturas.Any(e => e.Idfactura == id);
        }


    }
}

