using BeautifyMe.BeautifyMeDbModels;
using BeautifyMe.Services.Interfaces;

namespace BeautifyMe.Services
{
    public class AddressService : IAddressService
    {
        private readonly BeautifyMeContext _context;
        public AddressService(BeautifyMeContext context)
        {
            _context = context;
        }
        public void AddAddress(Address address)
        {
            _context.Addresses.Add(address);
            _context.SaveChanges();
        }

        public List<Address> GetAddresses(string userEmailId)
        {
            return _context.Addresses.Where(address => address.UserEmailId == userEmailId).ToList();   
        }
    }
}
