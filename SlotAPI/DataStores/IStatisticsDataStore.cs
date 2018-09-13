using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SlotAPI.Models;

namespace SlotAPI.DataStores
{
    public interface IStatisticsDataStore
    {
        void PayLineStat(int payLine);

        void SymbolStat(string symbol, int match);

        List<PayLineStat> GetPayLineStats();

        List<SymbolStat> GetSymbolStats();
    }
}
