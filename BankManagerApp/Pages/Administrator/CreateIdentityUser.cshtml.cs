using BankManagerApp.Areas.Identity.Pages.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BankManagerApp.Pages.Administrator
{
    public class CreateIdentityUserModel : PageModel
    {

        public RegisterModel.InputModel Input { get; set; }

        public void OnGet()
        {
        }
    }
}
