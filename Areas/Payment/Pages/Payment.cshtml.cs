#nullable disable

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BeautifyMe.Areas.Identity.Data;
using BeautifyMe.Services.Interfaces;
using BeautifyMe.BeautifyMeDbModels;
using BeautifyMe.Areas.Payment.Data;
using BeautifyMe.Services;

namespace BeautifyMe.Areas.Payment.Pages
{
    public class PaymentModel : PageModel
    {

        private readonly IPaymentService _paymentService;
        private readonly IAddressService _addressService;
        private readonly IEncryptionService _encryptionService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOrderService _orderService;
        private readonly ICartService _cartService;

        public PaymentModel(IPaymentService paymentService, IAddressService addressService, IEncryptionService encryptionService, UserManager<ApplicationUser> userManager, 
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
            public bool SaveCardInfo { get; set; }
           
            public string SelectedCardId { get; set; }
            [BindProperty]
            [Required]
            [Display(Name = "Security code for selected card")]
            public string SelectedCardSecurity { get; set; }
            public List<CardInfo> SavedCards { get; set; } = new List<CardInfo> { };
            public List<BeautifyMeDbModels.Address> UserAddresses { get; set; } = new List<BeautifyMeDbModels.Address>();
            public int AddressId { get; set; }
            public decimal TotalPrice { get; set; }
        }

        public async Task OnGet()
        {
            
            var savedCards = _paymentService.GetSavedCards(User.Identity.Name);
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            var encryptionKey = user.EncryptionKey;
            var encryptionKeyForJS = user.SessionId;
            Input = new InputModel();
            Input.UserAddresses = _addressService.GetAddresses(User.Identity.Name);
            Input.TotalPrice = _cartService.GetCartTotalPriceForUser(User.Identity.Name);
            foreach (var item in savedCards)
            {
                CardInfo card = new CardInfo()
                {
                    CardHolderName = _encryptionService.Decrypt(item.EncryptedCardHolderName, encryptionKey),
                    CardId = _encryptionService.EncryptForJavaScript(encryptionKeyForJS, item.Id.ToString()),
                    MaskedCardNumber = item.MaskedCreditCard,
                    Expiry = _encryptionService.Decrypt(item.EncryptedExpiry, encryptionKey)
                };
                Input.SavedCards.Add(card);
            }
        }

        private async Task AddAddressAndSavedCards(InputModel input)
        {
            var savedCards = _paymentService.GetSavedCards(User.Identity.Name);
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            var encryptionKey = user.EncryptionKey;
            var encryptionKeyForJS = user.SessionId;
            Input.UserAddresses = _addressService.GetAddresses(User.Identity.Name);
            foreach (var item in savedCards)
            {
                CardInfo card = new CardInfo()
                {
                    CardHolderName = _encryptionService.Decrypt(item.EncryptedCardHolderName, encryptionKey),
                    CardId = _encryptionService.EncryptForJavaScript(encryptionKeyForJS, item.Id.ToString()),
                    MaskedCardNumber = item.MaskedCreditCard,
                    Expiry = _encryptionService.Decrypt(item.EncryptedExpiry, encryptionKey)
                };
                Input.SavedCards.Add(card);
            }
        }

        public async Task<IActionResult> OnPost()
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            var encryptionKey = user.EncryptionKey;
            var encryptionKeyForJS = user.SessionId;
            DecryptInputModelForJavaScript(encryptionKeyForJS);
            Order order = null;
            if (Input.SelectedCardId != string.Empty)
            {
                if(!_paymentService.ValidateCard(Convert.ToInt32(Input.SelectedCardId), _encryptionService.Encrypt(Input.SelectedCardSecurity, encryptionKey)))
                {
                    ModelState.AddModelError("Input.SelectedCardSecurity", "Security code doesnot match");
                }
                if(ModelState.IsValid)
                {
                    // Make payment and redirect to orders screen
                    order = new Order()
                    {
                        AddressId = Input.AddressId,
                        CardId = Convert.ToInt32(Input.SelectedCardId),
                        EmailId = User.Identity.Name,
                        OrderDate = DateTime.Now,
                        TotalOrderCost = 0,
                    };
                    order = _orderService.AddOrder(order);
                    return Redirect("/OTP/OTP?orderId="+order?.OrderId.ToString());
                }
                else
                {
                    await AddAddressAndSavedCards(Input);
                    return Redirect("/Payment/Payment?isNotValid=true");
                }
            }
            return RedirectToPage("orders/orders");
        }
        private void DecryptInputModelForJavaScript(Guid encryptionKey)
        {
            Input.SelectedCardId = _encryptionService.DecryptForJavaScript(encryptionKey, Input.SelectedCardId);
            Input.SelectedCardSecurity = _encryptionService.DecryptForJavaScript(encryptionKey, Input.SelectedCardSecurity);           
        }
    }
}
