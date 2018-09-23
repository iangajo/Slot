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
        private const int MaxWheelNumber = 5;

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
            var gameId = GenerateGameId();

            var activeSlotDimensionArray = new string[3, 5];

            WheelSpin();

            var stillWinning = false;

            CheckIfPlayerHasBonusSpin(ref betAmount, playerId);

            do
            {
                // Populate the slots based on the spin per wheel...
                //
                // If still winning, (Winning symbols were deleted in the 'CheckIfPlayerHasWinningCombinations' function.)
                // Wheel[i] array were re-arrange because we deleted some items (winning symbol)
                // Get the indices in the array (top 3)
                // Repopulate the slots array with the new symbols / retain the not winning symbol (Cascade)
                #region Example
                // As we spin the array will move base on the roll.
                // example: wheel1 array is { 8, 7, 6, 5, 4... }
                // get the top 3 as line in array [0,0], [1,0] [2,0] = (6, 7, 8)
                // let say the winning line is '1' a straight line (array [1,0], [1,1], [1,2], [1,3], [1,4])
                // in our wheel1 the winning line is index 7. compute the win amount in CheckIfPlayerHasWinningCombinations function and we will delete the index 7 in wheel1 array.
                // remaining wheel1 array will be { 8, 6, 5, 4, 3... }
                // repopulate the slot array, in wheel1 will be { 5, 6, 8 } -(cascaded symbols)
                #endregion
                for (var line = 0; line < 3; line++)
                {
                    for (var wheel = 0; wheel < 5; wheel++)
                    {
                        var index = _wheels.Wheel[wheel].Skip(2 - line).First(); //get symbol index
                        var symbol = _reel.GetReelWheel(wheel).First(r => r.Id == index).Symbol; //get symbol base on the index

                        activeSlotDimensionArray[line, wheel] = symbol; // assign / reassign the slot symbol
                    }
                }

                stillWinning = CheckIfPlayerHasWinningCombinations(activeSlotDimensionArray, playerId, stillWinning, betAmount, gameId); //Check if the slot combination has winning combinations

            } while (stillWinning);

            return activeSlotDimensionArray;
        }

        public string GenerateGameId()
        {
            return Guid.NewGuid().ToString();
        }

        public void CheckIfPlayerHasBonusSpin(ref decimal betAmount, int playerId)
        {
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
        }

        public bool CheckIfPlayerHasWinningCombinations(string[,] slots, int playerId, bool cascaded, decimal betAmount, string gameId)
        {
            var winArrayDimensionList = new List<string>();
            var temporaryArrayDimensionList = new List<string>();

            for (var payLines = 1; payLines <= 30; payLines++) //paylines
            {
                var winningPayLine = _win.GetWinningPayLine(payLines);
                temporaryArrayDimensionList.Clear();

                foreach (var symbol in _symbols) //symbols
                {
                    var matchCounter = 0;
                    temporaryArrayDimensionList.Clear();
                    foreach (var lines in winningPayLine)//slot results base on payline
                    {
                        var currentSymbol = GetSymbol(slots, lines, ref temporaryArrayDimensionList);
                        if (currentSymbol == symbol || currentSymbol == "Wild") matchCounter += 1;
                        else break;
                    }

                    if (matchCounter > 2)
                    {
                        winArrayDimensionList.AddRange(temporaryArrayDimensionList); //add the winning symbol to the list of array (as return later)
                        CreditAndRecordWinningCombinations(matchCounter, symbol, betAmount, playerId, gameId, playerId);
                    }
                }
            }

            if (!winArrayDimensionList.Any() && !cascaded)
            {
                _transactionHistory.AddTransactionHistory(betAmount, playerId, "Lose", gameId, 0, string.Empty);
            }

            CheckIfPlayerSpinHasFreeBonusSpin(slots, playerId);

            //If there's a winning symbol in slot
            //get the line(3) and wheel(5) (base on the array row and col)
            //remove the symbol in the array.
            RemoveSymbolsInTheWheelArray(winArrayDimensionList);

            return winArrayDimensionList.Any();
        }

        public void CreditAndRecordWinningCombinations(int match, string symbol, decimal betAmount, int playerId, string gameId, int payLine)
        {
            var winAmount = _win.GetWin(symbol, match, betAmount); //compute the winning amount base on the number of match and symbol

            _accountCredits.Credit(playerId, winAmount); //credit the winning amount

            _transactionHistory.AddTransactionHistory(winAmount, playerId, "Win", gameId, payLine, symbol); //add transaction history

            _statisticsDataStore.SymbolStat(symbol, match); //add symbol stats
            _statisticsDataStore.PayLineStat(payLine); //add payline stats
        }

        public void CheckIfPlayerSpinHasFreeBonusSpin(string[,] slots, int playerId)
        {
            //check for bonuses
            var bonusCounter = 0;
            for (var line = 0; line < 3; line++)
            {
                for (var wheel = 0; wheel < 5; wheel++)
                {
                    var symbol = slots[line, wheel];
                    if (symbol == "Bonus")
                    {
                        bonusCounter += 1;
                    }
                }
            }

            if (bonusCounter >= 3) _accountCredits.CreditBonusSpin(playerId);
        }

        public void RemoveSymbolsInTheWheelArray(List<string> winArrayIndices)
        {
            foreach (var item in winArrayIndices.Distinct())
            {
                var concatenatedSymbols = item.Split(',');

                var row = Convert.ToInt32(concatenatedSymbols[0]);
                var col = Convert.ToInt32(concatenatedSymbols[1]);

                var index = _wheels.Wheel[col].Skip(row).First(); //get the index base on the row and col (array position)

                _wheels.Wheel[col].Remove(index); //remove the item in the array
            }
        }

        #region PrivateMethods

        private byte GenerateRandomNumber()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var numberOfSymbols = new int[25];
                var randomNumber = new byte[1];

                do
                {
                    rng.GetBytes(randomNumber);

                } while (!IsFairSpin(randomNumber[0], (byte)numberOfSymbols.Length));

                return (byte)((randomNumber[0] % (byte)numberOfSymbols.Length) + 1);
            } //dispose the rng
        }

        private bool IsFairSpin(byte spin, byte numberOfSymbols)
        {
            var fullSetsOfValues = Byte.MaxValue / numberOfSymbols;

            return spin < numberOfSymbols * fullSetsOfValues;
        }

        private void WheelSpin()
        {
            for (var i = 0; i < MaxWheelNumber; i++)
            {
                var randomNumber = (int)GenerateRandomNumber();

                //rotate the array based on the roll
                _wheels.Wheel[i] = _wheels.Wheel[i].Skip(randomNumber).Concat(_wheels.Wheel[i].Take(randomNumber))
                    .ToList();
            }
        }

        private string GetSymbol(string[,] slots, string lines, ref List<string> tempData)
        {
            var concatenatedSymbols = lines.Split(',');

            var row = Convert.ToInt32(concatenatedSymbols[0]); //get line
            var col = Convert.ToInt32(concatenatedSymbols[1]); //get wheel

            var currentSymbol = slots[row, col];

            tempData.Add($"{row},{col}"); //save the winning symbol

            return currentSymbol;
        }

        #endregion
    }
}
