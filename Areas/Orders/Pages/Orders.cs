#nullable disable

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BeautifyMe.Areas.Identity.Data;
using BeautifyMe.Services.Interfaces;
using BeautifyMe.BeautifyMeDbModels;
using BeautifyMe.Areas.Payment.Data;

namespace BeautifyMe.Areas.Orders.Pages
{
    public class OrdersModel : PageModel
    {

        private readonly IPaymentService _paymentService;
        private readonly IAddressService _addressService;
        private readonly IEncryptionService _encryptionService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOrderService _orderService;


        public OrdersModel(IPaymentService paymentService, IAddressService addressService, IEncryptionService encryptionService, UserManager<ApplicationUser> userManager, IOrderService orderService)
        {
            _paymentService = paymentService;
            _addressService = addressService;
            _encryptionService = encryptionService;
            _userManager = userManager;
            _orderService = orderService;
        }
        [BindProperty]
        public InputModel Input { get; set; }
        [BindProperty]
        public List<Order> Orders { get; set; } = new List<Order>();
        
        public class InputModel
        {            
            
        }

        public async Task OnGet()
        {
            Orders = _orderService.GetOrders(User.Identity.Name);
            
        }

        public void OnPost()
        {
            
        }
    }
}
