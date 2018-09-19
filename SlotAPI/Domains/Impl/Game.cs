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
            var byteArray = new byte[4];
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

            Cascade:

            for (var i = 0; i < 3; i++)
            {
                for (int j = 0; j < 5; j++)
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
            var winSymbols = new List<string>();
            var tempSymbols = new List<string>();
            for (var i = 1; i <= 30; i++)
            {
                var matchCounter = 1;
                var winLines = _win.PayLines(i);
                var previousSymbol = string.Empty;
                tempSymbols.Clear();
                foreach (var item in winLines)
                {
                    var values = item.Split(',');

                    var col = Convert.ToInt32(values[0]);
                    var row = Convert.ToInt32(values[1]);

                    var currentSymbol = slots[col, row];

                    tempSymbols.Add($"{col},{row}");

                    if (!string.IsNullOrEmpty(previousSymbol))
                    {
                        if (previousSymbol == currentSymbol || currentSymbol == "Wild")
                        {
                            matchCounter += 1;
                        }
                        else
                        {
                            if (matchCounter <= 2) tempSymbols.Clear();
                            break;
                        }
                    }

                    previousSymbol = currentSymbol;
                }

                if (matchCounter > 2)
                {
                    winSymbols.AddRange(tempSymbols);

                    var winAmount = _win.GetWin(previousSymbol, matchCounter, betAmount);

                    _accountCredits.Credit(playerId, winAmount);

                    _transactionHistory.AddTransactionHistory(winAmount, playerId, "Win", gameId, i, previousSymbol);

                    _statisticsDataStore.SymbolStat(previousSymbol, matchCounter);
                    _statisticsDataStore.PayLineStat(i);
                }

                if (!cascaded)
                {
                    _transactionHistory.AddTransactionHistory(0, playerId, "Lose", gameId, i, previousSymbol);
                }
            }

            //remove winning lines
            foreach (var item in winSymbols.Distinct())
            {
                var values = item.Split(',');

                var col = Convert.ToInt32(values[0]);
                var row = Convert.ToInt32(values[1]);

                var index = _wheels.Wheel[row].Skip(col).First();

                _wheels.Wheel[row].Remove(index);
            }

            return winSymbols.Any();
        }

        #endregion
    }
}
