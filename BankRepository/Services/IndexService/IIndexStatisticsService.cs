using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankRepository.ViewModels.IndexView;

namespace BankRepository.Services.IndexService
{
    public interface IIndexStatisticsService
    {
        IEnumerable<IndexDataViewModel> GetIndexCountryStatistics();

    }
}
