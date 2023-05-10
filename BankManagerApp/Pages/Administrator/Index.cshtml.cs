using BankRepository.Services.IdentityUserService;
using BankRepository.ViewModels.IdentityUserView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BankManagerApp.Pages.Administrator
{
    [Authorize(Policy = "AdminOnly")]
    public class IndexModel : PageModel
    {

        public IndexModel(IIdentityUserService identityUserService )
        {
            _identityUserService = identityUserService;
        }

        private readonly IIdentityUserService _identityUserService;

        public List<IdentityUserViewModel> IdentityUsers { get; set; }

        public void OnGet()
        {
            IdentityUsers = _identityUserService.GetAllIdentityUsers();
        }
    }
}
