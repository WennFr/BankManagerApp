using BankRepository.BankAppData;
using BankRepository.Services.IndexService;
using BankRepository.ViewModels.IndexView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using static BankManagerApp.Pages.Customers.IndexModel;

namespace BankManagerApp.Pages
{
    public class IndexModel : PageModel
    {
        public IndexModel(ILogger<IndexModel> logger, IIndexStatisticsService indexStatisticsService)
        {
            _logger = logger;
            _indexStatisticsService = indexStatisticsService;
        }

        private readonly ILogger<IndexModel> _logger;
        private readonly IIndexStatisticsService _indexStatisticsService;

        public string Paragraph { get; set; } =
            $"Access detailed statistics by country for all the Scandinavian accounts, allowing you to track our bank's performance and identify trends in Scandinavian banking.";

        public List<IndexDataViewModel> IndexPageData { get; set; }

        public void OnGet()
        {

            IndexPageData = _indexStatisticsService.GetIndexCountryStatistics().ToList();

        }
    }
}