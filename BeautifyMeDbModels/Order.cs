using System;
using System.Collections.Generic;

namespace BeautifyMe.BeautifyMeDbModels
{
    public partial class Order
    {
        public Order()
        {
            OrderProductMaps = new HashSet<OrderProductMap>();
        }

        public int OrderId { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? EmailId { get; set; }
        public int? AddressId { get; set; }
        public decimal? TotalOrderCost { get; set; }
        public int? CardId { get; set; }
        public bool? IsVerified { get; set; } = false;

        public virtual Address? Address { get; set; }
        public virtual Card? Card { get; set; }
        public virtual ICollection<OrderProductMap> OrderProductMaps { get; set; }
    }
}
