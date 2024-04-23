using System;
using System.Collections.Generic;

namespace BeautifyMe.BeautifyMeDbModels
{
    public partial class OrderProductMap
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public int? ProductId { get; set; }
        public int? SizeId { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public DateTime? CreatedOn { get; set; }

        public virtual Order? Order { get; set; }
        public virtual Product? Product { get; set; }
        public virtual Size? Size { get; set; }
    }
}
