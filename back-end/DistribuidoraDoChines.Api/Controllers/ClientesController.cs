using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DistribuidoraDoChines.Api.Data.Context;
using DistribuidoraDoChines.Commons.Helpers;
using DistribuidoraDoChines.Commons.Models;
using DistribuidoraDoChines.Commons.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DistribuidoraDoChines.Api.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly DistribuidoraDoChinesContext _context;
        private readonly IEmailService _emailService;

        public ClientesController(DistribuidoraDoChinesContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [HttpPut("PasswordRecovery")]
        public async Task<ActionResult> PasswordRecovery([FromQuery] string email)
        {
            try
            {
                var cliente = await _context.Clientes
                    .Where(c => c.Email == email)
                    .FirstOrDefaultAsync();

                if (cliente == null) return BadRequest("Esse e-mail não está cadastrado!");

                var newPassword = SecurePasswordHasher.GenerateRandomString(8);

                cliente.Senha = SecurePasswordHasher.Hash(newPassword);

                var html = System.IO.File
                    .ReadAllText(@"wwwroot/html/password-recovery.html")
                    .Replace("{{Senha}}", newPassword);

                await PutClientes(cliente.Id, cliente);

                _emailService.Send(new EmailMessage
                {
                    Content = html,
                    Subject = "Recuperação de senha",
                    ToAddresses = new List<EmailAddress>
                        {new EmailAddress {Name = cliente.Nome, Address = cliente.Email}}
                });

                return Ok("Email enviado.");
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

        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> PutClientes(uint id, [FromBody] Clientes cliente)
        {
            try
            {
                if (id != cliente.Id) return BadRequest();

                _context.Entry(cliente).State = EntityState.Modified;

                return Convert.ToBoolean(await _context.SaveChangesAsync());
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientesExists(id)) return NotFound();

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

        [HttpPost]
        public async Task<ActionResult<Clientes>> PostClientes([FromBody] Clientes cliente)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var usuarioExistente = await _context.Clientes
                    .Where(c => c.Email == cliente.Email)
                    .SingleOrDefaultAsync();

                if (usuarioExistente != null) return BadRequest("E-mail já cadastrado!");

                cliente.Senha = SecurePasswordHasher.Hash(cliente.Senha);

                _context.Clientes.Add(cliente);
                
                await _context.SaveChangesAsync();

                return Ok(await GetAsyncCliente(cliente.Id));
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
        
        private bool ClientesExists(uint id)
        {
            return _context.Clientes.Any(e => e.Id == id);
        }

        private async Task<Clientes> GetAsyncCliente(uint id)
        {
            return await _context.Clientes
                .Include(c => c.Enderecos)
                .Include(c => c.Telefones)
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();
        }
    }
}