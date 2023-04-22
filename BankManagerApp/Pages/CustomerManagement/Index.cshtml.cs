using BankRepository.ViewModels.CustomerView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BankManagerApp.Pages.CustomerManagement
{
    public class IndexModel : PageModel
    {


        public List<CustomerViewModel> Customers { get; set; }

        public void OnGet()
        {
        }
    }
}
