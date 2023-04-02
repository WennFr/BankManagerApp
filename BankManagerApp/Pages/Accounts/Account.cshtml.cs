using BankRepository.Services;
using BankRepository.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BankManagerApp.Pages.Accounts
{
    public class AccountModel : PageModel
    {

        public AccountModel(IAccountService accountService,ITransactionService transactionService, ICustomerService customerService)
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
        public List<TransactionViewModel> Transactions { get; set; }

        public string PreviousPage { get; set; }


        public void OnGet(int accountId, string previousPage)
        {
            Account = _accountService.GetAccountByAccountId(accountId);
            Transactions = _transactionService.GetAllAccountTransactions(accountId);
            Customer = _customerService.GetCustomerNameByAccountId(accountId);
            PreviousPage = previousPage;
        }
    }
}
