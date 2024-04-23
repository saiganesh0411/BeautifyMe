using System;
using System.Collections.Generic;

namespace BeautifyMe.BeautifyMeDbModels
{
    public partial class Size
    {
        public Size()
        {
            CartProductMaps = new HashSet<CartProductMap>();
            Inventories = new HashSet<Inventory>();
            OrderProductMaps = new HashSet<OrderProductMap>();
        }

        public int SizeId { get; set; }
        public string? SizeName { get; set; }
        public string? SizeDescription { get; set; }

        public virtual ICollection<CartProductMap> CartProductMaps { get; set; }
        public virtual ICollection<Inventory> Inventories { get; set; }
        public virtual ICollection<OrderProductMap> OrderProductMaps { get; set; }
    }
}
