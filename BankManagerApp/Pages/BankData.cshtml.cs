using BankManagerApp.BankAppData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics.Metrics;
using System.Drawing;

namespace BankManagerApp.Pages
{
    public class BankDataModel : PageModel
    {
        private readonly BankAppDataContext _dbContext;
        public BankDataModel(BankAppDataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public class CustomerViewModel
        {
            public int Id { get; set; }
            public string GivenName { get; set; }
            public string Surname { get; set; }
            public string Country { get; set; }

        }

        public List<CustomerViewModel> Customers { get; set; }
            = new List<CustomerViewModel>();


        public void OnGet()
        {
            Customers = _dbContext.Customers.Select(c => new CustomerViewModel
            {
                Id = c.CustomerId,
                GivenName = c.Givenname,
                Surname = c.Surname,
                Country = c.Country
            }).ToList();


        }
    }
}
