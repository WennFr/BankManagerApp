using BankRepository.BankAppData;
using BankRepository.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BankRepository.DataAccess;
using BankRepository.Infrastructure.Profiles;

namespace AntiMoneyLaundering
{
    public static class InitDI
    {
        public static TransactionMonitoring InitTransactionMonitoring()
        {
            var dbContext = new BankAppDataContext();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new CustomerProfile());
                cfg.AddProfile(new AccountProfile());
                cfg.AddProfile(new TransactionProfile());
            });

            IMapper mapper = new Mapper(config);

            var accountService = new AccountService(dbContext, mapper);
            return new TransactionMonitoring(new CustomerService(dbContext, accountService,mapper),accountService, new TransactionService(dbContext, mapper));
        }
    }

}
