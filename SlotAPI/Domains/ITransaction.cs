using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SlotAPI.Models;

namespace SlotAPI.Domains
{
    public interface ITransaction
    {
        TransactionHistory GetLastTransactionHistoryByPlayer(int playerId);
    }
}
