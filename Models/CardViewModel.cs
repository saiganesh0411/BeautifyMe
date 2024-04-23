using BeautifyMe.BeautifyMeDbModels;
using Twilio.Rest.Video.V1.Room.Participant;

namespace BeautifyMe.Models
{
    public class CardViewModel
    {
        public int InventoryId { get; set; }
        public int? ProductId { get; set; }
        public int? SizeId { get; set; }
        public int? QuantityAvailable { get; set; }
        public decimal? Price { get; set; }
        public bool? IsActive { get; set; }

        public virtual Product? Product { get; set; }
        public virtual List<Size> AvailableSizes { get; set; } = new List<Size>();
    }
}
