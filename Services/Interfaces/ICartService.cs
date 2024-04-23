using BeautifyMe.BeautifyMeDbModels;

namespace BeautifyMe.Services.Interfaces
{
    public interface ICartService
    {
        public void AddToCart(Cart cart);
        public Cart GetCartForUser(string emailId);
        public void DeleteCart(Cart cart);
        public void RemoveFromCart(int cartProductMapId);
        public decimal GetCartTotalPriceForUser(string emailId);
    }
}
