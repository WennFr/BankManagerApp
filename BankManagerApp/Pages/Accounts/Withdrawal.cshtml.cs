using BankRepository.Infrastructure.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using BankManagerApp.Infrastructure.Validation;
using Microsoft.EntityFrameworkCore;
using BankRepository.Services.AccountService;
using BankRepository.Services.TransactionService;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace BankManagerApp.Pages.Accounts
{

    [BindProperties]
    public class WithdrawalModel : PageModel
    {

        public WithdrawalModel(IAccountService accountService,ITransactionService transactionService, INotyfService toastNotification)
        {
            _accountService = accountService;
            _transactionService = transactionService;
            _toastNotification = toastNotification;
        }

        private readonly IAccountService _accountService;
        private readonly ITransactionService _transactionService;
        private readonly INotyfService _toastNotification;


        [Range(100, 10000)]
        public decimal Amount { get; set; }
        public decimal CurrentBalance { get; set; }

        public DateTime WithdrawalDate { get; set; }


        public void OnGet(int accountId)
        {
            CurrentBalance = _accountService.GetAccountByAccountId(accountId).Balance;
        }

        public IActionResult OnPost(int accountId)
        {
            WithdrawalDate = DateTime.Now.AddHours(1);
            var status = _transactionService.ReturnValidationStatus(CurrentBalance, Amount);

            if (ModelState.IsValid)
            {

                if (status == TransactionErrorCode.OK)
                {
                    var newBalance = _transactionService.RegisterWithdrawal(accountId, Amount);
                    _transactionService.RegisterTransaction(accountId, Amount, newBalance, OperationConstant.WithdrawalInCash, WithdrawalDate, TransactionType.Debit);

                    _toastNotification.Success("Withdrawal successful! The funds have been withdrawn from the account. ", 10);
                    return RedirectToPage("/Accounts/Account", new { accountId = accountId });
                }

                if (status == TransactionErrorCode.BalanceTooLow)
                {
                    ModelState.AddModelError("Amount", "The requested withdrawal amount exceeds the available balance." );
                }

                if (status == TransactionErrorCode.IncorrectAmount)
                {
                    ModelState.AddModelError("Amount", "Allowed withdrawal amount is between 100 and 10000." );
                }


                
            }

            return Page();
        }
    }
}
