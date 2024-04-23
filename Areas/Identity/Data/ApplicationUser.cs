using Microsoft.AspNetCore.Identity;

namespace BeautifyMe.Areas.Identity.Data
{
    public class ApplicationUser: IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CountryCode { get; set; }
        public Guid SessionId { get; set; }
        public Guid EncryptionKey { get; set; }
    }
}
