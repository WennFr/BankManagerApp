using BankRepository.Services;
using BankRepository.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BankManagerApp.Pages.Customers
{
    public class TopCustomersByCountryModel : PageModel
    {
        public TopCustomersByCountryModel(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        private readonly ICustomerService _customerService;
        public List<TopCustomerViewModel> TopCustomers { get; set; }


        public void OnGet(string country)
        {
            TopCustomers = _customerService.GetTopCustomersByCountry(country);

        }
    }
}
