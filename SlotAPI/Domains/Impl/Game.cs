using SlotAPI.DataStores;
using SlotAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace SlotAPI.Domains.Impl
{

    public class Game : IGame
    {
        private const int MaxReel = 5;

        private readonly ITransactionHistoryDataStore _transactionHistory;
        private readonly IReel _reel;
        private readonly IAccountCreditsDataStore _accountCredits;
        private readonly IStatisticsDataStore _statisticsDataStore;
        private readonly IWin _win;
        private readonly string[] _symbols = { "S7", "S6", "S5", "S4", "S3", "S2", "S1", "S0" };

        //Initialize the wheel
        readonly Wheels _wheels = new Wheels()
        {
            Wheel = new List<List<int>>()
            {
                new List<int>(){ 25, 24, 23, 22, 21, 20, 19, 18, 17, 16, 15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 },
                new List<int>(){ 25, 24, 23, 22, 21, 20, 19, 18, 17, 16, 15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 },
                new List<int>(){ 25, 24, 23, 22, 21, 20, 19, 18, 17, 16, 15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 },
                new List<int>(){ 25, 24, 23, 22, 21, 20, 19, 18, 17, 16, 15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 },
                new List<int>(){ 25, 24, 23, 22, 21, 20, 19, 18, 17, 16, 15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 },
            }

        };

        public Game(IReel reel, ITransactionHistoryDataStore transactionHistory, IAccountCreditsDataStore accountCredits, IStatisticsDataStore statisticsDataStore, IWin win)
        {
            _reel = reel;
            _transactionHistory = transactionHistory;
            _accountCredits = accountCredits;
            _statisticsDataStore = statisticsDataStore;
            _win = win;
        }

        public string[,] Spin(int playerId, decimal betAmount)
        {
            return Spin(playerId, betAmount, GenerateGameId());
        }

        public string GenerateGameId()
        {
            return Guid.NewGuid().ToString();
        }

        #region PrivateMethods

        private byte RandomPick()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var numberOfSymbols = new int[25];
                var randomNumber = new byte[1];

                do
                {
                    rng.GetBytes(randomNumber);

                } while (!IsFairSpin(randomNumber[0], (byte) numberOfSymbols.Length));

                return (byte) ((randomNumber[0] % (byte) numberOfSymbols.Length) + 1);
            } //dispose the rng
        }

        private bool IsFairSpin(byte spin, byte numberOfSymbols)
        {
            int fullSetsOfValues = Byte.MaxValue / numberOfSymbols;

            return spin < numberOfSymbols * fullSetsOfValues;
        }

        private string[,] Spin(int playerId, decimal betAmount, string gameId)
        {
            var slots = new string[3, 5];

            for (var i = 0; i < MaxReel; i++)
            {
                var randomNumber = (int)RandomPick();

                //rotate the array based on the roll
                _wheels.Wheel[i] = _wheels.Wheel[i].Skip(randomNumber).Concat(_wheels.Wheel[i].Take(randomNumber))
                    .ToList();
            }

            var stillWinning = false;
            var bonusSpin = _accountCredits.GetPlayerSpinBonus(playerId);

            if (bonusSpin > 0)
            {
                _accountCredits.DebitBonusSpin(playerId);
                betAmount = 1;
            }
            else
            {
                _accountCredits.Debit(playerId, betAmount);
            }

            do
            {
                // Populate the slots based on the spin per wheel...
                //
                // If still winning, (Winning symbols were deleted in the 'CheckWin' function.)
                // Wheel[i] array were re-arrange because we deleted some items (winning symbol)
                // Get the indices in the array (top 3)
                // Repopulate the slots array with the new symbols / retain the not winning symbol
                #region Example
                // As we spin the array will move base on the roll.
                // example: wheel1 array is { 8, 7, 6, 5, 4... }
                // get the top 3 as line in array [0,0], [1,0] [2,0] = (6, 7, 8)
                // let say the winning line is '1' a straight line (array [1,0], [1,1], [1,2], [1,3], [1,4])
                // in our wheel1 the winning line is index 7. compute the win amount in CheckWin function and we will delete the index 7 in wheel1 array.
                // remaining wheel1 array will be { 8, 6, 5, 4, 3... }
                // repopulate the slot array, in wheel1 will be { 5, 6, 8 }
                #endregion
                for (var line = 0; line < 3; line++)
                {
                    for (var wheel = 0; wheel < 5; wheel++)
                    {
                        var index = _wheels.Wheel[wheel].Skip(2 - line).First(); //get symbol index
                        var symbol = _reel.GetReelWheel(wheel).First(r => r.Id == index).Symbol; //get symbol base on the index

                        slots[line, wheel] = symbol; // assign / reassign the slot symbol
                    }
                }

                stillWinning = CheckWin(slots, playerId, stillWinning, betAmount, gameId); //Check if the slot combination has winning combinations

            } while (stillWinning);

            return slots;
        }


        private bool CheckWin(string[,] slots, int playerId, bool cascaded, decimal betAmount, string gameId)
        {
            var winArrayIndices = new List<string>();
            var tempArrayIndices = new List<string>();

            for (var i = 1; i <= 30; i++) //paylines
            {
                var winLines = _win.PayLines(i);
                tempArrayIndices.Clear();

                foreach (var symbol in _symbols) //symbols
                {
                    var matchCounter = 0;
                    tempArrayIndices.Clear();
                    foreach (var lines in winLines)//slot results base on payline
                    {
                        var values = lines.Split(',');

                        var row = Convert.ToInt32(values[0]); //get line
                        var col = Convert.ToInt32(values[1]); //get wheel

                        var currentSymbol = slots[row, col];

                        tempArrayIndices.Add($"{row},{col}"); //save the winning symbol

                        if (currentSymbol == symbol || currentSymbol == "Wild") matchCounter += 1;
                        else break;

                    }

                    if (matchCounter > 2)
                    {
                        winArrayIndices.AddRange(tempArrayIndices); //add the winning symbol to the list of array (as return later)
                        var winAmount = _win.GetWin(symbol, matchCounter, betAmount); //compute the winning amount base on the number of match and symbol

                        _accountCredits.Credit(playerId, winAmount); //credit the winning amount

                        _transactionHistory.AddTransactionHistory(winAmount, playerId, "Win", gameId, i, symbol); //add transaction history

                        _statisticsDataStore.SymbolStat(symbol, matchCounter); //add symbol stats
                        _statisticsDataStore.PayLineStat(i); //add payline stats
                    }
                }
            }

            if (!winArrayIndices.Any() && !cascaded)
            {
                _transactionHistory.AddTransactionHistory(betAmount, playerId, "Lose", gameId, 0, string.Empty);
            }

            //check for bonuses
            var bonusCounter = 0;
            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                    var symbol = slots[i, j];
                    if (symbol == "Bonus")
                    {
                        bonusCounter += 1;
                    }
                }
            }

            if (bonusCounter >= 3) _accountCredits.CreditBonusSpin(playerId);

            //If there's a winning symbol in slot
            //get the line(3) and wheel(5) (base on the array row and col)
            //remove the symbol in the array.
            foreach (var item in winArrayIndices.Distinct())
            {
                var values = item.Split(',');

                var row = Convert.ToInt32(values[0]);
                var col = Convert.ToInt32(values[1]);

                var index = _wheels.Wheel[col].Skip(row).First(); //get the index base on the row and col (array position)

                _wheels.Wheel[col].Remove(index); //remove the item in the array
            }

            return winArrayIndices.Any();
        }

        #endregion
    }
}
