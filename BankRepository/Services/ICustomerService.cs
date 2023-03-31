using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankRepository.ViewModels;

namespace BankRepository.Services
{
    public interface ICustomerService
    {
        List<TopCustomerViewModel> GetTopCustomersByCountry(string country);

        List<CustomerViewModel> GetAllCustomers(string sortColumn, string sortOrder, int pageNo);

        CustomerViewModel GetCustomerById(int cutomerId);


    }
}
