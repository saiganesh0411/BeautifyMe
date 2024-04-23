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

namespace BeautifyMe.Areas.Cart.Pages.Cart
{
    public class CartModel : PageModel
    {
        private readonly ICartService _cartService;

        public CartModel(ICartService cartService)
        {
            _cartService = cartService;
        }
        [BindProperty]
        public InputModel Input { get; set; }
        
        public class InputModel
        {
            public int cartId { get; set; }
            public List<BeautifyMe.BeautifyMeDbModels.CartProductMap> CartItems { get; set; } = new List<BeautifyMeDbModels.CartProductMap>();
            public List<decimal> Prices { get; set; }
        }

        public async void OnGetAsync(string email = null, string returnUrl = null)
        {
            var cart = _cartService.GetCartForUser(User.Identity.Name);
            List<decimal> prices = new List<decimal>();
            foreach(var item in cart.CartProductMaps)
            {
                var productId = item.ProductId;
                var size = item.Size.SizeId;
                var inventory = item.Product.Inventories.FirstOrDefault(s => s.ProductId == productId && s.SizeId == size);
                prices.Add((inventory.Price ?? 0) * item.Quatity ?? 0);
            }
            Input = new InputModel
            {
                CartItems = cart.CartProductMaps.ToList(),
                cartId = cart.CartId,
                Prices = prices
            };  
        }

        public async void OnPostAsync(string returnUrl = null)
        {
            
        }
    }
}
