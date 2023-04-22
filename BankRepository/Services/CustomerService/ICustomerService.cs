using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankRepository.ViewModels.CustomerView;

namespace BankRepository.Services.CustomerService
{
    public interface ICustomerService
    {
        List<TopCustomerViewModel> GetTopCustomersByCountry(string country);

        PagedCustomerViewModel GetAllCustomers(string sortColumn, string sortOrder, int pageNo,int pageSize ,string qName, string qCity);

        CustomerInformationViewModel GetFullCustomerInformationById(int customerId);

        CustomerViewModel GetCustomerNameByAccountId(int accountId);



    }
}
