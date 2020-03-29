using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedMember.Global

namespace DistribuidoraDoChines.Mvc.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public ResetPasswordModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty] public InputModel Input { get; set; }

        public IActionResult OnGet(string code = null)
        {
            if (code == null) return BadRequest("A code must be supplied for password reset.");

            Input = new InputModel
            {
                Code = code
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
                // Don't reveal that the user does not exist
                return RedirectToPage("./ResetPasswordConfirmation");

            var result = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);
            if (result.Succeeded) return RedirectToPage("./ResetPasswordConfirmation");

            foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
            return Page();
        }

        public class InputModel
        {
            [Required(ErrorMessage = "{0} é obrigatório")]
            [EmailAddress(ErrorMessage = "{0} deve ser um email válido")]
            public string Email { get; set; }

            [Required(ErrorMessage = "{0} é obrigatório")]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
                MinimumLength = 6)]
            [DataType(DataType.Password, ErrorMessage = "{0} deve ser um campo de senha válido")]
            public string Password { get; set; }

            [DataType(DataType.Password, ErrorMessage = "{0} deve ser um campo de senha válido")]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            public string Code { get; set; }
        }
    }
}