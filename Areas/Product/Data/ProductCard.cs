using BeautifyMe.BeautifyMeDbModels;

namespace BeautifyMe.Areas.Product.Data
{
    public class ProductCard
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public List<Size> Sizes { get; set; } = new List<Size>();
        public decimal Price { get; set; }
        public int QuantityAvailable { get; set; }
    }
}
