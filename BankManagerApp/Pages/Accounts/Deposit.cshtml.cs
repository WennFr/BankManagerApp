using BankRepository.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace BankManagerApp.Pages.Accounts
{
    [BindProperties]
    public class DepositModel : PageModel
    {

        public DepositModel(IAccountService accountService, ITransactionService transactionService)
        {
            _accountService = accountService;
            _transactionService = transactionService;
        }
        private readonly IAccountService _accountService;
        private readonly ITransactionService _transactionService;


        [Range(100, 10000)]
        public decimal Amount { get; set; }


        public DateTime DepositDate { get; set; }


        [Required(ErrorMessage =
            "Comment is required.")]
        [MinLength(5, ErrorMessage =
            "Comment is to short, 5-250 characters required.")]
        [MaxLength(250, ErrorMessage =
            "Comment is to long, 5-250 characters required.")]
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
                _transactionService.RegisterTransaction(accountId, Amount, newBalance, DepositDate);
                return RedirectToPage("/Accounts/Account", new { accountId = accountId });
            }

            return Page();
        }
    }
}
