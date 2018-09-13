using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SlotAPI.Models;

namespace SlotAPI.DataStores
{
    public interface ITransactionHistoryDataStore
    {
        void AddTransactionHistory(decimal winAmount, int playerId, string transaction, string gameId, int winningLine, string winningSymbol);

        TransactionHistory GetLastTransactionHistoryByPlayer(int playerId);

        decimal GetPlayerTotalWinAmount(int playerId);
    }
}
