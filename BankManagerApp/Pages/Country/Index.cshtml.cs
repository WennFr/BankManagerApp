using BankRepository.Services.CustomerService;
using BankRepository.ViewModels.CustomerView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BankManagerApp.Pages.Country
{
    public class IndexModel : PageModel
    {
        public IndexModel(ICustomerService customerService)
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
