using BeautifyMe.BeautifyMeDbModels;
using BeautifyMe.Services.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace BeautifyMe.Services
{
    public class CartService : ICartService
    {
        private readonly BeautifyMeContext _context;
        public CartService(BeautifyMeContext context)
        {
            _context = context;
        }

        public void AddToCart(Cart cart)
        {

            var existingCart = _context.Carts
                            .Include(c => c.CartProductMaps)
                            .FirstOrDefault(c => c.UserEmailId == cart.UserEmailId && (c.IsActive ?? false == true));

            if (existingCart != null)
            {
                foreach (var cartProductMap in cart.CartProductMaps)
                {
                    existingCart.CartProductMaps.Add(cartProductMap);
                }
            }
            else
            {                
                _context.Carts.Add(cart);
            }
            _context.SaveChanges();
        }
        public Cart GetCartForUser(string emailId)
        {
            var cart = _context.Carts.Include(c => c.CartProductMaps).ThenInclude(cpm => cpm.Product)
                .Include(c => c.CartProductMaps).ThenInclude(cpm => cpm.Size) 
                .Include(c => c.CartProductMaps).ThenInclude(c => c.Product.Inventories)
                .FirstOrDefault(cart => cart.UserEmailId == emailId && (cart.IsActive ?? false == true));

            return cart ?? new Cart();
        }

        public decimal GetCartTotalPriceForUser(string emailId)
        {
            var cart = _context.Carts.Include(c => c.CartProductMaps).ThenInclude(cpm => cpm.Product)
                .Include(c => c.CartProductMaps).ThenInclude(cpm => cpm.Size)
                .Include(c => c.CartProductMaps).ThenInclude(c => c.Product.Inventories)
                .FirstOrDefault(cart => cart.UserEmailId == emailId && (cart.IsActive ?? false == true));

            decimal totalCost = 0;
            foreach (var item in cart.CartProductMaps)
            {
                var productId = item.ProductId;
                var size = item.Size.SizeId;
                var inventory = item.Product.Inventories.FirstOrDefault(s => s.ProductId == productId && s.SizeId == size);
                totalCost = totalCost + (inventory.Price ?? 0) * item.Quatity ?? 0;
            }
            return totalCost;
        }
        public void DeleteCart(Cart cart)
        {
            _context.Carts.Remove(cart);
            _context.SaveChanges();
        }
        public void RemoveFromCart(int cartProductMapId)
        {
            var cartId = _context.CartProductMaps.FirstOrDefault(s=>s.CartProductMapId == cartProductMapId).CartId;    
            var cart = _context.Carts
                                .Include(c => c.CartProductMaps)
                                .FirstOrDefault(c => c.CartId == cartId);

            if (cart != null)
            {
                var cartProductMap = cart.CartProductMaps.FirstOrDefault(cp => cp.CartProductMapId == cartProductMapId);

                if (cartProductMap != null)
                {
                    if (cart.CartProductMaps.Count > 1)
                    {
                        cart.CartProductMaps.Remove(cartProductMap);
                    }
                    else
                    {
                        _context.Carts.Remove(cart);
                    }

                    _context.SaveChanges();
                }
            }
        }

    }
}
