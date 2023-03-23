using BankManagerApp.NorthwindData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BankManagerApp.Pages
{
    public class IndexModel : PageModel
    {
        public IndexModel(NorthwindInclIdentityContext dbContext)
        {
            _dbContext = dbContext;
        }

        private readonly NorthwindInclIdentityContext _dbContext;

        public class CategoryViewModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
       
        public List<CategoryViewModel> Categories { get; set; }

        public class ProductViewModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string CategoryName { get; set; }
            public decimal UnitPrice { get; set; }
        }

        public List<ProductViewModel> Products { get; set; }

        public void OnGet()
        {

            Categories = _dbContext.Categories
                .Take(6)
                .Select(c => new CategoryViewModel
            {
                Id = c.CategoryId,
                Name = c.CategoryName,
            }).ToList();

            //=======================================================

            Products = _dbContext.Products
                .Include(c => c.Category) /*Kan exkluderas!*/
                .Take(5) /*Vi begränsar listan till 5st. */
                .Select(p => new ProductViewModel
                {
                    Id = p.ProductId,
                    Name = p.ProductName,
                    CategoryName = p.Category.CategoryName,
                    UnitPrice = p.UnitPrice.Value
                }).ToList();


        }
    }

}