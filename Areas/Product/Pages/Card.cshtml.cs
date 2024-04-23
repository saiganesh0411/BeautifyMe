#nullable disable
using BeautifyMe.Areas.Product.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace BeautifyMe.Areas.Product.Pages.Card
{
    public class CardModel : PageModel
    {


        public CardModel()
        {
        }
        [BindProperty]
        public InputModel Input { get; set; }
        
        public class InputModel
        {
            public ProductCard ProductCard { get; set; }
        }

        public async void OnGetAsync(string email = null, string returnUrl = null)
        {
            
        }

        public async void OnPostAsync(string returnUrl = null)
        {
            
        }
    }
}
