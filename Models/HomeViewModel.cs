using BeautifyMe.BeautifyMeDbModels;

namespace BeautifyMe.Models
{
    public class HomeViewModel
    {
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<Brand> Brands { get; set; } = new List<Brand>();
        public List<Size> Sizes { get; set; } = new List<Size>();   

        //public List<Inventory> Cards { get; set; }  = new List<Inventory> { };
        public List<CardViewModel> Cards { get; set; }= new List<CardViewModel>();
    }
}
