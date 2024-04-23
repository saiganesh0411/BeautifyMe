using System;
using System.Collections.Generic;

namespace BeautifyMe.BeautifyMeDbModels
{
    public partial class Inventory
    {
        public int InventoryId { get; set; }
        public int? ProductId { get; set; }
        public int? SizeId { get; set; }
        public int? QuantityAvailable { get; set; }
        public decimal? Price { get; set; }
        public bool? IsActive { get; set; }

        public virtual Product? Product { get; set; }
        public virtual Size? Size { get; set; }
    }
}
