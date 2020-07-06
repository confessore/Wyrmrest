using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Wyrmrest.Web.Data;

namespace Wyrmrest.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        readonly AuthDbContext _auth;

        public ResetPasswordModel(
            UserManager<IdentityUser> userManager,
            AuthDbContext auth)
        {
            _userManager = userManager;
            _auth = auth;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            public string Code { get; set; }
        }

        public IActionResult OnGet(string code = null)
        {
            if (code == null)
            {
                return BadRequest("A code must be supplied for password reset.");
            }
            else
            {
                Input = new InputModel
                {
                    Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code))
                };
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToPage("./ResetPasswordConfirmation");
            }

            var result = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);
            if (result.Succeeded)
            {
                if (await _auth.account.AnyAsync(x => x.Username == user.UserName.ToUpper()))
                {
                    var tmp = await _auth.account.FirstOrDefaultAsync(x => x.Username == user.UserName.ToUpper());
                    tmp.SHA_Pass_Hash = await ComputeSHA1PassHashAsync(user.UserName, Input.Password);
                    tmp.SessionKey = string.Empty;
                    tmp.V = string.Empty;
                    tmp.S = string.Empty;
                    await _auth.SaveChangesAsync();
                }
                return RedirectToPage("./ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return Page();
        }

        Task<string> ComputeSHA1PassHashAsync(string username, string password)
        {
            var sha = new SHA1CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes($"{username.ToUpper()}:{password.ToUpper()}"));
            return Task.FromResult(string.Concat(sha.Select(x => x.ToString("x2"))));
        }
    }
}
