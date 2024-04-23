using BeautifyMe.BeautifyMeDbModels;
using BeautifyMe.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BeautifyMe.Controllers
{
    public class CartController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmailService _smsService;
        private readonly IOTPGenerator _otpGenerator;
        private readonly ICartService _cartService;
        public CartController(ILogger<HomeController> logger, IEmailService smsService, IOTPGenerator otpGenerator, ICartService cartService)
        {
            _logger = logger;
            _smsService = smsService;
            _otpGenerator = otpGenerator;
            _cartService = cartService;
        }

        [HttpPost]
        public IActionResult AddToCart(int sizeId, int quantity, int productId)
        {

            CartProductMap cartProductMap = new CartProductMap()
            {
                ProductId = productId,
                SizeId = sizeId,
                Quatity = quantity,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now
            };
            List<CartProductMap> cartProductMaps = new List<CartProductMap>();
            cartProductMaps.Add(cartProductMap);
            Cart cart = new Cart()
            {
                UserEmailId = User.Identity.Name,
                CartProductMaps = cartProductMaps,
                IsActive = true
            };
            _cartService.AddToCart(cart);
            return Json(new { success = true, message = "Item added to the cart" });
        }

        public IActionResult RemoveItemFromCart(int cartProductMapId)
        {
            _cartService.RemoveFromCart(cartProductMapId);
            return Redirect("/cart/cart");
        }
    }
}
