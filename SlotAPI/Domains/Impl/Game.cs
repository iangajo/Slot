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

        private uint RandomPick()
        {
            var rng = new RNGCryptoServiceProvider();
            var byteArray = new byte[25];
            rng.GetBytes(byteArray);

            var randomInteger = BitConverter.ToUInt32(byteArray, 0);

            return (randomInteger % 25) + 1;
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

            var isCascaded = false;
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
            
            Cascade:

            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                    var index = _wheels.Wheel[j].Skip(2 - i).First();
                    var symbol = _reel.GetReelWheel(j).First(r => r.Id == index).Symbol;

                    slots[i, j] = symbol;
                }
            }

            var hasWin = CheckWin(slots, playerId, isCascaded, betAmount, gameId);

            isCascaded = hasWin;

            if (hasWin) goto Cascade;

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

                        var col = Convert.ToInt32(values[0]);
                        var row = Convert.ToInt32(values[1]);

                        var currentSymbol = slots[col, row];

                        tempArrayIndices.Add($"{col},{row}");

                        if (currentSymbol == symbol || currentSymbol == "Wild") matchCounter += 1;
                        else break;

                    }

                    if (matchCounter > 2)
                    {
                        winArrayIndices.AddRange(tempArrayIndices);
                        var winAmount = _win.GetWin(symbol, matchCounter, betAmount);

                        _accountCredits.Credit(playerId, winAmount);

                        _transactionHistory.AddTransactionHistory(winAmount, playerId, "Win", gameId, i, symbol);

                        _statisticsDataStore.SymbolStat(symbol, matchCounter);
                        _statisticsDataStore.PayLineStat(i);
                    }
                }
            }

            if (!winArrayIndices.Any() && !cascaded)
            {
                _transactionHistory.AddTransactionHistory(0, playerId, "Lose", gameId, 0, string.Empty);
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

            //remove winning lines
            foreach (var item in winArrayIndices.Distinct())
            {
                var values = item.Split(',');

                var col = Convert.ToInt32(values[0]);
                var row = Convert.ToInt32(values[1]);

                var index = _wheels.Wheel[row].Skip(col).First();

                _wheels.Wheel[row].Remove(index);
            }

            return winArrayIndices.Any();
        }

        #endregion
    }
}
