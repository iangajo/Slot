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

        /// <summary>
        /// Generate Game ID
        /// </summary>
        /// <returns></returns>
        string GenerateGameId();

        /// <summary>
        /// Check if player has bonus spin
        /// </summary>
        /// <param name="betAmount"></param>
        /// <param name="playerId"></param>
        void CheckIfPlayerHasBonusSpin(ref decimal betAmount, int playerId);

        /// <summary>
        /// Check If player has winning combinations base on the spin
        /// </summary>
        /// <param name="slots"></param>
        /// <param name="playerId"></param>
        /// <param name="cascaded"></param>
        /// <param name="betAmount"></param>
        /// <param name="gameId"></param>
        /// <returns></returns>
        bool CheckIfPlayerHasWinningCombinations(string[,] slots, int playerId, bool cascaded, decimal betAmount, string gameId);

        /// <summary>
        /// Credit the winning amount and save the transaction to transaction history and statistics
        /// </summary>
        /// <param name="match"></param>
        /// <param name="symbol"></param>
        /// <param name="betAmount"></param>
        /// <param name="playerId"></param>
        /// <param name="gameId"></param>
        /// <param name="payLine"></param>
        void CreditAndRecordWinningCombinations(int match, string symbol, decimal betAmount, int playerId, string gameId, int payLine);

        /// <summary>
        /// Check player active slot combination if there's a bonus symbol
        /// </summary>
        /// <param name="slots"></param>
        /// <param name="playerId"></param>
        void CheckIfPlayerSpinHasFreeBonusSpin(string[,] slots, int playerId);

        /// <summary>
        /// Remove winning symbol in the wheel (for cascading)
        /// </summary>
        /// <param name="winArrayIndices"></param>
        void RemoveSymbolsInTheWheelArray(List<string> winArrayIndices);
    }
}
