using BankRepository.Services;
using BankRepository.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BankManagerApp.Pages.Accounts
{
    public class AccountsModel : PageModel
    {

        public AccountsModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        private readonly IAccountService _accountService;

        public List<AccountViewModel> Accounts { get; set; }


        public void OnGet(string sortColumn, string sortOrder)
        {
            Accounts = _accountService.GetAllAccounts(sortColumn, sortOrder);
        }
    }
}
