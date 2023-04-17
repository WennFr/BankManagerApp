using BankRepository.BankAppData;
using BankRepository.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntiMoneyLaundering
{
    public static class InitDI
    {
        public static TransactionMonitoring InitTransactionMonitoring()
        {
            var dbContext = new BankAppDataContext();
            return new TransactionMonitoring(new CustomerService(dbContext, new AccountService(dbContext)));
        }
    }

}
