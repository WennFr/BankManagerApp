using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using AspNetCoreHero.ToastNotification.Abstractions;
using BankRepository.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using BankRepository.Services.AccountService;
using BankRepository.Services.TransactionService;
using Microsoft.AspNetCore.Authorization;

namespace BankManagerApp.Pages.Accounts
{
    [Authorize(Policy = "CashierOnly")]
    [BindProperties]
    public class DepositModel : PageModel
    {

        public DepositModel(IAccountService accountService, ITransactionService transactionService, INotyfService toastNotification)
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


        public DateTime DepositDate { get; set; }


        [Required(ErrorMessage =
            "Comment is required.")]
        [MinLength(5, ErrorMessage =
            "Comment is too short, 5-250 characters required.")]
        [MaxLength(250, ErrorMessage =
            "Comment is too long, 5-250 characters required.")]
        public string Comment { get; set; }

        public void OnGet()
        {
            DepositDate = DateTime.Now.AddHours(1);
        }

        public IActionResult OnPost(int accountId)
        {

            if (DepositDate < DateTime.Now)
            {
                ModelState.AddModelError("DepositDate", "Please select a current date.");
            }

            if (ModelState.IsValid)
            {
                var newBalance = _transactionService.RegisterDeposit(accountId, Amount);
                _transactionService.RegisterTransaction(accountId, Amount, newBalance, OperationConstant.CreditInCash, DepositDate, TransactionType.Credit);

                _toastNotification.Success("Deposit successful! The funds have been added to the account. ", 10);
                return RedirectToPage("/Accounts/Account", new { accountId = accountId });
            }

            return Page();
        }
    }
}
