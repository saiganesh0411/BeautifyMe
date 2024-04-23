
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using BeautifyMe.Areas.Identity.Data;
using BeautifyMe.Services;
using BeautifyMe.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace BeautifyMe.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterConfirmationModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _sender;
        private readonly IOTPGenerator _otpGenerator;
        private readonly IEncryptionService _encryptionService;
        public RegisterConfirmationModel(UserManager<ApplicationUser> userManager, IEmailSender sender, IOTPGenerator otpGenerator, IEncryptionService encryptionService)
        {
            _userManager = userManager;
            _sender = sender;
            _otpGenerator = otpGenerator;
            _encryptionService = encryptionService;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public string UserId { get; set; }
        public string OTP { get; set; }
        public bool UserValidated { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "One Time Password")]
            public string OTP { get; set; }
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string email)
        {
            if (email == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByEmailAsync(email);
            Input = new InputModel()
            {
                Email = email
            };

            if (user == null)
            {
                return NotFound($"Unable to load user with email '{email}'.");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var isValidated = _otpGenerator.ValidateOTP(Input.Email, Input.OTP);
                if (isValidated)
                {
                    var user = await _userManager.FindByEmailAsync(Input.Email);
                    user.EmailConfirmed = true;
                    user.PhoneNumberConfirmed = true;
                    await _userManager.UpdateAsync(user);
                    return RedirectToPage("Login");
                }
                ModelState.AddModelError("Input.OTP", "Please enter OTP sent to your email");
            }            
            return Page();
        }
    }
}
