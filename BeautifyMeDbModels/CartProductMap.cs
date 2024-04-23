using System;
using System.Collections.Generic;

namespace BeautifyMe.BeautifyMeDbModels
{
    public partial class CartProductMap
    {
        public int CartProductMapId { get; set; }
        public int? CartId { get; set; }
        public int? ProductId { get; set; }
        public int? SizeId { get; set; }
        public int? Quatity { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual Cart? Cart { get; set; }
        public virtual Product? Product { get; set; }
        public virtual Size? Size { get; set; }
    }
}
