using System;
using System.Collections.Generic;

namespace BeautifyMe.BeautifyMeDbModels
{
    public partial class Product
    {
        public Product()
        {
            CartProductMaps = new HashSet<CartProductMap>();
            Inventories = new HashSet<Inventory>();
            OrderProductMaps = new HashSet<OrderProductMap>();
        }

        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public int? BrandId { get; set; }
        public int? CategoryId { get; set; }
        public string ImagePath { get; set; }
        public virtual Brand? Brand { get; set; }
        public virtual Category? Category { get; set; }
        public virtual ICollection<CartProductMap> CartProductMaps { get; set; }
        public virtual ICollection<Inventory> Inventories { get; set; }
        public virtual ICollection<OrderProductMap> OrderProductMaps { get; set; }
    }
}
