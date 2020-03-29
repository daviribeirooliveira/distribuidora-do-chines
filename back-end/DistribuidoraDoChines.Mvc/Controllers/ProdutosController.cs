using System;
using System.Globalization;
using System.IO;
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
    public class ProdutosController : Controller
    {
        private readonly DistribuidoraDoChinesContext _context;

        public ProdutosController(DistribuidoraDoChinesContext context)
        {
            _context = context;
        }

        // GET: Produtos
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString,
            int? pageNumber, uint? idCategoria = null)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NomeSortParm"] = sortOrder == "nome" ? "nome_decr" : "nome";
            ViewData["CategoriaSortParm"] = sortOrder == "categoria" ? "categoria_decr" : "categoria";
            ViewData["PrecoSortParm"] = sortOrder == "preco" ? "preco_decr" : "preco";
            ViewData["UnidadeSortParm"] = sortOrder == "unidade" ? "unidade_decr" : "unidade";
            ViewData["StatusSortParm"] = sortOrder == "status" ? "status_decr" : "status";

            if (searchString != null)
                pageNumber = 1;
            else
                searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;

            var distribuidoraDoChinesContext = _context.Produtos
                .Include(p => p.IdCategoriaNavigation);

            IQueryable<Produtos> produtos = distribuidoraDoChinesContext;

            if (!string.IsNullOrEmpty(searchString))
                produtos = produtos.Where(p =>
                    p.Nome.Contains(searchString) || p.IdCategoriaNavigation.Descricao.Contains(searchString));

            if (idCategoria != null) produtos = produtos.Where(p => p.IdCategoria == idCategoria);

            produtos = sortOrder switch
            {
                "nome" => produtos.OrderBy(p => p.Nome),
                "nome_decr" => produtos.OrderByDescending(p => p.Nome),
                "categoria" => produtos.OrderBy(p => p.IdCategoriaNavigation.Descricao),
                "categoria_decr" => produtos.OrderByDescending(p => p.IdCategoriaNavigation.Descricao),
                "preco" => produtos.OrderBy(p => p.Preco),
                "preco_decr" => produtos.OrderByDescending(p => p.Preco),
                "unidade" => produtos.OrderBy(p => p.Unidade),
                "unidade_decr" => produtos.OrderByDescending(p => p.Unidade),
                "status" => produtos.OrderBy(p => p.Status),
                "status_decr" => produtos.OrderByDescending(p => p.Status),
                _ => produtos.OrderBy(p => p.Nome)
            };

            const int pageSize = 10;

            return View(await PaginatedList<Produtos>.CreateAsync(produtos, pageNumber ?? 1, pageSize));
        }


        // GET: Produtos/Details/5
        public async Task<IActionResult> Details(uint? id)
        {
            if (id == null) return NotFound();

            var produtos = await _context.Produtos
                .Include(p => p.IdCategoriaNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (produtos == null) return NotFound();

            return View(produtos);
        }

        // GET: Produtos/Create
        public IActionResult Create()
        {
            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "Id", "Descricao");

            return View();
        }

        // POST: Produtos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdCategoria,Nome,PrecoEmReais,Unidade,Status")]
            Produtos produtos)
        {
            if (Request.Form.Files.FirstOrDefault() != null)
            {
                var imagem = Request.Form.Files.First();

                if (imagem.ContentType != "image/png") throw new Exception("Formato da imagem deve ser png.");

                using var memoryStream = new MemoryStream();
                
                imagem.CopyTo(memoryStream);
                produtos.Imagem = memoryStream.ToArray();
            }

            produtos.Preco =
                decimal.Parse(
                    produtos.PrecoEmReais.Replace(produtos.PrecoEmReais[produtos.PrecoEmReais.LastIndexOf('.')], ','));

            if (ModelState.IsValid)
            {
                _context.Add(produtos);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "Id", "Descricao", produtos.IdCategoria);

            return View(produtos);
        }

        // GET: Produtos/Edit/5
        public async Task<IActionResult> Edit(uint? id)
        {
            if (id == null) return NotFound();

            var produtos = await _context.Produtos.FindAsync(id);

            if (produtos == null) return NotFound();

            produtos.PrecoEmReais = produtos.Preco.ToString(CultureInfo.InvariantCulture);

            produtos.PrecoEmReais =
                produtos.PrecoEmReais.Replace(produtos.PrecoEmReais[produtos.PrecoEmReais.LastIndexOf(',')], '.');

            if (produtos.Imagem != null) produtos.ImagemBlob = Convert.ToBase64String(produtos.Imagem);

            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "Id", "Descricao", produtos.IdCategoria);

            return View(produtos);
        }

        // POST: Produtos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(uint id,
            [Bind("Id,IdCategoria,Nome,PrecoEmReais,Unidade,Status,Imagem,ImagemBlob,CreatedAt")]
            Produtos produtos)
        {
            if (id != produtos.Id) return NotFound();

            if (Request.Form.Files.FirstOrDefault() != null)
            {
                var imagem = Request.Form.Files.First();

                if (imagem.ContentType != "image/png") throw new Exception("Formato da imagem deve ser png.");

                using var memoryStream = new MemoryStream();
                
                imagem.CopyTo(memoryStream);
                produtos.Imagem = memoryStream.ToArray();
            }
            else
            {
                if (produtos.ImagemBlob != null) produtos.Imagem = Convert.FromBase64String(produtos.ImagemBlob);
            }

            produtos.Preco =
                decimal.Parse(
                    produtos.PrecoEmReais.Replace(produtos.PrecoEmReais[produtos.PrecoEmReais.LastIndexOf('.')], ','));

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Entry(produtos).State = EntityState.Modified;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProdutosExists(produtos.Id)) return NotFound();

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "Id", "Descricao", produtos.IdCategoria);

            return View(produtos);
        }

        // GET: Produtos/Delete/5
        public async Task<IActionResult> Delete(uint? id)
        {
            if (id == null) return NotFound();

            var produtos = await _context.Produtos
                .Include(p => p.IdCategoriaNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (produtos == null) return NotFound();

            return View(produtos);
        }

        // POST: Produtos/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(uint id)
        {
            var produtos = await _context.Produtos.FindAsync(id);

            _context.Produtos.Remove(produtos);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool ProdutosExists(uint id)
        {
            return _context.Produtos.Any(e => e.Id == id);
        }
    }
}