using BankRepository.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BankManagerApp.Pages.Accounts
{
    public class TransferModel : PageModel
    {
        public TransferModel(IAccountService accountService, ITransactionService transactionService)
        {
            _accountService = accountService;
            _transactionService = transactionService;
        }

        private readonly IAccountService _accountService;
        private readonly ITransactionService _transactionService;

        public decimal Amount { get; set; }
        public decimal CurrentBalance { get; set; }
        public DateTime TransferDate { get; set; }
        public int FromAccountId { get; set; }
        public int ToAccountId { get; set; }
        public string Q { get; set; }

        public void OnGet(int accountId)
        {
            FromAccountId = _accountService.GetAccountByAccountId(accountId).AccountId;
            CurrentBalance = _accountService.GetAccountByAccountId(accountId).Balance;
        }



    }
}
