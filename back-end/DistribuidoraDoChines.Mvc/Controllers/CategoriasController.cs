using System.Linq;
using System.Threading.Tasks;
using DistribuidoraDoChines.Commons.Models;
using DistribuidoraDoChines.Mvc.Data.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DistribuidoraDoChines.Mvc.Controllers
{
    [Authorize]
    public class CategoriasController : Controller
    {
        private readonly DistribuidoraDoChinesContext _context;

        public CategoriasController(DistribuidoraDoChinesContext context)
        {
            _context = context;
        }

        // GET: Categorias
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString,
            int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["DescricaoSortParm"] = sortOrder == "descricao" ? "descricao_decr" : "descricao";
            ViewData["StatusSortParm"] = sortOrder == "status" ? "status_decr" : "status";

            if (searchString != null)
                pageNumber = 1;
            else
                searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;

            var categorias = from c in _context.Categorias select c;

            if (!string.IsNullOrEmpty(searchString))
                categorias = categorias.Where(c => c.Descricao.Contains(searchString));

            categorias = sortOrder switch
            {
                "descricao" => categorias.OrderBy(c => c.Descricao),
                "descricao_decr" => categorias.OrderByDescending(c => c.Descricao),
                "status" => categorias.OrderBy(c => c.Status),
                "status_decr" => categorias.OrderByDescending(c => c.Status),
                _ => categorias.OrderBy(c => c.Descricao)
            };

            const int pageSize = 10;

            return View(await PaginatedList<Categorias>.CreateAsync(categorias, pageNumber ?? 1, pageSize));
        }

        // GET: Categorias/FindProdutos/5
        public IActionResult FindProdutos(uint idCategoria)
        {
            return RedirectToAction("Index", "Produtos", new {IdCategoria = idCategoria});
        }

        // GET: Categorias/Details/5
        public async Task<IActionResult> Details(uint? id)
        {
            if (id == null) return NotFound();

            var categorias = await _context.Categorias
                .FirstOrDefaultAsync(m => m.Id == id);

            if (categorias == null) return NotFound();

            return View(categorias);
        }

        // GET: Categorias/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categorias/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Descricao,Status")] Categorias categorias)
        {
            if (!ModelState.IsValid) return View(categorias);

            _context.Add(categorias);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Categorias/Edit/5
        public async Task<IActionResult> Edit(uint? id)
        {
            if (id == null) return NotFound();

            var categorias = await _context.Categorias.FindAsync(id);

            if (categorias == null) return NotFound();

            return View(categorias);
        }

        // POST: Categorias/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(uint id, [Bind("Id,Descricao,Status,CreatedAt")]
            Categorias categorias)
        {
            if (id != categorias.Id) return NotFound();

            if (!ModelState.IsValid) return View(categorias);

            try
            {
                _context.Update(categorias);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoriasExists(categorias.Id)) return NotFound();

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Categorias/Delete/5
        public async Task<IActionResult> Delete(uint? id)
        {
            if (id == null) return NotFound();

            var categorias = await _context.Categorias
                .FirstOrDefaultAsync(m => m.Id == id);

            if (categorias == null) return NotFound();

            return View(categorias);
        }

        // POST: Categorias/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(uint id)
        {
            var categorias = await _context.Categorias.FindAsync(id);

            _context.Categorias.Remove(categorias);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool CategoriasExists(uint id)
        {
            return _context.Categorias.Any(e => e.Id == id);
        }
    }
}