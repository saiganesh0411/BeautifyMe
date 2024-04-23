using BeautifyMe.BeautifyMeDbModels;
using BeautifyMe.Models;
using BeautifyMe.Services.Interfaces;
using MailKit.Search;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using System.Diagnostics;

namespace BeautifyMe.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmailService _smsService;
        private readonly IOTPGenerator _otpGenerator;
        private readonly IProductService _productService;
        public HomeController(ILogger<HomeController> logger, IEmailService smsService, IOTPGenerator otpGenerator, IProductService productService)
        {
            _logger = logger;
            _smsService = smsService;
            _otpGenerator = otpGenerator;
            _productService = productService;
        }

        public IActionResult Index()
        {

            HomeViewModel viewModel = new HomeViewModel();
            viewModel.Brands = _productService.GetBrands();
            viewModel.Categories = _productService.GetCategories();
            viewModel.Sizes = _productService.GetSizes();
            viewModel.Cards = CreateCardsFromInventory(_productService.GetInventoryItems());
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Search(string searchText) 
        {
            List<Inventory> inventory = _productService.GetInventoryItems(searchText);
            return PartialView("_Card", CreateCardsFromInventory(inventory));
        }

        public IActionResult Filter(int? sizeId, int? brandId, int? categoryId)
        {
            List<Inventory> inventory = _productService.GetInventoryItems(null, brandId, categoryId, sizeId);
            return PartialView("_Card", CreateCardsFromInventory(inventory));
        }

        [HttpGet]
        public IActionResult Suggestions(string term)
        {
            List<Inventory> inventory = _productService.GetInventoryItems(term);           
            // Filter and return matching product names as JSON
            var results = inventory
                .Select(i => new { label = i.Product.ProductName, value = i.Product.ProductName })
                .ToList();

            return Json(results.Distinct());
        }

        public List<CardViewModel> CreateCardsFromInventory(List<Inventory> inventory)
        {
            List<CardViewModel> uniqueInventory = new List<CardViewModel>();
            foreach (var item in inventory.ToList())
            {
                
                if (!uniqueInventory.Any(inven => inven.Product.ProductId == item.ProductId))
                {
                    CardViewModel cardViewModel = new CardViewModel()
                    {
                        InventoryId = item.InventoryId,
                        IsActive = item.IsActive,
                        Price = item.Price,
                        Product = item.Product,
                        ProductId = item.ProductId,
                        QuantityAvailable = item.QuantityAvailable,
                        SizeId = item.SizeId
                    };
                    cardViewModel.AvailableSizes.Add(item.Size);
                    uniqueInventory.Add(cardViewModel);
                }
                else
                {
                    var currentInventory = uniqueInventory.FirstOrDefault(inven => inven.Product.ProductId == item.ProductId);
                    if (currentInventory != null)
                    {
                        currentInventory.AvailableSizes.Add(item.Size);
                    }
                }
            }
            return uniqueInventory;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}