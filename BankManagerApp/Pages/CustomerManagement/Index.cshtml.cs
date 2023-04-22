using BankRepository.Services.CustomerService;
using BankRepository.ViewModels.CustomerView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BankManagerApp.Pages.CustomerManagement
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

        public void OnGet(int pageNo)
        {
            var pagedCustomerViewModelResult = _customerService.GetAllCustomers(null, null, 1, 20, null, null);


            if (pageNo == 0)
                pageNo = 1;

            CurrentPage = pagedCustomerViewModelResult.PageCount;
            Customers = pagedCustomerViewModelResult.Customers;

        }
    }
}
