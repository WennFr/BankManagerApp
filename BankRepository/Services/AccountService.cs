using BankRepository.BankAppData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankRepository.Services
{
    public class AccountService : IAccountService
    {

        public AccountService(BankAppDataContext dbContext)
        {

            _dbContext = dbContext;

        }

        private readonly BankAppDataContext _dbContext;


        public decimal GetTotalCustomerBalance(int customerId)
        {

            var TotalAccountsBalance = _dbContext.Dispositions
                .Where(d => d.CustomerId == customerId && d.Type.ToLower() == "owner")
                .Sum(d => d.Account.Balance);


            return TotalAccountsBalance;
        }



    }
}
