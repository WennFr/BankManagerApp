using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankRepository.BankAppData;
using BankRepository.Infrastructure.Common;
using BankRepository.ViewModels.TransactionView;

public enum TransactionErrorCode
{
    OK,
    BalanceTooLow,
    IncorrectAmount
}

namespace BankRepository.Services.TransactionService
{
    public interface ITransactionService
    {
        List<TransactionViewModel> GetAllAccountTransactions(int accountId, int pageNo, int limit, int offset);

        TransactionErrorCode ReturnValidationStatus(decimal balance, decimal amount);

        decimal RegisterDeposit(int accountId, decimal amount);

        decimal RegisterWithdrawal(int accountId, decimal amount);

        void RegisterTransaction(int accountId, decimal amount, decimal newBalance, string operation, DateTime transactionDate, TransactionType type);

    }
}
