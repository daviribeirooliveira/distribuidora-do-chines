using System;
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
    public class EnderecosController : ControllerBase
    {
        private readonly DistribuidoraDoChinesContext _context;

        public EnderecosController(DistribuidoraDoChinesContext context)
        {
            _context = context;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> PutEnderecos(uint id, [FromBody] Enderecos endereco)
        {
            try
            {
                if (id != endereco.Id) return BadRequest();

                _context.Entry(endereco).State = EntityState.Modified;

                return Convert.ToBoolean(await _context.SaveChangesAsync());
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EnderecosExists(id)) return NotFound();

                throw;
            }
#if DEBUG
            catch (Exception exception)
            {
                return StatusCode(500, exception.GetBaseException().Message);
            }
#else
            catch (Exception exception)
            {
                return StatusCode(500, "Não foi possível atender a solicitação no momento, por favor tente novamente mais tarde.");
            }
#endif
        }

        private bool EnderecosExists(uint id)
        {
            return _context.Enderecos.Any(e => e.Id == id);
        }
    }
}