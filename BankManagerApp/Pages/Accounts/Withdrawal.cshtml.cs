using BankRepository.Infrastructure.Common;
using BankRepository.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace BankManagerApp.Pages.Accounts
{
    public class WithdrawalModel : PageModel
    {

        public WithdrawalModel(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }
        private readonly ITransactionService _transactionService;


        [Range(100, 10000)]
        public decimal Amount { get; set; }

        public DateTime WithdrawalDate { get; set; }




        public void OnGet()
        {
            WithdrawalDate = DateTime.Now.AddHours(1);
        }

        public IActionResult OnPost(int accountId)
        {

            //if ()
            //{
            //    ModelState.AddModelError("DepositDate", "Please select a current date.");
            //}

            if (ModelState.IsValid)
            {
                var newBalance = _transactionService.RegisterDeposit(accountId, Amount);
                _transactionService.RegisterTransaction(accountId, Amount, newBalance, OperationConstant.CreditInCash, WithdrawalDate, TransactionType.Credit);
                return RedirectToPage("/Accounts/Account", new { accountId = accountId });
            }

            return Page();
        }
    }
}
