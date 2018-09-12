using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlotAPI.DataStores
{
    public interface IAccountCredits
    {
        void Credit(int playerId, decimal amount);

        void Debit(int playerId, decimal amount);

        decimal GetBalance(int playerId);
    }
}
