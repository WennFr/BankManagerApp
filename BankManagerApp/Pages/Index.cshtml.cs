using BankManagerApp.BankAppData;
using BankRepository.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static BankManagerApp.Pages.BankDataModel;

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

        public List<IndexDataViewModel> IndexPageData { get; set; } = new List<IndexDataViewModel>();

        public void OnGet()
        {
            
            IndexPageData.Add(new IndexDataViewModel
            {
                TotalNumberOfCustomers = _dbContext.Customers.Count(),
                TotalNumberOfAccounts = _dbContext.Accounts.Count(),
                TotalSumOfAccounts = _dbContext.Accounts.Sum(a => a.Balance)
            });

        }
    }
}