using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SlotAPI.DataStore;
using SlotAPI.Models;

namespace SlotAPI.DataStores
{
    public class Transaction : ITransactionHistory
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public Transaction(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public void AddTransactionHistory(decimal winAmount, int playerId, string transaction, string gameId, int winningLine, string winningCombinations)
        {
            _applicationDbContext.TransactionHistory.Add(new TransactionHistory()
            {
                Amount = winAmount,
                PlayerId = playerId,
                Transaction = transaction,
                GameId = gameId,
                WinningLine = winningLine,
                WinningCombination = winningCombinations
            });

            _applicationDbContext.SaveChanges();
        }

        public TransactionHistory GetLastTransactionHistoryByPlayer(int playerId)
        {
            return _applicationDbContext.TransactionHistory.Where(t => t.PlayerId == playerId)
                .OrderByDescending(t => t.Id).First();
        }
    }
}
