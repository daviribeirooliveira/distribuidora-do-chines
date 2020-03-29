using System.Linq;
using System.Threading.Tasks;
using DistribuidoraDoChines.Commons.Helpers;
using DistribuidoraDoChines.Commons.Models;
using DistribuidoraDoChines.Mvc.Data.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DistribuidoraDoChines.Mvc.Controllers
{
    [Authorize]
    public class ClientesController : Controller
    {
        private readonly DistribuidoraDoChinesContext _context;

        public ClientesController(DistribuidoraDoChinesContext context)
        {
            _context = context;
        }

        // GET: Clientes
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString,
            int? pageNumber, int? idCliente)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NomeSortParm"] = sortOrder == "nome" ? "nome_decr" : "nome";
            ViewData["EmailSortParm"] = sortOrder == "email" ? "email_decr" : "email";
            ViewData["StatusSortParm"] = sortOrder == "status" ? "status_decr" : "status";

            if (searchString != null)
                pageNumber = 1;
            else
                searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;

            var clientes = from c in _context.Clientes select c;

            if (!string.IsNullOrEmpty(searchString))
                clientes = clientes.Where(c => c.Nome.Contains(searchString) || c.Email.Contains(searchString));

            if (idCliente != null) clientes = clientes.Where(c => c.Id == idCliente);

            clientes = sortOrder switch
            {
                "nome" => clientes.OrderBy(c => c.Nome),
                "nome_decr" => clientes.OrderByDescending(c => c.Nome),
                "email" => clientes.OrderBy(c => c.Nome),
                "email_decr" => clientes.OrderByDescending(c => c.Nome),
                "status" => clientes.OrderBy(c => c.Status),
                "status_decr" => clientes.OrderByDescending(c => c.Status),
                _ => clientes.OrderBy(c => c.Nome)
            };

            const int pageSize = 10;

            return View(await PaginatedList<Clientes>.CreateAsync(clientes, pageNumber ?? 1, pageSize));
        }

        // GET: Categorias/FindEnderecos/5
        public IActionResult FindEnderecos(uint idCliente)
        {
            return RedirectToAction("Index", "Enderecos", new {IdCliente = idCliente});
        }

        // GET: Categorias/FindTelefones/5
        public IActionResult FindTelefones(uint idCliente)
        {
            return RedirectToAction("Index", "Telefones", new {IdCliente = idCliente});
        }

        // GET: Categorias/FindPedidos/5
        public IActionResult FindPedidos(uint idCliente)
        {
            return RedirectToAction("Index", "Pedidos", new {IdCliente = idCliente});
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(uint? id)
        {
            if (id == null) return NotFound();

            var clientes = await _context.Clientes
                .FirstOrDefaultAsync(m => m.Id == id);

            if (clientes == null) return NotFound();

            return View(clientes);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Email,Senha,Status")] Clientes clientes)
        {
            if (!ModelState.IsValid) return View(clientes);

            clientes.Senha = SecurePasswordHasher.Hash(clientes.Senha);

            _context.Add(clientes);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(uint? id)
        {
            if (id == null) return NotFound();

            var clientes = await _context.Clientes.FindAsync(id);

            if (clientes == null) return NotFound();

            return View(clientes);
        }

        // POST: Clientes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(uint id, [Bind("Id,Nome,Email,Senha,Status,CreatedAt")]
            Clientes clientes)
        {
            if (id != clientes.Id) return NotFound();

            if (!ModelState.IsValid) return View(clientes);

            try
            {
                if (!clientes.Senha.Contains("MYHASH")) clientes.Senha = SecurePasswordHasher.Hash(clientes.Senha);

                _context.Update(clientes);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientesExists(clientes.Id)) return NotFound();

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(uint? id)
        {
            if (id == null) return NotFound();

            var clientes = await _context.Clientes
                .FirstOrDefaultAsync(m => m.Id == id);

            if (clientes == null) return NotFound();

            return View(clientes);
        }

        // POST: Clientes/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(uint id)
        {
            var clientes = await _context.Clientes.FindAsync(id);

            _context.Clientes.Remove(clientes);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool ClientesExists(uint id)
        {
            return _context.Clientes.Any(e => e.Id == id);
        }
    }
}