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
            _applicationDbContext.Database.EnsureCreated();
        }

        public void PayLineStat(int payLine)
        {
            var line = _applicationDbContext.PayLineWinStat.First(p => p.Id == payLine);
            line.Stat += 1;
            _applicationDbContext.SaveChanges();
        }

        public void SymbolStat(string symbol, int match)
        {
            var line = _applicationDbContext.SymbolWinStat.First(p => p.Symbol == symbol);

            if (match == 3)
                line.ThreeKind += 1;
            else if (match == 4)
                line.FourKind += 1;
            else if (match == 5) line.FiveKind += 1;

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
