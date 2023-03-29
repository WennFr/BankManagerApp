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

    }
}
