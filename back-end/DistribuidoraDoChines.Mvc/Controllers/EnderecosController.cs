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
    public class EnderecosController : Controller
    {
        private readonly DistribuidoraDoChinesContext _context;

        public EnderecosController(DistribuidoraDoChinesContext context)
        {
            _context = context;
        }

        // GET: Enderecos
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString,
            int? pageNumber, uint? idClienteEndereco = null)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["ClienteSortParm"] = sortOrder == "cliente" ? "cliente_decr" : "cliente";
            ViewData["NomeSortParm"] = sortOrder == "nome" ? "nome_decr" : "nome";
            ViewData["CepSortParm"] = sortOrder == "cep" ? "cep_decr" : "cep";
            ViewData["RuaSortParm"] = sortOrder == "rua" ? "rua_decr" : "rua";
            ViewData["BairroSortParm"] = sortOrder == "bairro" ? "bairro_decr" : "bairro";
            ViewData["NumeroSortParm"] = sortOrder == "numero" ? "numero_decr" : "numero";
            ViewData["ComplementoSortParm"] = sortOrder == "complemento" ? "complemento_decr" : "complemento";
            ViewData["ReferenciaSortParm"] = sortOrder == "referencia" ? "referencia_decr" : "referencia";

            if (searchString != null)
                pageNumber = 1;
            else
                searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;

            var distribuidoraDoChinesContext = _context.Enderecos
                .Include(e => e.IdClienteNavigation);

            IQueryable<Enderecos> enderecos = distribuidoraDoChinesContext;

            if (!string.IsNullOrEmpty(searchString))
                enderecos = enderecos.Where(e =>
                    e.IdClienteNavigation.Email.Contains(searchString) || e.Rua.Contains(searchString) ||
                    e.Bairro.Contains(searchString));

            if (idClienteEndereco != null) enderecos = enderecos.Where(e => e.Id == idClienteEndereco);

            enderecos = sortOrder switch
            {
                "cliente" => enderecos.OrderBy(e => e.IdClienteNavigation.Nome),
                "cliente_decr" => enderecos.OrderByDescending(e => e.IdClienteNavigation.Nome),
                "nome" => enderecos.OrderBy(e => e.Nome),
                "nome_decr" => enderecos.OrderByDescending(e => e.Nome),
                "cep" => enderecos.OrderBy(e => e.Cep),
                "cep_decr" => enderecos.OrderByDescending(e => e.Cep),
                "rua" => enderecos.OrderBy(e => e.Rua),
                "rua_decr" => enderecos.OrderByDescending(e => e.Rua),
                "bairro" => enderecos.OrderBy(e => e.Bairro),
                "bairro_decr" => enderecos.OrderByDescending(e => e.Bairro),
                "numero" => enderecos.OrderBy(e => e.Numero),
                "numero_decr" => enderecos.OrderByDescending(e => e.Numero),
                "complemento" => enderecos.OrderBy(e => e.Complemento),
                "complemento_decr" => enderecos.OrderByDescending(e => e.Complemento),
                "referencia" => enderecos.OrderBy(e => e.Referencia),
                "referencia_decr" => enderecos.OrderByDescending(e => e.Referencia),
                _ => enderecos.OrderByDescending(e => e.Id)
            };

            const int pageSize = 10;

            return View(await PaginatedList<Enderecos>.CreateAsync(enderecos, pageNumber ?? 1, pageSize));
        }

        // GET: Categorias/FindCliente/5
        public IActionResult FindCliente(uint idCliente)
        {
            return RedirectToAction("Index", "Clientes", new {IdCliente = idCliente});
        }

        // GET: Enderecos/Details/5
        public async Task<IActionResult> Details(uint? id)
        {
            if (id == null) return NotFound();

            var enderecos = await _context.Enderecos
                .Include(e => e.IdClienteNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (enderecos == null) return NotFound();

            return View(enderecos);
        }

        // GET: Enderecos/Create
        public IActionResult Create()
        {
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "Id", "Email");

            return View();
        }

        // POST: Enderecos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdCliente,Nome,Cep,Rua,Bairro,Numero,Complemento,Referencia")]
            Enderecos enderecos)
        {
            if (ModelState.IsValid)
            {
                _context.Add(enderecos);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["IdCliente"] = new SelectList(_context.Clientes, "Id", "Email", enderecos.IdCliente);

            return View(enderecos);
        }

        // GET: Enderecos/Edit/5
        public async Task<IActionResult> Edit(uint? id)
        {
            if (id == null) return NotFound();

            var enderecos = await _context.Enderecos.FindAsync(id);

            if (enderecos == null) return NotFound();

            ViewData["IdCliente"] = new SelectList(_context.Clientes, "Id", "Email", enderecos.IdCliente);

            return View(enderecos);
        }

        // POST: Enderecos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(uint id,
            [Bind("Id,IdCliente,Nome,Cep,Rua,Bairro,Numero,Complemento,Referencia,CreatedAt")]
            Enderecos enderecos)
        {
            if (id != enderecos.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(enderecos);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnderecosExists(enderecos.Id)) return NotFound();

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["IdCliente"] = new SelectList(_context.Clientes, "Id", "Email", enderecos.IdCliente);

            return View(enderecos);
        }

        // GET: Enderecos/Delete/5
        public async Task<IActionResult> Delete(uint? id)
        {
            if (id == null) return NotFound();

            var enderecos = await _context.Enderecos
                .Include(e => e.IdClienteNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (enderecos == null) return NotFound();

            return View(enderecos);
        }

        // POST: Enderecos/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(uint id)
        {
            var enderecos = await _context.Enderecos.FindAsync(id);

            _context.Enderecos.Remove(enderecos);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool EnderecosExists(uint id)
        {
            return _context.Enderecos.Any(e => e.Id == id);
        }
    }
}