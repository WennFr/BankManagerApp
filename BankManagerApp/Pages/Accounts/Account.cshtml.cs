using BankRepository.Services;
using BankRepository.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BankManagerApp.Pages.Accounts
{
    public class AccountModel : PageModel
    {

        public AccountModel(IAccountService accountService,ITransactionService transactionService)
        {
            _accountService = accountService;
            _transactionService = transactionService;
        }

        private readonly IAccountService _accountService;
        private readonly ITransactionService _transactionService;


        public AccountViewModel Account { get; set; }
        public List<TransactionViewModel> Transactions { get; set; }

        public string PreviousPage { get; set; }


        public void OnGet(int accountId, string previousPage)
        {
            Account = _accountService.GetAccountById(accountId);
            Transactions = _transactionService.GetAllAccountTransactions(accountId);
            PreviousPage = previousPage;
        }
    }
}
