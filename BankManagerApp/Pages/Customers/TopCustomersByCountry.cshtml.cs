using BankRepository.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BankManagerApp.Pages.Customers
{
    public class TopCustomersByCountryModel : PageModel
    {

        public List<TopCustomerViewModel> TopCustomers { get; set; }


        public void OnGet(string country)
        {


        }
    }
}
