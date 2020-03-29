using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable UnusedMember.Global

namespace DistribuidoraDoChines.Mvc.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly ILogger<LoginModel> _logger;
        private readonly SignInManager<IdentityUser> _signInManager;

        public LoginModel(SignInManager<IdentityUser> signInManager, ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty] public InputModel Input { get; set; }

        private IList<AuthenticationScheme> ExternalLogins { get; set; }

        private string ReturnUrl { get; set; }

        [TempData] private string ErrorMessage { get; set; }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage)) ModelState.AddModelError(string.Empty, ErrorMessage);

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (!ModelState.IsValid) return Page();
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            var result =
                await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, true);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in.");
                return LocalRedirect(returnUrl);
            }

            if (result.RequiresTwoFactor)
                return RedirectToPage("./LoginWith2fa", new {ReturnUrl = returnUrl, Input.RememberMe});
            if (result.IsLockedOut)
            {
                _logger.LogWarning("User account locked out.");
                return RedirectToPage("./Lockout");
            }

            ModelState.AddModelError(string.Empty, "Login inválido.");
            return Page();

            // If we got this far, something failed, redisplay form
        }

        public class InputModel
        {
            [Required(ErrorMessage = "{0} é obrigatório")]
            [EmailAddress(ErrorMessage = "{0} deve ser um email válido")]
            public string Email { get; set; }

            [Required(ErrorMessage = "{0} é obrigatório")]
            [DataType(DataType.Password, ErrorMessage = "{0} deve ser um campo de senha válido")]
            public string Password { get; set; }

            [Display(Name = "Salvar credenciais?")]
            public bool RememberMe { get; set; }
        }
    }
}