using BeautifyMe.BeautifyMeDbModels;

namespace BeautifyMe.Services.Interfaces
{
    public interface IAddressService
    {
        public void AddAddress(Address address);
        public List<Address> GetAddresses(string userEmailId);
    }
}
