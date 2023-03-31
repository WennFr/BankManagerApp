using BankRepository.Services;
using BankRepository.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BankManagerApp.Pages.Accounts
{
    public class AccountModel : PageModel
    {

        public AccountModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        private readonly IAccountService _accountService;

        public AccountViewModel Account { get; set; }

        public string PreviousPage { get; set; }


        public void OnGet(int accountId, string previousPage)
        {
            Account = _accountService.GetAccountById(accountId);

            PreviousPage = previousPage;


        }
    }
}
