using BankRepository.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankRepository.BankAppData;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BankRepository.Services
{
    public class TransactionService : ITransactionService
    {

        public TransactionService(BankAppDataContext dbContext)
        {
            _dbContext = dbContext;
        }

        private readonly BankAppDataContext _dbContext;

       public List<TransactionViewModel> GetAllAccountTransactions(int accountId)
       {

           var query = _dbContext.Transactions.Where(t=> t.AccountId == accountId).AsQueryable();
            
            var viewModelResult = query.Select(t => new TransactionViewModel()
            {
                TransactionId = t.TransactionId,
                TransactionDate = t.Date.ToString(),
                Type = t.Type,
                Operation = t.Operation,
                Amount = t.Amount,
                BalanceAfterTransaction = t.Balance

            }).OrderByDescending(t => t.TransactionDate).ToList();

            return viewModelResult;

        }


    }
}
