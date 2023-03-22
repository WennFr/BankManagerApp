using BankManagerApp.NorthwindData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BankManagerApp.Pages
{
    public class NorthwindModel : PageModel
    {

        private readonly NorthwindInclIdentityContext _dbContext;

        public NorthwindModel(NorthwindInclIdentityContext dbContext)
        {
            _dbContext = dbContext;
        }


        public class SupplierViewModel
        {
            public int Id { get; set; }
            public string CompanyName { get; set; }
            public string Region { get; set; }
        }

        public List<SupplierViewModel> Suppliers { get; set; }
            = new List<SupplierViewModel>();


        public void OnGet()
        {
            Suppliers = _dbContext.Suppliers.Select(s => new SupplierViewModel
            {
                Id = s.SupplierId,
                CompanyName = s.CompanyName,
                Region = s.Region
            }).ToList();
        }



    }
}
