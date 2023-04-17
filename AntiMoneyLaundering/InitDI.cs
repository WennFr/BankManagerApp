using BankRepository.BankAppData;
using BankRepository.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankRepository.DataAccess;

namespace AntiMoneyLaundering
{
    public static class InitDI
    {
        public static TransactionMonitoring InitTransactionMonitoring()
        {
            var dbContext = new BankAppDataContext();

            var accountService = new AccountService(dbContext);
            return new TransactionMonitoring(new CustomerService(dbContext, accountService),accountService, new TransactionService(dbContext));
        }
    }

}
