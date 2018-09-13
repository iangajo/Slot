using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SlotAPI.DataStore;
using SlotAPI.Models;

namespace SlotAPI.DataStores
{
    public class StatisticsDataStore : IStatisticsDataStore
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public StatisticsDataStore(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public void PayLineStat(int payline)
        {
            var line = _applicationDbContext.PayLineWinStat.First(p => p.Id == payline);
            line.Stat += 1;
            _applicationDbContext.SaveChanges();
        }

        public void SymbolStat(string symbol)
        {
            var line = _applicationDbContext.SymbolWinStat.First(p => p.Symbol == symbol);
            line.Stat += 1;
            _applicationDbContext.SaveChanges();
        }

        public List<PayLineStat> GetPayLineStats()
        {
            return _applicationDbContext.PayLineWinStat.ToList();
        }

        public List<SymbolStat> GetSymbolStats()
        {
            return _applicationDbContext.SymbolWinStat.ToList();
        }
    }
}
