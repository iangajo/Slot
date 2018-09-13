using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SlotAPI.Models;

namespace SlotAPI.DataStores
{
    public interface IAccountCreditsDataStore
    {
        BaseResponse Credit(int playerId, decimal amount);

        BaseResponse Debit(int playerId, decimal amount);

        decimal GetBalance(int playerId);

        BaseResponse CreditBonusSpin(int playerId);

        BaseResponse DebitBonusSpin(int playerId);

        int GetPlayerSpinBonus(int playerId);
    }
}
