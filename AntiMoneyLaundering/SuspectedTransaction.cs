using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntiMoneyLaundering
{
    public class SuspectedTransaction
    {
        public string CustomerName { get; set; }
        public int AccountId { get; set; }
        public List<decimal> Amount { get; set; }
        public List<string> TransactionDate { get; set; }
        public List<int> TransactionIds { get; set; }
    }
}
