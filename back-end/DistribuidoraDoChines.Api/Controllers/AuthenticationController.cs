using System;
using System.Linq;
using System.Threading.Tasks;
using DistribuidoraDoChines.Api.Data.Context;
using DistribuidoraDoChines.Commons.Helpers;
using DistribuidoraDoChines.Commons.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DistribuidoraDoChines.Api.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly DistribuidoraDoChinesContext _context;

        public AuthenticationController(DistribuidoraDoChinesContext context)
        {
            _context = context;
        }
        
        [HttpPost]
        public async Task<ActionResult<Clientes>> AppAuthentication([FromBody] Credentials credential)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var cliente = await _context.Clientes
                    .Include(c => c.Enderecos)
                    .Include(c => c.Telefones)
                    .Where(c => c.Email == credential.Email)
                    .FirstOrDefaultAsync();

                if (string.IsNullOrEmpty(credential.Email)) return Unauthorized("Email inválido");

                if (!SecurePasswordHasher.Verify(credential.Senha, cliente.Senha))
                    return Unauthorized("Senha inválida!");

                if (cliente.Status == false) return Unauthorized("Usuário desativado!");

                return cliente;
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