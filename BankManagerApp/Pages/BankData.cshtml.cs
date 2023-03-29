using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics.Metrics;
using System.Drawing;
using BankRepository.BankAppData;
using BankRepository.ViewModels;

namespace BankManagerApp.Pages
{
    public class BankDataModel : PageModel
    {
        private readonly BankAppDataContext _dbContext;
        public BankDataModel(BankAppDataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<TopCustomerViewModel> Customers { get; set; }



        public void OnGet()
        {
            Customers = _dbContext.Customers.Select(c => new TopCustomerViewModel
            {
                Id = c.CustomerId,
                GivenName = c.Givenname,
                Surname = c.Surname,
                Country = c.Country
            }).ToList();

                
        }

        //public void OnGet(int page = 1, int pageSize = 10)
        //{
        //    int skip = (page - 1) * pageSize;
        //    Customers = _dbContext.Customers
        //        .OrderBy(c => c.CustomerId)
        //        .Skip(skip)
        //        .Take(pageSize)
        //        .Select(c => new CustomerViewModel
        //        {
        //            Id = c.CustomerId,
        //            GivenName = c.Givenname,
        //            Surname = c.Surname,
        //            Country = c.Country
        //        })
        //        .ToList();
        //}





    }
}
