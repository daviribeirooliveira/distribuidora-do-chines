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
    public class DeliveryController : ControllerBase
    {
        private readonly DistribuidoraDoChinesContext _context;

        public DeliveryController(DistribuidoraDoChinesContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<Delivery>> GetDelivery([FromQuery] uint idEndereco)
        {
            try
            {
                var endereco = await _context.Enderecos.FindAsync(idEndereco);

                return new Delivery(endereco);
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