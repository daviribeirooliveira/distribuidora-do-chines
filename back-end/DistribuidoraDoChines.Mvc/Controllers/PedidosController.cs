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
    public class PedidosController : Controller
    {
        private readonly DistribuidoraDoChinesContext _context;

        public PedidosController(DistribuidoraDoChinesContext context)
        {
            _context = context;
        }

        // GET: Pedidos
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString,
            int? pageNumber, uint? idCliente = null, uint? idPedido = null)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["PedidoSortParm"] = sortOrder == "pedido" ? "pedido_decr" : "pedido";
            ViewData["DataSortParm"] = sortOrder == "data" ? "data_decr" : "data";
            ViewData["ClienteSortParm"] = sortOrder == "cliente" ? "cliente_decr" : "cliente";
            ViewData["EnderecoSortParm"] = sortOrder == "endereco" ? "endereco_decr" : "endereco";
            ViewData["ValorSortParm"] = sortOrder == "valor" ? "valor_decr" : "valor";
            ViewData["FreteSortParm"] = sortOrder == "frete" ? "frete_decr" : "frete";
            ViewData["TrocoSortParm"] = sortOrder == "troco" ? "troco_decr" : "troco";
            ViewData["TipoDePagamentoSortParm"] =
                sortOrder == "tipo_de_pagamento" ? "tipo_de_pagamento_decr" : "tipo_de_pagamento";

            if (searchString != null)
                pageNumber = 1;
            else
                searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;

            var distribuidoraDoChinesContext = _context.Pedidos
                .Include(p => p.IdClienteEnderecoNavigation)
                .Include(p => p.IdClienteNavigation)
                .Include(p => p.IdTiposDePagamentoNavigation);

            IQueryable<Pedidos> pedidos = distribuidoraDoChinesContext;

            if (!string.IsNullOrEmpty(searchString))
                pedidos = pedidos
                    .Where(p => p.Id == searchString.ParseUInt(0) ||
                                p.IdClienteNavigation.Email.Contains(searchString));

            if (idCliente != null)
                pedidos = pedidos
                    .Where(p => p.IdCliente == idCliente);

            if (idPedido != null)
                pedidos = pedidos
                    .Where(p => p.Id == idPedido);

            pedidos = sortOrder switch
            {
                "pedido" => pedidos.OrderBy(p => p.IdClienteNavigation.Id),
                "pedido_decr" => pedidos.OrderByDescending(p => p.IdClienteNavigation.Id),
                "data" => pedidos.OrderBy(p => p.Data),
                "data_decr" => pedidos.OrderByDescending(p => p.Data),
                "cliente" => pedidos.OrderBy(p => p.IdClienteNavigation.Email),
                "cliente_decr" => pedidos.OrderByDescending(p => p.IdClienteNavigation.Email),
                "endereco" => pedidos.OrderBy(p => p.IdClienteEnderecoNavigation.Nome),
                "endereco_decr" => pedidos.OrderByDescending(p => p.IdClienteEnderecoNavigation.Nome),
                "valor" => pedidos.OrderBy(p => p.Valor),
                "valor_decr" => pedidos.OrderByDescending(p => p.Valor),
                "frete" => pedidos.OrderBy(p => p.ValorFrete),
                "frete_decr" => pedidos.OrderByDescending(p => p.ValorFrete),
                "troco" => pedidos.OrderBy(p => p.ValorFrete),
                "troco_decr" => pedidos.OrderByDescending(p => p.ValorFrete),
                "tipo_de_pagamento" => pedidos.OrderBy(p => p.IdTiposDePagamentoNavigation.Descricao),
                "tipo_de_pagamento_decr" => pedidos.OrderByDescending(p => p.IdTiposDePagamentoNavigation.Descricao),
                _ => pedidos.OrderByDescending(p => p.Data)
            };

            const int pageSize = 10;

            return View(await PaginatedList<Pedidos>.CreateAsync(pedidos, pageNumber ?? 1, pageSize));
        }

        // GET: Categorias/FindCliente/5
        public IActionResult FindCliente(uint idCliente)
        {
            return RedirectToAction("Index", "Clientes", new {IdCliente = idCliente});
        }

        // GET: Categorias/FindEndereco/5
        public IActionResult FindEndereco(uint idClienteEndereco)
        {
            return RedirectToAction("Index", "Enderecos", new {IdClienteEndereco = idClienteEndereco});
        }

        // GET: Categorias/FindDetalhes/5
        public IActionResult FindDetalhes(uint idPedido)
        {
            return RedirectToAction("Index", "Detalhes", new {IdPedido = idPedido});
        }

        // GET: Pedidos/Details/5
        public async Task<IActionResult> Details(uint? id)
        {
            if (id == null) return NotFound();

            var pedidos = await _context.Pedidos
                .Include(p => p.IdClienteEnderecoNavigation)
                .Include(p => p.IdClienteNavigation)
                .Include(p => p.IdTiposDePagamentoNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (pedidos == null) return NotFound();

            return View(pedidos);
        }

        // GET: Pedidos/Create
        public IActionResult Create()
        {
            ViewData["IdClienteEndereco"] = new SelectList(_context.Enderecos, "Id", "Nome");

            ViewData["IdCliente"] = new SelectList(_context.Clientes, "Id", "Email");

            ViewData["IdTiposDePagamento"] = new SelectList(_context.TiposDePagamento, "Id", "Descricao");

            return View();
        }

        // POST: Pedidos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,IdClienteEndereco,IdCliente,IdTiposDePagamento,ValorEmReais,ValorFreteEmReais")]
            Pedidos pedidos)
        {
            pedidos.Valor =
                decimal.Parse(pedidos.ValorEmReais.Replace(pedidos.ValorEmReais[pedidos.ValorEmReais.LastIndexOf('.')],
                    ','));

            pedidos.ValorFrete =
                decimal.Parse(
                    pedidos.ValorFreteEmReais.Replace(
                        pedidos.ValorFreteEmReais[pedidos.ValorFreteEmReais.LastIndexOf('.')], ','));

            if (ModelState.IsValid)
            {
                _context.Add(pedidos);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["IdClienteEndereco"] = new SelectList(_context.Enderecos, "Id", "Nome", pedidos.IdClienteEndereco);

            ViewData["IdCliente"] = new SelectList(_context.Clientes, "Id", "Email", pedidos.IdCliente);

            ViewData["IdTiposDePagamento"] =
                new SelectList(_context.TiposDePagamento, "Id", "Descricao", pedidos.IdTiposDePagamento);

            return View(pedidos);
        }

        // GET: Pedidos/Edit/5
        public async Task<IActionResult> Edit(uint? id)
        {
            if (id == null) return NotFound();

            var pedidos = await _context.Pedidos.FindAsync(id);

            if (pedidos == null) return NotFound();

            pedidos.ValorEmReais = pedidos.Valor.ToString(CultureInfo.InvariantCulture);

            pedidos.ValorFreteEmReais = pedidos.ValorFrete.ToString(CultureInfo.InvariantCulture);

            ViewData["IdClienteEndereco"] = new SelectList(_context.Enderecos, "Id", "Nome", pedidos.IdClienteEndereco);

            ViewData["IdCliente"] = new SelectList(_context.Clientes, "Id", "Email", pedidos.IdCliente);

            ViewData["IdTiposDePagamento"] =
                new SelectList(_context.TiposDePagamento, "Id", "Descricao", pedidos.IdTiposDePagamento);

            return View(pedidos);
        }

        public async Task<IActionResult> EditStatus([Bind("Id, Status")] Pedidos pedido)
        {
            _context.Pedidos.Attach(pedido);
            _context.Entry(pedido).Property(p => p.Status).IsModified = true;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // POST: Pedidos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(uint id,
            [Bind("Id,IdClienteEndereco,IdCliente,IdTiposDePagamento,Data,ValorEmReais,ValorFreteEmReais,CreatedAt")]
            Pedidos pedidos)
        {
            if (id != pedidos.Id) return NotFound();

            pedidos.Valor = decimal.Parse(pedidos.ValorEmReais);

            pedidos.ValorFrete = decimal.Parse(pedidos.ValorFreteEmReais);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pedidos);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PedidosExists(pedidos.Id)) return NotFound();

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["IdClienteEndereco"] = new SelectList(_context.Enderecos, "Id", "Nome", pedidos.IdClienteEndereco);

            ViewData["IdCliente"] = new SelectList(_context.Clientes, "Id", "Email", pedidos.IdCliente);

            ViewData["IdTiposDePagamento"] =
                new SelectList(_context.TiposDePagamento, "Id", "Descricao", pedidos.IdTiposDePagamento);

            return View(pedidos);
        }

        // GET: Pedidos/Delete/5
        public async Task<IActionResult> Delete(uint? id)
        {
            if (id == null) return NotFound();

            var pedidos = await _context.Pedidos
                .Include(p => p.IdClienteEnderecoNavigation)
                .Include(p => p.IdClienteNavigation)
                .Include(p => p.IdTiposDePagamentoNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (pedidos == null) return NotFound();

            return View(pedidos);
        }

        // POST: Pedidos/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(uint id)
        {
            var pedidos = await _context.Pedidos.FindAsync(id);

            _context.Pedidos.Remove(pedidos);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool PedidosExists(uint id)
        {
            return _context.Pedidos.Any(e => e.Id == id);
        }
    }
}