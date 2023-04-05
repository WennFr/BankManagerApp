using BankRepository.Services;
using BankRepository.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace BankManagerApp.Pages.Accounts
{
    public class IndexModel : PageModel
    {

        public IndexModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        private readonly IAccountService _accountService;

        public List<AccountViewModel> Accounts { get; set; }


        public int CurrentPage { get; set; }
        public string SortColumn { get; set; }
        public string SortOrder { get; set; }




        public void OnGet(string sortColumn, string sortOrder, int pageNo)
        {

            SortColumn = sortColumn;
            SortOrder = sortOrder;

            if (pageNo == 0)
                pageNo = 1;
            CurrentPage = pageNo;

            Accounts = _accountService.GetAllAccounts(sortColumn, sortOrder, pageNo);
        }
    }
}
