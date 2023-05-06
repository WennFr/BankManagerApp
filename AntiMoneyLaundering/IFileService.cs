using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntiMoneyLaundering
{
    public interface IFileService
    {
        DateTime SetupLastMonitoringDate();
        void RegisterSuspectedTransactions(List<SuspectedTransaction> suspectedTransactionsByCountry, string country, DateTime monitoringDate);
        void CreateNewLastMonitoringDate();

    }
}
