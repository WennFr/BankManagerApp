using BankRepository.Services.AccountService;
using BankRepository.Services.CustomerService;
using BankRepository.ViewModels.AccountView;
using BankRepository.ViewModels.CustomerView;
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
        public List<AccountViewModel> Accounts { get; set; }
        public string PreviousPage { get; set; }
        public int AccountIdFromAccountPage { get; set; }
        public string Country { get; set; }



        public void OnGet(int customerId, string previousPage,int accountId)
        {
            Customer = _customerService.GetFullCustomerInformationById(customerId);
            Accounts = _accountService.GetAccountsByCustomerId(customerId).ToList();
            PreviousPage = previousPage;
            Country = Customer.Country;
            AccountIdFromAccountPage = accountId;
        }
    }
}
