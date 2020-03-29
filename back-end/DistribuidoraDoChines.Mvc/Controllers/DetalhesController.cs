using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using DistribuidoraDoChines.Commons.Helpers;
using DistribuidoraDoChines.Commons.Models;
using DistribuidoraDoChines.Mvc.Data.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DistribuidoraDoChines.Mvc.Controllers
{
    [Authorize]
    public class DetalhesController : Controller
    {
        private readonly DistribuidoraDoChinesContext _context;

        public DetalhesController(DistribuidoraDoChinesContext context)
        {
            _context = context;
        }

        // GET: Detalhes
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString,
            int? pageNumber, uint? idPedido = null)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["PedidoSortParm"] = sortOrder == "pedido" ? "pedido_decr" : "pedido";
            ViewData["ProdutoSortParm"] = sortOrder == "produto" ? "produto_decr" : "produto";
            ViewData["ValorSortParm"] = sortOrder == "valor" ? "valor_decr" : "valor";
            ViewData["QuantidadeSortParm"] = sortOrder == "quantidade" ? "quantidade_decr" : "quantidade";

            if (searchString != null)
                pageNumber = 1;
            else
                searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;

            var distribuidoraDoChinesContext = _context.Detalhes
                .Include(d => d.IdPedidoNavigation)
                .Include(d => d.IdProdutoNavigation);

            IQueryable<Detalhes> detalhes = distribuidoraDoChinesContext;

            if (!string.IsNullOrEmpty(searchString))
                detalhes = detalhes.Where(d =>
                    d.IdPedido == searchString.ParseUInt(0) || d.IdProdutoNavigation.Nome.Contains(searchString));

            if (idPedido != null) detalhes = detalhes.Where(d => d.IdPedido == idPedido);

            detalhes = sortOrder switch
            {
                "pedido" => detalhes.OrderBy(d => d.IdPedido),
                "pedido_decr" => detalhes.OrderByDescending(d => d.IdPedido),
                "produto" => detalhes.OrderBy(d => d.IdProdutoNavigation.Nome),
                "produto_decr" => detalhes.OrderByDescending(d => d.IdProdutoNavigation.Nome),
                "valor" => detalhes.OrderBy(d => d.Valor),
                "valor_decr" => detalhes.OrderByDescending(d => d.Valor),
                "quantidade" => detalhes.OrderBy(d => d.Quantidade),
                "quantidade_decr" => detalhes.OrderByDescending(d => d.Quantidade),
                _ => detalhes.OrderByDescending(d => d.IdPedido)
            };

            const int pageSize = 10;

            return View(await PaginatedList<Detalhes>.CreateAsync(detalhes, pageNumber ?? 1, pageSize));
        }

        // GET: Categorias/FindPedido/5
        public IActionResult FindPedido(uint idPedido)
        {
            return RedirectToAction("Index", "Pedidos", new {IdPedido = idPedido});
        }

        // GET: Detalhes/Details/5
        public async Task<IActionResult> Details(uint? id)
        {
            if (id == null) return NotFound();

            var detalhes = await _context.Detalhes
                .Include(d => d.IdPedidoNavigation)
                .Include(d => d.IdProdutoNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (detalhes == null) return NotFound();

            return View(detalhes);
        }

        // GET: Detalhes/Create
        public IActionResult Create()
        {
            ViewData["IdPedido"] = new SelectList(_context.Pedidos, "Id", "Id");

            ViewData["IdProduto"] = new SelectList(_context.Produtos, "Id", "Nome");

            return View();
        }

        // POST: Detalhes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdPedido,IdProduto,ValorEmReais,Quantidade")]
            Detalhes detalhes)
        {
            detalhes.Valor =
                decimal.Parse(
                    detalhes.ValorEmReais.Replace(detalhes.ValorEmReais[detalhes.ValorEmReais.LastIndexOf('.')], ','));

            if (ModelState.IsValid)
            {
                _context.Add(detalhes);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["IdPedido"] = new SelectList(_context.Pedidos, "Id", "Id", detalhes.IdPedido);

            ViewData["IdProduto"] = new SelectList(_context.Produtos, "Id", "Nome", detalhes.IdProduto);

            return View(detalhes);
        }

        // GET: Detalhes/Edit/5
        public async Task<IActionResult> Edit(uint? id)
        {
            if (id == null) return NotFound();

            var detalhes = await _context.Detalhes.FindAsync(id);

            if (detalhes == null) return NotFound();

            detalhes.ValorEmReais = detalhes.Valor.ToString(CultureInfo.InvariantCulture);

            ViewData["IdPedido"] = new SelectList(_context.Pedidos, "Id", "Id", detalhes.IdPedido);

            ViewData["IdProduto"] = new SelectList(_context.Produtos, "Id", "Nome", detalhes.IdProduto);

            return View(detalhes);
        }

        // POST: Detalhes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(uint id, [Bind("Id,IdPedido,IdProduto,ValorEmReais,Quantidade,CreatedAt")]
            Detalhes detalhes)
        {
            if (id != detalhes.Id) return NotFound();

            detalhes.Valor = decimal.Parse(detalhes.ValorEmReais);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(detalhes);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DetalhesExists(detalhes.Id)) return NotFound();

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["IdPedido"] = new SelectList(_context.Pedidos, "Id", "Id", detalhes.IdPedido);

            ViewData["IdProduto"] = new SelectList(_context.Produtos, "Id", "Nome", detalhes.IdProduto);

            return View(detalhes);
        }

        // GET: Detalhes/Delete/5
        public async Task<IActionResult> Delete(uint? id)
        {
            if (id == null) return NotFound();

            var detalhes = await _context.Detalhes
                .Include(d => d.IdPedidoNavigation)
                .Include(d => d.IdProdutoNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (detalhes == null) return NotFound();

            return View(detalhes);
        }

        // POST: Detalhes/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(uint id)
        {
            var detalhes = await _context.Detalhes.FindAsync(id);

            _context.Detalhes.Remove(detalhes);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool DetalhesExists(uint id)
        {
            return _context.Detalhes.Any(e => e.Id == id);
        }
    }
}