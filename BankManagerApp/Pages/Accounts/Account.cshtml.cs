using BankRepository.BankAppData;
using BankRepository.Services.AccountService;
using BankRepository.Services.CustomerService;
using BankRepository.Services.TransactionService;
using BankRepository.ViewModels.AccountView;
using BankRepository.ViewModels.CustomerView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BankManagerApp.Pages.Accounts
{
    [Authorize(Policy = "CashierOnly")]
    public class AccountModel : PageModel
    {

        public AccountModel(IAccountService accountService, ITransactionService transactionService, ICustomerService customerService)
        {
            _accountService = accountService;
            _transactionService = transactionService;
            _customerService = customerService;

        }

        private readonly IAccountService _accountService;
        private readonly ITransactionService _transactionService;
        private readonly ICustomerService _customerService;


        public AccountViewModel Account { get; set; }
        public CustomerViewModel Customer { get; set; }
        public string PreviousPage { get; set; }

        public string Currency { get; set; }

        public void OnGet(int accountId, string previousPage)
        {
            Account = _accountService.GetAccountByAccountId(accountId);
            Customer = _customerService.GetCustomerNameByAccountId(accountId);
            PreviousPage = previousPage;

            Currency = _accountService.GetCurrency();

        }

        public IActionResult OnGetShowMore(int accountId, int pageNo)
        {
            var limit = 20;
            var offset = 0;

            var listOfTransactions = _transactionService.GetAllAccountTransactions(accountId, pageNo, limit, offset);
            return new JsonResult(new { transactions = listOfTransactions });
        }

    }
}
