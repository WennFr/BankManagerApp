using BankManagerApp.Areas.Identity.Pages.Account;
using BankRepository.Services.IdentityUserService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BankManagerApp.Pages.Administrator
{
    public class CreateIdentityUserModel : PageModel
    {
        private readonly IIdentityUserService _identityUserService;

        public CreateIdentityUserModel(IIdentityUserService identityUserService)
        {
            _identityUserService = identityUserService;
        }

        public string Email { get; set; }

        public RegisterModel.InputModel Input { get; set; }

        public void OnGet()
        {
        }
    }
}
