#nullable disable

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BeautifyMe.Areas.Identity.Data;
using BeautifyMe.Services.Interfaces;
using BeautifyMe.BeautifyMeDbModels;
using BeautifyMe.Areas.Payment.Data;

namespace BeautifyMe.Areas.OTP.Pages
{
    public class OTPModel : PageModel
    {

        private readonly IOTPGenerator _otpGenerator;
        private readonly IOrderService _orderService;
        private readonly IEncryptionService _encryptionService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;

        public OTPModel(IOTPGenerator otpGenerator, IOrderService orderService, UserManager<ApplicationUser> userManager, IEncryptionService encryptionService, IEmailService emailService)
        {
            _otpGenerator = otpGenerator;
            _orderService = orderService;
            _userManager = userManager;
            _encryptionService = encryptionService;
            _emailService = emailService;
        }
        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            public string OTP { get; set; }
            public string OrderId { get; set; }
        }

        public async Task OnGet(string orderId)
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            string otp = _otpGenerator.GenerateOTP(User.Identity.Name);
            _emailService.SendEmail(User.Identity.Name, "Verification code for your recent order from BeautifyMe", "Use this one time password for confirming your recent order with BeautifyMe " + otp);
            Input = new InputModel()
            {
                OrderId = orderId,
            };
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                var isValidated = _otpGenerator.ValidateOTP(User.Identity.Name, Input.OTP);
                if (isValidated)
                {
                    _orderService.VerfiyOrder(Input.OrderId);
                    return Redirect("/Orders/Orders");
                }
                ModelState.AddModelError("Input.OTP", "Please enter OTP sent to your email");
            }
            return Page();
        }
    }
}
