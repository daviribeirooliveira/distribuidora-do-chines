using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedMember.Global

namespace DistribuidoraDoChines.Mvc.Areas.Identity.Pages.Account
{
    [Authorize]
    public class RegisterModel : PageModel
    {
        // private readonly IEmailSender _emailSender;
        private readonly ILogger<RegisterModel> _logger;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger
            // ,IEmailSender emailSender
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            // _emailSender = emailSender;
        }

        [BindProperty] public InputModel Input { get; set; }

        public string ReturnUrl { get; private set; }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            
            if (!ModelState.IsValid) return Page();
            var user = new IdentityUser {UserName = Input.Email, Email = Input.Email};
            var result = await _userManager.CreateAsync(user, Input.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");

                // var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                // var callbackUrl = Url.Page(
                //     "/Account/ConfirmEmail",
                //     null,
                //     new {userId = user.Id, code},
                //     Request.Scheme);

                //await _emailSender.SendEmailAsync(Input.Email, "Confirme seu email",
                //    $"Por favor confirme sua conta através <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>Clicando aqui</a>.");

                await _signInManager.SignInAsync(user, false);
                return LocalRedirect(returnUrl);
            }

            foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);

            // If we got this far, something failed, redisplay form
            return Page();
        }

        public class InputModel
        {
            [Required(ErrorMessage = "{0} é obrigatório")]
            [EmailAddress(ErrorMessage = "{0} deve ser um email válido")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "{0} é obrigatório")]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
                MinimumLength = 6)]
            [DataType(DataType.Password, ErrorMessage = "{0} deve ser um campo de senha válido")]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password, ErrorMessage = "{0} deve ser um campo de senha válido")]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }
    }
}