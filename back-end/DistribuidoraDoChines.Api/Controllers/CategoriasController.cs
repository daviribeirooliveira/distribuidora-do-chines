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
    public class CategoriasController : ControllerBase
    {
        private readonly DistribuidoraDoChinesContext _context;

        public CategoriasController(DistribuidoraDoChinesContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categorias>>> GetCategorias()
        {
            return await _context.Categorias
                .Where(c => c.Status)
                .OrderBy(c => c.Descricao)
                .ToListAsync();
        }
    }
}