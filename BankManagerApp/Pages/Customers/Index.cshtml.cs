using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics.Metrics;
using System.Drawing;
using BankRepository.BankAppData;
using BankRepository.Infrastructure.Paging;
using BankRepository.Services.CustomerService;
using BankRepository.ViewModels.CustomerView;
using Microsoft.AspNetCore.Authorization;

namespace BankManagerApp.Pages.Customers
{
    [Authorize(Policy = "CashierOnly")]
    public class IndexModel : PageModel
    {
        public IndexModel(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        private readonly ICustomerService _customerService;

        public List<CustomerViewModel> Customers { get; set; }

        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public string QName { get; set; }
        public string QCity { get; set; }
        public string SortColumn { get; set; }
        public string SortOrder { get; set; }
        public string PreviousPage { get; set; }



        public void OnGet(string sortColumn, string sortOrder,int pageNo, string qName, string qCity, string previousPage)
        {
            PreviousPage = previousPage;
            QName = qName;
            QCity = qCity;
            SortColumn = sortColumn;
            SortOrder = sortOrder;

            if (pageNo == 0)
                pageNo = 1;
            CurrentPage = pageNo;
            
            var pagedCustomerViewModelResult = _customerService.GetAllCustomers(sortColumn, sortOrder, pageNo,50 ,qName, qCity);
            Customers = pagedCustomerViewModelResult.Customers;
            PageCount = pagedCustomerViewModelResult.PageCount;
        }

      




    }
}
