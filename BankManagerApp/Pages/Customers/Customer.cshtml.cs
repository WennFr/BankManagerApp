using BankRepository.Services;
using BankRepository.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BankManagerApp.Pages.Customers
{
    public class CustomerModel : PageModel
    {
        public CustomerModel(ICustomerService customerService,IAccountService accountService)
        {
            _customerService = customerService;
            _accountService = accountService;
        }

        private readonly ICustomerService _customerService;
        private readonly IAccountService _accountService;

        public CustomerInformationViewModel Customer { get; set; }



        public void OnGet(int customerId)
        {
            Customer = _customerService.GetFullCustomerInformationById(customerId);

        }
    }
}
