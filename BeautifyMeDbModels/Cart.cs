using System;
using System.Collections.Generic;

namespace BeautifyMe.BeautifyMeDbModels
{
    public partial class Cart
    {
        public Cart()
        {
            CartProductMaps = new HashSet<CartProductMap>();
        }

        public int CartId { get; set; }
        public string? UserEmailId { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<CartProductMap> CartProductMaps { get; set; }
    }
}
