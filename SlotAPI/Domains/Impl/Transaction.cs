using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SlotAPI.DataStores;
using SlotAPI.Models;

namespace SlotAPI.Domains.Impl
{
    public class Transaction : ITransaction
    {
        private readonly ITransactionHistoryDataStore _transactionHistory;
        private readonly IStatisticsDataStore _statisticsDataStore;

        public Transaction(ITransactionHistoryDataStore transactionHistory, IStatisticsDataStore statisticsDataStore)
        {
            _transactionHistory = transactionHistory;
            _statisticsDataStore = statisticsDataStore;
        }
        public TransactionHistory GetLastTransactionHistoryByPlayer(int playerId)
        {
            return _transactionHistory.GetLastTransactionHistoryByPlayer(playerId);
        }

        public List<PayLineStat> GetPayLineStats()
        {
            return _statisticsDataStore.GetPayLineStats();
        }

        public List<SymbolStat> GetSymbolStats()
        {
            return _statisticsDataStore.GetSymbolStats();
        }

        public WinAmount GetPlayerTotalWinAmount(int playerId)
        {
            var response = new WinAmount()
            {
                ErrorMessage = string.Empty,
                Success = true,

            };
            var winAmount = _transactionHistory.GetPlayerTotalWinAmount(playerId);

            response.Amount = winAmount;

            return response;
        }
    }
}
