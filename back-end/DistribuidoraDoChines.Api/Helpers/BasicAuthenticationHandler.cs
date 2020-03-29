using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using DistribuidoraDoChines.Api.Data.Context;
using DistribuidoraDoChines.Commons.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

// ReSharper disable ClassNeverInstantiated.Global

namespace DistribuidoraDoChines.Api.Helpers
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly DistribuidoraDoChinesContext _context;
        private Usuarios _usuario;

        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            DistribuidoraDoChinesContext context)
            : base(options, logger, encoder, clock)
        {
            _context = context;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Cabeçalho Authorization Ausente");

            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] {':'}, 2);
                var username = credentials[0];
                var password = credentials[1];

                _usuario = await _context.Usuarios
                    .Where(u => u.Usuario == username && u.Senha == password)
                    .FirstOrDefaultAsync();
            }
            catch
            {
                return AuthenticateResult.Fail("Cabeçalho Authorization Inválido");
            }

            if (_usuario == null)
                return AuthenticateResult.Fail("Usuário ou Senha Inválido");

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, _usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, _usuario.Usuario)
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}