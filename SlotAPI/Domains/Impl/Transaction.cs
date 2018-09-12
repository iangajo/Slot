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
        public Transaction(ITransactionHistoryDataStore transactionHistory)
        {
            _transactionHistory = transactionHistory;
        }
        public TransactionHistory GetLastTransactionHistoryByPlayer(int playerId)
        {
            return _transactionHistory.GetLastTransactionHistoryByPlayer(playerId);
        }
    }
}
