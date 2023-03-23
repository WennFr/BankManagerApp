using BankManagerApp.NorthwindData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BankManagerApp.Pages
{
    public class CategoryModel : PageModel
    {
        private readonly NorthwindInclIdentityContext _dbContext;
        public string CategoryName { get; set; }

        public CategoryModel(NorthwindInclIdentityContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<IndexModel.ProductViewModel> Products { get; set; }
        public void OnGet(int catId)
        {
            CategoryName = _dbContext.Categories
                .First(c => c.CategoryId == catId).CategoryName;

            Products = _dbContext.Products
                .Include(p => p.Category) /*Kan exkluderas!*/
                .Where(p => p.Category.CategoryId == catId)
                .Select(p => new IndexModel.ProductViewModel
                {
                    Id = p.ProductId,
                    Name = p.ProductName,
                    CategoryName = p.Category.CategoryName,
                    UnitPrice = p.UnitPrice.Value
                }).ToList();
        }
    }

}
