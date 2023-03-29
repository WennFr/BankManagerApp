using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics.Metrics;
using System.Drawing;
using BankRepository.BankAppData;
using BankRepository.Services;
using BankRepository.ViewModels;

namespace BankManagerApp.Pages.Customers
{
    public class CustomersModel : PageModel
    {
        public CustomersModel(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        private readonly ICustomerService _customerService;

        public List<CustomerViewModel> Customers { get; set; }



        public void OnGet(string sortColumn, string sortOrder)
        {
            Customers = _customerService.GetAllCustomers(sortColumn, sortOrder);

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
