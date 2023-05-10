using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntiMoneyLaundering.Services
{
    public class FileService : IFileService
    {


        public DateTime SetupLastMonitoringDate()
        {

            var date = new DateTime();

            var folderName = "../../../MonitoringData";
            var filePath = Path.Combine(folderName, "lastTransactionMonitoring.txt");

            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }

            if (!File.Exists(filePath))
            {
                using (StreamWriter writer = new StreamWriter(filePath, append: true))
                {
                    date = new DateTime(2010, 1, 1);
                    writer.WriteLine($"{date}");
                }
            }

            if (File.Exists(filePath))
            {

                using (StreamReader reader = new StreamReader(filePath))
                {

                    string dateString = reader.ReadLine();
                    date = DateTime.Parse(dateString);
                    date = date.Date;
                }
            }


            return date;
        }

        public void RegisterSuspectedTransactions(List<SuspectedTransaction> suspectedTransactionsByCountry, string country, DateTime monitoringDate)
        {
            var folderName = "../../../MonitoringData";
            var fileName = $"suspected_transactions_{country}_{monitoringDate.ToString("yyyy-MM-dd_HHmm")}.txt";

            var filePath = Path.Combine(folderName, fileName);

            using (StreamWriter writer = new StreamWriter(filePath, append: false))
            {
                foreach (var transaction in suspectedTransactionsByCountry)
                {
                    writer.WriteLine($"Customer Name: {transaction.CustomerName}");
                    writer.WriteLine($"Account ID: {transaction.AccountId}");
                    writer.WriteLine($"Transaction ID: {string.Join(", ", transaction.TransactionIds)}");
                    writer.WriteLine($"Amount: {string.Join(", ", transaction.Amount)}");
                    writer.WriteLine($"Transaction Date: {string.Join(", ", transaction.TransactionDate)}");
                    writer.WriteLine("------------------------------------");
                }
            }

        }

        public void CreateNewLastMonitoringDate()
        {

            var folderName = "../../../MonitoringData";
            var filePath = Path.Combine(folderName, "lastTransactionMonitoring.txt");

            using (StreamWriter writer = new StreamWriter(filePath, append: false))
            {
                var date = DateTime.Now.Date;
                writer.WriteLine($"{date}");
            }

        }

    }

}




