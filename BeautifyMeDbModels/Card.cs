using System;
using System.Collections.Generic;

namespace BeautifyMe.BeautifyMeDbModels
{
    public partial class Card
    {
        public Card()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string? EmailId { get; set; }
        public string MaskedCreditCard { get; set; } = null!;
        public string EncryptedCreditCard { get; set; } = null!;
        public string EncryptedCardHolderName { get; set; } = null!;
        public string EncryptedSecurityCode { get; set; } = null!;
        public string EncryptedExpiry { get; set; } = null!;
        public bool? IsAcive { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
