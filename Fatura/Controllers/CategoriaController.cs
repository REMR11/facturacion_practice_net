using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fatura.Models;

namespace Fatura.Controllers
{
    public class CategoriaController : Controller
    {
        private xstoreContext context;
        public CategoriaController(xstoreContext xstoreContext)
        {
            context = xstoreContext;

        }
        // GET: CategoriaController
        public async Task<ActionResult> Index()
        {
            var categorias = await context.Categoria.ToListAsync();
            return View(categorias);
        }

        // GET: CategoriaController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var categoria = await context.Categoria.FirstOrDefaultAsync((c) => c.Idcategoria == id);
            return View(categoria);
        }

        // GET: CategoriaController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoriaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Categorium categorium)
        {
            try
            {
                var Categoria = context.Categoria.Add(categorium);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CategoriaController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var Categoria = await context.Categoria.FirstOrDefaultAsync((c) => c.Idcategoria == id);
            return View(Categoria);
        }

        // POST: CategoriaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Categorium categorium)
        {
            try
            {
                categorium.Idcategoria = id;
                context.Categoria.Update(categorium);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CategoriaController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CategoriaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}

