using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable NotAccessedField.Local
// ReSharper disable UnusedMember.Global

namespace DistribuidoraDoChines.Mvc.Areas.Identity.Pages.Account.Manage
{
    public class PersonalDataModel : PageModel
    {
        private readonly ILogger<PersonalDataModel> _logger;
        private readonly UserManager<IdentityUser> _userManager;

        public PersonalDataModel(
            UserManager<IdentityUser> userManager,
            ILogger<PersonalDataModel> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound(
                    $"Não foi possível carregar as informação do usuário com ID '{_userManager.GetUserId(User)}'.");

            return Page();
        }
    }
}