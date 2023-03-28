using BankRepository.BankAppData;
using BankRepository.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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

            foreach (var country in _dbContext.Customers.Select(c => c.Country).Distinct())
            {
                IndexPageData.Add(new IndexDataViewModel
                {
                    TotalNumberOfCustomers = _dbContext.Customers.Where(c => c.Country == country).Count(),

                    TotalNumberOfAccounts = _dbContext.Dispositions
                        .Where(d => d.Customer.Country == country && d.Type.ToLower() == "owner")
                        .Distinct()
                        .Count(),
                    TotalSumOfAccounts = _dbContext.Dispositions
                        .Include(d => d.Account)
                        .Where(d => d.Customer.Country == country && d.Type.ToLower() == "owner")
                        .Sum(d => d.Account.Balance),

                    Country = country
                });
            }
        }
    }
}