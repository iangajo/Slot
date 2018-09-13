using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SlotAPI.Models;

namespace SlotAPI.DataStores
{
    public interface IStatisticsDataStore
    {
        void PayLineStat(int payline);

        void SymbolStat(string symbol);

        List<PayLineStat> GetPayLineStats();

        List<SymbolStat> GetSymbolStats();
    }
}
