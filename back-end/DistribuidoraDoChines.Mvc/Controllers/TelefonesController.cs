using System.Linq;
using System.Threading.Tasks;
using DistribuidoraDoChines.Commons.Models;
using DistribuidoraDoChines.Mvc.Data.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DistribuidoraDoChines.Mvc.Controllers
{
    [Authorize]
    public class TelefonesController : Controller
    {
        private readonly DistribuidoraDoChinesContext _context;

        public TelefonesController(DistribuidoraDoChinesContext context)
        {
            _context = context;
        }

        // GET: Telefones
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString,
            int? pageNumber, uint? idCliente = null)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["ClienteSortParm"] = sortOrder == "cliente" ? "cliente_decr" : "cliente";
            ViewData["CelularSortParm"] = sortOrder == "celular" ? "celular_decr" : "celular";
            ViewData["ResidencialSortParm"] = sortOrder == "residencial" ? "residencial_decr" : "residencial";
            ViewData["ComercialSortParm"] = sortOrder == "comercial" ? "comercial_decr" : "comercial";

            if (searchString != null)
                pageNumber = 1;
            else
                searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;

            var distribuidoraDoChinesContext = _context.Telefones
                .Include(t => t.IdClienteNavigation);

            IQueryable<Telefones> telefones = distribuidoraDoChinesContext;

            if (!string.IsNullOrEmpty(searchString))
                telefones = telefones.Where(p =>
                    p.IdClienteNavigation.Email.Contains(searchString) || p.Celular.Contains(searchString) ||
                    p.Residencial.Contains(searchString) || p.Comercial.Contains(searchString));

            if (idCliente != null) telefones = telefones.Where(c => c.IdCliente == idCliente);

            telefones = sortOrder switch
            {
                "cliente" => telefones.OrderBy(t => t.IdClienteNavigation.Nome),
                "cliente_decr" => telefones.OrderByDescending(t => t.IdClienteNavigation.Nome),
                "celular" => telefones.OrderBy(t => t.Celular),
                "celular_decr" => telefones.OrderByDescending(t => t.Celular),
                "residencial" => telefones.OrderBy(t => t.Residencial),
                "residencial_decr" => telefones.OrderByDescending(t => t.Residencial),
                "comercial" => telefones.OrderBy(t => t.Comercial),
                "comercial_decr" => telefones.OrderByDescending(t => t.Comercial),
                _ => telefones.OrderByDescending(t => t.Id)
            };

            const int pageSize = 10;

            return View(await PaginatedList<Telefones>.CreateAsync(telefones, pageNumber ?? 1, pageSize));
        }

        // GET: Categorias/FindCliente/5
        public IActionResult FindCliente(uint idCliente)
        {
            return RedirectToAction("Index", "Clientes", new {IdCliente = idCliente});
        }

        // GET: Telefones/Details/5
        public async Task<IActionResult> Details(uint? id)
        {
            if (id == null) return NotFound();

            var telefones = await _context.Telefones
                .Include(t => t.IdClienteNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (telefones == null) return NotFound();

            return View(telefones);
        }

        // GET: Telefones/Create
        public IActionResult Create()
        {
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "Id", "Email");

            return View();
        }

        // POST: Telefones/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdCliente,Residencial,Comercial,Celular")]
            Telefones telefones)
        {
            if (ModelState.IsValid)
            {
                _context.Add(telefones);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["IdCliente"] = new SelectList(_context.Clientes, "Id", "Email", telefones.IdCliente);

            return View(telefones);
        }

        // GET: Telefones/Edit/5
        public async Task<IActionResult> Edit(uint? id)
        {
            if (id == null) return NotFound();

            var telefones = await _context.Telefones.FindAsync(id);

            if (telefones == null) return NotFound();

            ViewData["IdCliente"] = new SelectList(_context.Clientes, "Id", "Email", telefones.IdCliente);

            return View(telefones);
        }

        // POST: Telefones/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(uint id, [Bind("Id,IdCliente,Residencial,Comercial,Celular,CreatedAt")]
            Telefones telefones)
        {
            if (id != telefones.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(telefones);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TelefonesExists(telefones.Id)) return NotFound();

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["IdCliente"] = new SelectList(_context.Clientes, "Id", "Email", telefones.IdCliente);

            return View(telefones);
        }

        // GET: Telefones/Delete/5
        public async Task<IActionResult> Delete(uint? id)
        {
            if (id == null) return NotFound();

            var telefones = await _context.Telefones
                .Include(t => t.IdClienteNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (telefones == null) return NotFound();

            return View(telefones);
        }

        // POST: Telefones/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(uint id)
        {
            var telefones = await _context.Telefones.FindAsync(id);

            _context.Telefones.Remove(telefones);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool TelefonesExists(uint id)
        {
            return _context.Telefones.Any(e => e.Id == id);
        }
    }
}