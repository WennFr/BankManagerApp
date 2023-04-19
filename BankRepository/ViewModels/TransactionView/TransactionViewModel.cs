using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankRepository.ViewModels.TransactionView
{
    public class TransactionViewModel
    {
        public int TransactionId { get; set; }

        public string Date { get; set; }

        public string Type { get; set; }

        public string? Operation { get; set; }

        public decimal Amount { get; set; }

        public decimal Balance { get; set; }
    }
}
