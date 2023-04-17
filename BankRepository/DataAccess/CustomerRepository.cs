using BankRepository.BankAppData;
using BankRepository.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankRepository.Infrastructure.Paging;

namespace BankRepository.DataAccess
{
    public class CustomerRepository : ICustomerRepository
    {
        public CustomerRepository(BankAppDataContext dbContext)
        {
            _dbContext = dbContext;
        }

        private readonly BankAppDataContext _dbContext;


        public List<Customer> GetAllCustomers()
        {
            var allCustomers = _dbContext.Customers.ToList();
            return allCustomers;
        }


    }
}
