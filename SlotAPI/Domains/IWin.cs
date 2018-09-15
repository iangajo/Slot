using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SlotAPI.Models;

namespace SlotAPI.Domains
{
    public interface IWin
    {
        int[] GetWinningCombinations(int winningCombination);

        decimal GetWin(string symbol, int match, decimal bet);

        List<Odds> GetOddsTable();
    }
}
