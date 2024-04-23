using BeautifyMe.BeautifyMeDbModels;
using BeautifyMe.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace BeautifyMe.Services
{
    public class ProductService : IProductService
    {
        private readonly BeautifyMeContext _context;
        public ProductService(BeautifyMeContext context)
        {
            _context = context;
        }
        public List<Brand> GetBrands()
        {
            return _context.Brands.ToList();
        }

        public List<Category> GetCategories()
        {
            return _context.Categories.ToList();
        }

        public List<Size> GetSizes()
        {
            return _context.Sizes.ToList();
        }



        public List<Inventory> GetInventoryItems(
        string searchText = null,
        int? brandId = null,
        int? categoryId = null,
        int? sizeId = null)
        {
            IQueryable<Inventory> query = _context.Inventories.Include(i => i.Product).Include(i => i.Size);

            if (!string.IsNullOrEmpty(searchText))
            {
                query = query.Where(i => i.Product.ProductName.Contains(searchText) || i.Product.Brand.BrandName.Contains(searchText) || i.Product.Category.CategoryName.Contains(searchText));
            }

            if (brandId.HasValue)
            {
                query = query.Where(i => i.Product.BrandId == brandId);
            }

            if (categoryId.HasValue)
            {
                query = query.Where(i => i.Product.CategoryId == categoryId);
            }

            if (sizeId.HasValue)
            {
                query = query.Where(i => i.SizeId == sizeId);
            }

            return query.ToList();
        }
    }
}
