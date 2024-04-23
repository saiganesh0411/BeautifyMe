using BeautifyMe.BeautifyMeDbModels;
using BeautifyMe.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace BeautifyMe.Areas.Address.Pages
{
    public class AddressModel : PageModel
    {
        private readonly IAddressService _addressService;
        public AddressModel(IAddressService addressService)
        {
            _addressService = addressService;
        }
        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel
        {
            [Required]
            [Display(Name = "Frist Name")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Required]
            [Display(Name = "Address Line 1")]
            public string AddressLine1 { get; set; }

            [Display(Name = "Address Line 2")]
            public string AddressLine2 { get; set; }
            [Required]
            [Display(Name = "State")]
            public string State { get; set; }
            [Required]
            [Display(Name = "Zipcode")]
            public string Zipcode { get; set; }

        }
        public void OnGet()
        {
        }
        public IActionResult OnPost() {
            BeautifyMe.BeautifyMeDbModels.Address address = new BeautifyMeDbModels.Address()
            {
                FirstName = Input.FirstName,
                LastName = Input.LastName,
                AddressLine1 = Input.AddressLine1,
                AddressLine2 = Input.AddressLine2,
                State = Input.State,
                ZipCode = Input.Zipcode,
                UserEmailId = User.Identity.Name,
                IsActive = true,
                Country = "US",
                CreatedOn= DateTime.Now
            };
            _addressService.AddAddress(address);
            return Redirect("/Payment/Payment");
        }
    }
}
