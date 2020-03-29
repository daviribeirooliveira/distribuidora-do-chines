using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DistribuidoraDoChines.Api.Data.Context;
using DistribuidoraDoChines.Commons.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DistribuidoraDoChines.Api.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly DistribuidoraDoChinesContext _context;

        public ProdutosController(DistribuidoraDoChinesContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produtos>>> GetProdutos([FromQuery] int idCategoria)
        {
            var produtos = _context.Produtos.Where(p => p.Status);

            if (idCategoria != 0) produtos = produtos.Where(p => p.IdCategoria == idCategoria);

            return await produtos
                .OrderBy(p => p.IdCategoriaNavigation.Descricao)
                .ThenBy(p => p.Nome)
                .ToListAsync();
        }
    }
}