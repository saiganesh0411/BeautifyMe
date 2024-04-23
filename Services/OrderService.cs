using BeautifyMe.BeautifyMeDbModels;
using BeautifyMe.Services.Interfaces;
using MailKit.Search;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore;

namespace BeautifyMe.Services
{
    public class OrderService: IOrderService
    {
        private readonly BeautifyMeContext _context;
        private readonly ICartService _cartService;

        public OrderService(BeautifyMeContext context, ICartService cartService)
        {
            _context = context;
            _cartService = cartService; 
        }

        public List<Order> GetOrders(string emailId)
        {
            var orders = _context.Orders.Include(o => o.OrderProductMaps)
                .ThenInclude(opm => opm.Product).Include(o => o.Card).Where(o => o.EmailId == emailId && o.IsVerified == true).OrderByDescending(o=>o.OrderDate).ToList();
            return orders;
        }

        public Order AddOrder(Order order)
        {

            // get all items from cart
            var cart = _context.Carts.Include(c => c.CartProductMaps).ThenInclude(cpm => cpm.Product)
                .Include(c => c.CartProductMaps).ThenInclude(cpm => cpm.Size)
                .Include(c => c.CartProductMaps).ThenInclude(c => c.Product.Inventories)
                .FirstOrDefault(cart => cart.UserEmailId == order.EmailId && (cart.IsActive ?? false == true));

            decimal totalCost = 0;
            foreach (var item in cart.CartProductMaps)
            {
                var productId = item.ProductId;
                var size = item.Size.SizeId;
                var inventory = item.Product.Inventories.FirstOrDefault(s => s.ProductId == productId && s.SizeId == size);
                totalCost = totalCost + (inventory.Price ?? 0) * item.Quatity ?? 0;
                OrderProductMap orderProductMap = new OrderProductMap()
                {
                    ProductId = productId,
                    SizeId = size,
                    Quantity = item.Quatity,
                    Price = inventory.Price,
                    CreatedOn = DateTime.Now,
                };
                order.OrderProductMaps.Add(orderProductMap); 
            }
            order.TotalOrderCost = totalCost;
            _context.Orders.Add(order);     
            _context.SaveChanges();
            return order;
        }
        public void VerfiyOrder(string orderId)
        {
            var order = _context.Orders.FirstOrDefault(order => order.OrderId == Convert.ToInt32(orderId));
            if (order != null)
            {
                order.IsVerified = true;
                _context.Orders.Update(order);
                var cart = _context.Carts.Include(c => c.CartProductMaps).ThenInclude(cpm => cpm.Product)
                .Include(c => c.CartProductMaps).ThenInclude(cpm => cpm.Size)
                .Include(c => c.CartProductMaps).ThenInclude(c => c.Product.Inventories)
                .FirstOrDefault(cart => cart.UserEmailId == order.EmailId && (cart.IsActive ?? false == true));
                _cartService.DeleteCart(cart);
                _context.SaveChanges();
            }
        }
    }
}
