using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics.Metrics;
using System.Drawing;
using BankRepository.BankAppData;
using BankRepository.Services;
using BankRepository.ViewModels;

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
        public string SortColumn { get; set; }
        public string SortOrder { get; set; }



        public void OnGet(string sortColumn, string sortOrder,int pageNo)
        {
            SortColumn = sortColumn;
            SortOrder = sortOrder;

            if (pageNo == 0)
                pageNo = 1;
            CurrentPage = pageNo;
           

            Customers = _customerService.GetAllCustomers(sortColumn, sortOrder, pageNo);

        }

      




    }
}
