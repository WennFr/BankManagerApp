using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankRepository.BankAppData;
using BankRepository.Infrastructure.Common;
using BankRepository.ViewModels;

namespace BankRepository.Services
{
    public interface ITransactionService
    {
        List<TransactionViewModel> GetAllAccountTransactions(int accountId);
        decimal RegisterDeposit(int accountId, decimal amount);
        void RegisterTransaction(int accountId, decimal amount, decimal newBalance, string Operation, DateTime transactionDate, TransactionType Type);

    }
}
