using BankRepository.Infrastructure.Common;
using BankRepository.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.JSInterop;
using System.ComponentModel.DataAnnotations;

namespace BankManagerApp.Pages.Accounts
{
    [BindProperties]
    public class TransferModel : PageModel
    {
        public TransferModel(IAccountService accountService, ITransactionService transactionService, ICustomerService customerService)
        {
            _accountService = accountService;
            _transactionService = transactionService;
            _customerService = customerService;
        }

        private readonly IAccountService _accountService;
        private readonly ITransactionService _transactionService;
        private readonly ICustomerService _customerService;

        [Range(100, 10000)]
        public decimal Amount { get; set; }
        public decimal CurrentBalance { get; set; }
        public DateTime TransferDate { get; set; }
        public int FromAccountId { get; set; }
        public int ToAccountId { get; set; }

        public void OnGet(int accountId)
        {
            FromAccountId = _accountService.GetAccountByAccountId(accountId).AccountId;
            CurrentBalance = _accountService.GetAccountByAccountId(accountId).Balance;
        }

        public IActionResult OnPostTransferFunds()
        {
            TransferDate = DateTime.Now.AddHours(1);
            var status = _transactionService.ReturnValidationStatus(CurrentBalance, Amount);


            //if (!ModelState.IsValid)
            //{
            //    var errors = new List<string>();

            //    foreach (var entry in ModelState.Values)
            //    {
            //        foreach (var error in entry.Errors)
            //        {
            //            string errorMessage = error.ErrorMessage;
            //            // You can also customize the alert message to your liking
            //            string alertMessage = $"Error: {errorMessage}";
            //            // Show the alert message using JavaScript
            //            string script = $"<script>alert('{alertMessage}');</script>";
            //            Response.WriteAsync(script);
            //        }
            //    }

            //    // Do something with the errors list, such as logging or returning an error response
              
            //}




            if (ModelState.IsValid)
            {

                if (status == ErrorCode.OK)
                {
                    var newFromAccountBalance = _transactionService.RegisterWithdrawal(FromAccountId, Amount);
                    _transactionService.RegisterTransaction(FromAccountId, Amount, newFromAccountBalance, OperationConstant.WithdrawalInCash, TransferDate, TransactionType.Debit);

                    var newToAccountBalance = _transactionService.RegisterDeposit(ToAccountId, Amount);
                    _transactionService.RegisterTransaction(ToAccountId, Amount, newToAccountBalance, OperationConstant.CreditInCash, TransferDate, TransactionType.Credit);


                    return RedirectToPage("/Accounts/Account", new { accountId = FromAccountId });
                }

                if (status == ErrorCode.BalanceTooLow)
                {
                    ModelState.AddModelError("Amount", "The requested withdrawal amount exceeds the available balance.");
                }

                if (status == ErrorCode.IncorrectAmount)
                {
                    ModelState.AddModelError("Amount", "Allowed withdrawal amount is between 100 and 10000.");
                }



            }

            return Page();
        }

        public IActionResult OnPostRetrieveToAccountId(int toAccountId)
        {
            ToAccountId = _accountService.GetAccountByAccountId(toAccountId).AccountId;

            return Page();
        }

    }
}
