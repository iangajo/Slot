using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SlotAPI.Models;

namespace SlotAPI.Domains
{
    public interface IGame
    {
        string[,] Spin(int playerId, decimal betAmount);

        string GenerateGameId();

        void CheckIfPlayerHasBonusSpin(ref decimal betAmount, int playerId);

        bool CheckWin(string[,] slots, int playerId, bool cascaded, decimal betAmount, string gameId);

        void WinLineMatch(int match, string symbol, decimal betAmount, int playerId, string gameId, int payLine);

        void CheckBonus(string[,] slots, int playerId);

        void RemoveSymbolsInTheWheelArray(List<string> winArrayIndices);
    }
}
