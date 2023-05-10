using BankRepository.Infrastructure.Common;
using BankRepository.Services.AccountService;
using BankRepository.Services.CustomerService;
using BankRepository.Services.TransactionService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.JSInterop;
using System.ComponentModel.DataAnnotations;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;

namespace BankManagerApp.Pages.Accounts
{
    [Authorize(Policy = "CashierOnly")]
    [BindProperties]
    public class TransferModel : PageModel
    {
        public TransferModel(IAccountService accountService, ITransactionService transactionService, ICustomerService customerService, INotyfService toastNotification)
        {
            _accountService = accountService;
            _transactionService = transactionService;
            _customerService = customerService;
            _toastNotification = toastNotification;
        }

        private readonly IAccountService _accountService;
        private readonly ITransactionService _transactionService;
        private readonly ICustomerService _customerService;
        private readonly INotyfService _toastNotification;

        [Range(100, 10000)]
        public decimal Amount { get; set; }
        public decimal CurrentBalance { get; set; }
        public DateTime TransferDate { get; set; }
        public int FromAccountId { get; set; }

        [Range(1, int.MaxValue)]
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

            if (ModelState.IsValid)
            {

                if (status == TransactionErrorCode.OK)
                {
                    var newFromAccountBalance = _transactionService.RegisterWithdrawal(FromAccountId, Amount);

                    _transactionService.RegisterTransaction(FromAccountId, Amount, newFromAccountBalance, OperationConstant.WithdrawalInCash, TransferDate, TransactionType.Debit);

                    var newToAccountBalance = _transactionService.RegisterDeposit(ToAccountId, Amount);

                    _transactionService.RegisterTransaction(ToAccountId, Amount, newToAccountBalance, OperationConstant.CreditInCash, TransferDate, TransactionType.Credit);

                    _toastNotification.Success($"Transfer was successful! The funds have been safely transferred to account: {ToAccountId}", 10);
                    return RedirectToPage("/Accounts/Account", new { accountId = FromAccountId });
                }

                if (status == TransactionErrorCode.BalanceTooLow)
                {
                    ModelState.AddModelError("Amount", "The requested transfer amount exceeds the available balance.");
                }

                if (status == TransactionErrorCode.IncorrectAmount)
                {
                    ModelState.AddModelError("Amount", "Allowed transfer amount is between 100 and 10000.");
                }
            }

            return Page();
        }

        public IActionResult OnPostRetrieveToAccountId()
        {


            if (FromAccountId == ToAccountId)
            {
                ModelState.AddModelError("ToAccountId", "Can not transfer to the same account.");
            }

            if (ModelState.IsValid && FromAccountId != ToAccountId)
            {
                var status = _accountService.ReturnValidationStatus(ToAccountId);
                if (status == AccountErrorCode.OK)
                {
                    ToAccountId = _accountService.GetAccountByAccountId(ToAccountId).AccountId;
                    return Page();
                }

                if (status == AccountErrorCode.AccountNotFound)
                {
                    ModelState.AddModelError("ToAccountId", "Account could not be found.");
                }
            }

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("ToAccountId", "Invalid input");
            }

            ToAccountId = 0;
            return Page();
        }

    }
}
