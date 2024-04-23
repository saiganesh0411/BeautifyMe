
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using BeautifyMe.Areas.Identity.Data;
using BeautifyMe.Areas.Identity.Pages.Account.Manage;
using BeautifyMe.BeautifyMeDbModels;
using BeautifyMe.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace BeautifyMe.Areas.Identity.Pages.Account.Manage
{
    public class ProfileModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<ChangePasswordModel> _logger;
        public ProfileModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<ChangePasswordModel> logger
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }
        [BindProperty]
        public InputModel Input { get; set; }
        [TempData]
        public string StatusMessage { get; set; }
        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            [MaxLength(100)]
            public string Email { get; set; }
            [Required]
            [DataType(DataType.PhoneNumber)]
            [Display(Name = "USA Phone Number")]
            public string PhoneNumber { get; set; }
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "First Name")]
            [MaxLength(20)]
            public string FirstName { get; set; }
            [Required]
            [DataType(DataType.Text)]
            [MaxLength(20)]
            [Display(Name = "Last   Name")]
            public string LastName { get; set; }
            [Required]
            [DataType(DataType.Text)]
            [MaxLength(200)]
            [Display(Name = "Expereince")]
            public string Experience { get; set; }
            [Required]
            [DataType(DataType.Text)]
            [MaxLength(500)]
            [Display(Name = "Certifications")]
            public string Certifications { get; set; }
            [Required]
            [DataType(DataType.Text)]
            [MaxLength(8000)]
            [Display(Name = "Description")]
            public string Description { get; set; }
            public int InstructorId { get; set; }
            public string UserId { get; set; }

        }

        public async void OnGetAsync()
        {
            
        }

        public void OnPostAsync()
        {
            
        }
    }
}
