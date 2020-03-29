using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedMember.Global

namespace DistribuidoraDoChines.Mvc.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordConfirmation : PageModel
    {
        public void OnGet()
        {
        }
    }
}