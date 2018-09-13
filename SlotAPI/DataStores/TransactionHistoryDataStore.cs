using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SlotAPI.DataStore;
using SlotAPI.Models;

namespace SlotAPI.DataStores
{
    public class TransactionHistoryDataStore : ITransactionHistoryDataStore
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public TransactionHistoryDataStore(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
            _applicationDbContext.Database.EnsureCreated();
        }

        public void AddTransactionHistory(decimal winAmount, int playerId, string transaction, string gameId, int winningLine, string winningSymbol)
        {
            _applicationDbContext.TransactionHistory.Add(new TransactionHistory()
            {
                Amount = winAmount,
                PlayerId = playerId,
                Transaction = transaction,
                GameId = gameId,
                WinningLine = winningLine,
                WinningSymbol = winningSymbol
            });

            _applicationDbContext.SaveChanges();
        }

        public TransactionHistory GetLastTransactionHistoryByPlayer(int playerId)
        {
            if (_applicationDbContext.TransactionHistory.Any(t => t.PlayerId == playerId))
            {
                return _applicationDbContext.TransactionHistory.Where(t => t.PlayerId == playerId)
                    .OrderByDescending(t => t.Id).First();
            }

            return new TransactionHistory()
            {
                Amount = 0.00m,
                GameId = string.Empty,
                PlayerId = 0,
                Transaction = string.Empty,
                WinningSymbol = string.Empty,
                WinningLine = 0
            };
        }

        public decimal GetPlayerTotalWinAmount(int playerId)
        {
            if (!_applicationDbContext.TransactionHistory.Any(t => t.PlayerId == playerId)) return 0;

            var sum = _applicationDbContext.TransactionHistory.Where(s => s.PlayerId == playerId && s.Transaction == "Win").Sum(p => p.Amount);

            return sum;
        }
    }
}
