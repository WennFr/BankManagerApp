using BankRepository.Infrastructure.Common;
using BankRepository.Services.AccountService;
using BankRepository.Services.CustomerService;
using BankRepository.Services.TransactionService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.JSInterop;
using System.ComponentModel.DataAnnotations;
using AspNetCoreHero.ToastNotification.Abstractions;
using BankRepository.ViewModels.CustomerView;
using Microsoft.AspNetCore.Authorization;
using BankRepository.BankAppData;

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
    
        public string? FromCustomerName { get; set; }
        public string? ToCustomerName { get; set; }


        [Range(1, int.MaxValue, ErrorMessage = "Please choose a valid number larger than 1")]
        public int ToAccountId { get; set; }
        public string? Currency { get; set; }

        public void OnGet(int accountId)
        {
            FromAccountId = _accountService.GetAccountByAccountId(accountId).AccountId;
            CurrentBalance = _accountService.GetAccountByAccountId(accountId).Balance;
            Currency = _accountService.GetCurrency();
            var customerName = _customerService.GetCustomerNameByAccountId(accountId).Givenname + " " + _customerService.GetCustomerNameByAccountId(accountId).Surname;
            FromCustomerName = customerName;
            ToCustomerName = "";
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

        public IActionResult OnPostRetrieveToAccountId(int fromAccountId)
        {
            Currency = _accountService.GetCurrency();
            var customerName = _customerService.GetCustomerNameByAccountId(fromAccountId).Givenname + " " + _customerService.GetCustomerNameByAccountId(fromAccountId).Surname;
            FromCustomerName = customerName;

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
                    customerName = _customerService.GetCustomerNameByAccountId(ToAccountId).Givenname + " " + _customerService.GetCustomerNameByAccountId(ToAccountId).Surname;
                    ToCustomerName = customerName;
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
