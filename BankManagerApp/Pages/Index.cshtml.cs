using BankRepository.BankAppData;
using BankRepository.Services;
using BankRepository.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using static BankManagerApp.Pages.Customers.IndexModel;

namespace BankManagerApp.Pages
{
    public class IndexModel : PageModel
    {
        public IndexModel(ILogger<IndexModel> logger,IIndexStatisticsService indexStatisticsService)
        {
            _logger = logger;
            _indexStatisticsService = indexStatisticsService;
        }

        private readonly ILogger<IndexModel> _logger;
        private readonly IIndexStatisticsService _indexStatisticsService;


        public List<IndexDataViewModel> IndexPageData { get; set; } 

        public void OnGet()
        {

          IndexPageData =  _indexStatisticsService.GetIndexCountryStatistics().ToList();
        }
    }
}