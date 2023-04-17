using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics.Metrics;
using System.Drawing;
using BankRepository.BankAppData;
using BankRepository.Services;
using BankRepository.ViewModels;
using BankRepository.Infrastructure.Paging;

namespace BankManagerApp.Pages.Customers
{
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



        public void OnGet(string sortColumn, string sortOrder,int pageNo, string qName, string qCity)
        {
            QName = qName;
            QCity = qCity;
            SortColumn = sortColumn;
            SortOrder = sortOrder;

            if (pageNo == 0)
                pageNo = 1;
            CurrentPage = pageNo;
            
            var pagedCustomerViewModelResult = _customerService.GetAllCustomers(sortColumn, sortOrder, pageNo, qName, qCity, false);
            Customers = pagedCustomerViewModelResult.Customers;
            PageCount = pagedCustomerViewModelResult.PageCount;
        }

      




    }
}
