using BankRepository.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BankManagerApp.Pages.Accounts
{
    [BindProperties]
    public class DepositModel : PageModel
    {

        public DepositModel(IAccountService accountService,ITransactionService transactionService)
        {
            _accountService = accountService;
            _transactionService = transactionService;
        }
        private readonly IAccountService _accountService;
        private readonly ITransactionService _transactionService;


        public decimal Amount { get; set; }

        public DateTime DepositDate { get; set; }

        public string Comment { get; set; }      

        public void OnGet()
        {
            DepositDate = DateTime.Now.AddHours(1);

        }

        public IActionResult OnPost(int accountId)
        {
            _transactionService.RegisterDeposit(accountId,Amount);
            return RedirectToPage("Index");
        }
    }
}
