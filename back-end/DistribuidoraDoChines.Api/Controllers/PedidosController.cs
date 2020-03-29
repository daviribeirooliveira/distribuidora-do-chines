using System;
using System.Threading.Tasks;
using DistribuidoraDoChines.Api.Data.Context;
using DistribuidoraDoChines.Commons.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DistribuidoraDoChines.Api.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class PedidosController : ControllerBase
    {
        private readonly DistribuidoraDoChinesContext _context;

        public PedidosController(DistribuidoraDoChinesContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<bool>> PostPedidos([FromBody] Pedidos pedido)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                _context.Pedidos.Add(pedido);

                return Convert.ToBoolean(await _context.SaveChangesAsync());
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
    }
}