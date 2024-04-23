#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using BeautifyMe.Areas.Identity.Data;
using BeautifyMe.Services.Interfaces;
using BeautifyMe.BeautifyMeDbModels;
using BeautifyMe.Areas.Payment.Data;

namespace BeautifyMe.Areas.PaymentNewCard.Pages
{
    public class PaymentNewCardModel : PageModel
    {

        private readonly IPaymentService _paymentService;
        private readonly IAddressService _addressService;
        private readonly IEncryptionService _encryptionService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOrderService _orderService;
        private readonly ICartService _cartService;

        public PaymentNewCardModel(IPaymentService paymentService, IAddressService addressService, IEncryptionService encryptionService, UserManager<ApplicationUser> userManager, 
            IOrderService orderService, ICartService cartService)
        {
            _paymentService = paymentService;
            _addressService = addressService;
            _encryptionService = encryptionService;
            _userManager = userManager;
            _orderService = orderService;
            _cartService = cartService;
        }
        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "Card holder's name")]
            [Required]
            public string CardHolderName { get; set; }

            [BindProperty]
            [Display(Name = "Credit Card Number")]
            [Required]
            public string CardNumber { get; set; }

            [BindProperty]
            [Display(Name = "Expiration Date (MM/YY)")]
            [Required]
            public string ExpirationDate { get; set; }

            [BindProperty]
            [Required]
            [Display(Name = "Security code")]
            public string CVV { get; set; }

            public bool SaveCardInfo { get; set; }
            [Required]
            public int AddressId { get; set; }
            public List<BeautifyMeDbModels.Address> UserAddresses { get; set; } = new List<BeautifyMeDbModels.Address>();
            public decimal TotalPrice { get; set; }
        }

        public async Task OnGet()
        {
            Input = new InputModel();
            Input.UserAddresses = _addressService.GetAddresses(User.Identity.Name);
            Input.TotalPrice = _cartService.GetCartTotalPriceForUser(User.Identity.Name);
        }

        public async Task<IActionResult> OnPost()
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            var encryptionKey = user.EncryptionKey;
            var encryptionKeyForJS = user.SessionId;
            DecryptInputModelForJavaScript(encryptionKeyForJS);
            if (ModelState.IsValid)
            {
                Card card;
                if (Input.SaveCardInfo)
                {
                    card = new Card()
                    {
                        EncryptedCardHolderName = _encryptionService.Encrypt(Input.CardHolderName, encryptionKey),
                        EncryptedCreditCard = _encryptionService.Encrypt(Input.CardNumber, encryptionKey),
                        EncryptedExpiry = _encryptionService.Encrypt(Input.ExpirationDate, encryptionKey),
                        EncryptedSecurityCode = _encryptionService.Encrypt(Input.CVV, encryptionKey),
                        MaskedCreditCard = _encryptionService.MaskCreditCard(Input.CardNumber),
                        EmailId = User.Identity.Name,
                        IsAcive = true,
                    };

                }
                else
                {
                    // if user doesnot want to save card just mask card info and store as inactive
                    card = new Card()
                    {
                        MaskedCreditCard = _encryptionService.MaskCreditCard(Input.CardNumber),
                        EncryptedCardHolderName = string.Empty,
                        EncryptedCreditCard = string.Empty,
                        EncryptedExpiry = string.Empty,
                        EncryptedSecurityCode = string.Empty,
                        EmailId = User.Identity.Name,
                        IsAcive = false,
                    };

                }
                card = _paymentService.AddCard(card);
                Order order = new Order()
                {
                    AddressId = Input.AddressId,
                    CardId = card.Id,
                    EmailId = User.Identity.Name,
                    OrderDate = DateTime.Now,
                    TotalOrderCost = 0,
                };
                order = _orderService.AddOrder(order);
                return Redirect("/OTP/OTP?orderId=" + order?.OrderId.ToString());
            }
            return Page(); ;
        }
        private void DecryptInputModelForJavaScript(Guid encryptionKey)
        {
            Input.CardHolderName = _encryptionService.DecryptForJavaScript(encryptionKey, Input.CardHolderName);
            Input.CardNumber = _encryptionService.DecryptForJavaScript(encryptionKey, Input.CardNumber);
            Input.CVV = _encryptionService.DecryptForJavaScript(encryptionKey, Input.CVV);
            Input.ExpirationDate = _encryptionService.DecryptForJavaScript(encryptionKey, Input.ExpirationDate);
        }
    }
}
