using BeautifyMe.BeautifyMeDbModels;
using Twilio.Rest.Video.V1.Room.Participant;

namespace BeautifyMe.Services.Interfaces
{
    public interface IOrderService
    {
        public List<Order> GetOrders(string emailId);
        public Order AddOrder(Order order);
        public void VerfiyOrder(string orderId);
    }
}
