using BankManagerApp.NorthwindData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BankManagerApp.Pages
{
    public class SupplierModel : PageModel
    {

        private readonly NorthwindInclIdentityContext _dbContext;

        public string SupplierName { get; set; }


        public SupplierModel(NorthwindInclIdentityContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void OnGet(int id)
        {

            SupplierName = _dbContext.Suppliers.First(s => s.SupplierId == id).CompanyName;

        }
    }
}
