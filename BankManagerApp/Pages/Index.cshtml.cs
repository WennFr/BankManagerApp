using BankManagerApp.BankAppData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BankManagerApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly BankAppDataContext _dbContext;


        public IndexModel(ILogger<IndexModel> logger, BankAppDataContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public void OnGet()
        {

        }
    }
}