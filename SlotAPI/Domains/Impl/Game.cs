using SlotAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using SlotAPI.DataStores;
using ReelResult = SlotAPI.Models.ReelResult;

namespace SlotAPI.Domains.Impl
{

    public class Wheels
    {
        public List<List<int>> Wheel { get; set; }
    }

    public class Game : IGame
    {
        private const int MaxLine = 25;
        private const int MaxReel = 5;
        private const int WinCombinations = 30;
        private readonly string[] _symbols = { "S7", "S6", "S5", "S4", "S3", "S2", "S1", "S0" };

        private readonly ITransactionHistoryDataStore _transactionHistory;
        private readonly IReel _reel;
        private readonly IAccountCreditsDataStore _accountCredits;
        private readonly IStatisticsDataStore _statisticsDataStore;
        private readonly IWin _win;

        //Initialize the wheel
        readonly Wheels wheels = new Wheels()
        {
            Wheel = new List<List<int>>()
            {
                new List<int>(){ 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25 },
                new List<int>(){ 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25 },
                new List<int>(){ 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25 },
                new List<int>(){ 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25 },
                new List<int>(){ 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25 },
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



        private List<ReelResult> SpinVer2()
        {
            List<ReelResult> spinResult = new List<ReelResult>();

            //do spin per wheel
            for (int i = 0; i < MaxReel; i++)
            {
                var randomNumber = (int)RandomPick();

                //rotate the array based on the roll
                wheels.Wheel[i] = wheels.Wheel[i].Skip(randomNumber).Concat(wheels.Wheel[i].Take(randomNumber))
                    .ToList();
            }

            //Pick top 3 for each wheel
            //to create the 3x5 slots
            for (int i = 0; i < MaxReel; i++)
            {
                var top3 = wheels.Wheel[i].Take(3).ToList();
                var position = 1 + i;
                foreach (var item in top3)
                {
                    spinResult.Add(new ReelResult()
                    {
                        Symbol = _reel.GetReelStrips(i + 1).First(r => r.Id == item).Symbol, // get symbol based on symbol index
                        SymbolIndex = item,
                        WheelIndex = position,
                        WheelNumber = i + 1
                    });

                    position += 5;
                }

            }

            CheckWin(spinResult, wheels, 1, 10);

            return spinResult;
        }

        private List<ReelResult> CheckWin(List<ReelResult> spinResults, Wheels wheels, int playerId, decimal bet)
        {
            var spinBonus = _accountCredits.GetPlayerSpinBonus(playerId);

            if (spinBonus > 0) { bet = 1; _accountCredits.DebitBonusSpin(playerId); }
            else _accountCredits.Debit(playerId, bet);

            var isCascade = false;

            ReCheckForWin:
            //check if the slot has winning combinations
            var winResults = CheckIfThereMatch(spinResults, bet, GenerateGameId(), isCascade, playerId);

            if (winResults.Any())
            {
                for (var i = 0; i < MaxReel; i++)
                {
                    var wheelNumber = (i + 1);
                    var winnerSymbol = winResults.Where(w => w.ReelNumber == wheelNumber).Select(s => new
                    {
                        Symbol = s.Symbol,
                        SymbolIndex = s.Order,
                        WheelIndex = s.Position,
                        WheelNumber = s.ReelNumber

                    }).ToList().Distinct();

                    foreach (var winner in winnerSymbol)
                    {
                        wheels.Wheel[i].Remove(winner.SymbolIndex);
                    }

                    //reverse the array to get the symbol to cascade
                    wheels.Wheel[i].Reverse();
                    
                    //get the index to be cascaded
                    var cascadedSymbols = wheels.Wheel[i].Take(winnerSymbol.Count()).ToList();

                    //revert back the array
                    wheels.Wheel[i].Reverse();

                    //Cascade the array
                    wheels.Wheel[i] = wheels.Wheel[i].Skip(wheels.Wheel[i].Count - cascadedSymbols.Count).Concat(wheels.Wheel[i].Take(wheels.Wheel[i].Count - cascadedSymbols.Count)).ToList();

                    //remove winner symbol in spinResult
                    spinResults.RemoveAll(s => s.WheelNumber == wheelNumber && winnerSymbol.Any(a => a.Symbol == s.Symbol));

                    //create temporary list to hold the cascaded symbol
                    var holder = new List<ReelResult>();

                    //add cascaded symbol to the slot
                    foreach (var item in cascadedSymbols)
                    {
                        holder.Add(new ReelResult()
                        {
                            Symbol = _reel.GetReelStrips(wheelNumber).First(r => r.Id == item).Symbol,
                            SymbolIndex = item,
                            WheelNumber = wheelNumber
                        });
                    }

                    //get remaining symbols in current reel
                    var remainingReel = spinResults.Where(s => s.WheelNumber == wheelNumber).ToList();

                    //add remaining symbol to holder
                    holder.AddRange(remainingReel);

                    //reindex the holder
                    var positionIndex = wheelNumber;
                    holder.ForEach(s =>
                    {
                        s.WheelIndex = positionIndex;
                        positionIndex += 5;
                    });

                    //Remove all the items for specific wheelNumber
                    spinResults.RemoveAll(s => s.WheelNumber == wheelNumber);

                    //Added the cascaded reel to slots
                    spinResults.AddRange(holder);

                    isCascade = true;

                    goto ReCheckForWin;
                }
            }

            return spinResults;
        }

        public List<ReelResult> Spin()
        {
            return SpinVer2();

            //var reelResults = new List<ReelResult>();

            //for (var i = 1; i < MaxReel + 1; i++)
            //{
            //    var r = DoSpinPerReel(i);
            //    reelResults.AddRange(r);

            //}

            //return reelResults;
        }

        public string GenerateGameId()
        {
            return Guid.NewGuid().ToString();
        }

        public BaseResponse CheckIfPlayerWin(List<ReelResult> spinResults, decimal bet, int playerId)
        {
            var baseResponse = new BaseResponse()
            {
                ErrorMessage = string.Empty
            };

            var isCascade = false;
            var stillWinning = true;

            try
            {

                var spinBonus = _accountCredits.GetPlayerSpinBonus(playerId);

                if (spinBonus > 0)
                {
                    bet = 1;

                    _accountCredits.DebitBonusSpin(playerId);
                }
                else
                {
                    _accountCredits.Debit(playerId, bet);
                }

                while (stillWinning)
                {
                    var winResults = CheckIfThereMatch(spinResults, bet, GenerateGameId(), isCascade, playerId);

                    if (!winResults.Any())
                    {
                        stillWinning = false;
                        break;
                    }

                    isCascade = true;

                    for (var reelNumber = 1; reelNumber < (MaxReel + 1); reelNumber++)
                    {
                        var number = reelNumber;

                        var winnerReelSymbol = winResults.Where(w => w.ReelNumber == number).ToList();

                        var winnerReelSymbolCount = winnerReelSymbol.GroupBy(w => w.Position).Count();

                        if (winnerReelSymbolCount == 0) continue;

                        //Get Minimum order  of the reel
                        var minimumOrder = spinResults.First(s => s.WheelNumber == reelNumber && s.WheelIndex == number).SymbolIndex;

                        var stepBackRange = 1;

                        if (minimumOrder <= 3)
                        {
                            stepBackRange = minimumOrder - winnerReelSymbolCount;

                            if (stepBackRange <= 0) stepBackRange = MaxLine + stepBackRange;
                        }
                        else
                        {
                            stepBackRange = minimumOrder - winnerReelSymbolCount;
                        }

                        var reelStrip = _reel.GetReelStrips(number);

                        List<ReelStrip> reelStripsRange = null;

                        reelStripsRange = minimumOrder < stepBackRange ?
                            reelStrip.Where(r => r.Id < minimumOrder || r.Id >= stepBackRange).ToList() :
                            reelStrip.Where(r => r.Id >= stepBackRange && r.Id < minimumOrder).ToList();

                        var tempSpinResult = new List<ReelResult>();

                        var position = number;

                        foreach (var reel in reelStripsRange)
                        {
                            tempSpinResult.Add(new ReelResult()
                            {
                                WheelIndex = position,
                                SymbolIndex = reel.Id,
                                Symbol = reel.Symbol,
                                WheelNumber = number
                            });
                            position += 5;
                        }

                        var remainingReels = spinResults.Where(r => r.WheelNumber == number && !winnerReelSymbol.Select(w => w.Order).Contains(r.SymbolIndex));

                        foreach (var remainingReel in remainingReels)
                        {

                            tempSpinResult.Add(new ReelResult()
                            {
                                WheelIndex = position,
                                SymbolIndex = remainingReel.SymbolIndex,
                                WheelNumber = number,
                                Symbol = remainingReel.Symbol
                            });

                            position += 5;
                        }


                        spinResults.RemoveAll(s => s.WheelNumber == number);

                        spinResults.AddRange(tempSpinResult);

                    }
                }
            }
            catch (Exception e)
            {
                baseResponse.ErrorMessage = e.Message;
            }

            return baseResponse;
        }


        #region PrivateMethods

        private List<ReelResult> DoSpinPerReel(int reelNumber)
        {
            var reelResultPositions = new List<ReelResult>();

            var reelStrips = _reel.GetReelStrips(reelNumber);

            var spinResult = RandomPick();

            var position = spinResult;
            var reelNo = reelNumber;

            for (var i = 0; i < 3; i++)
            {
                reelResultPositions.Add(new ReelResult()
                {
                    WheelNumber = reelNumber,
                    WheelIndex = reelNo,
                    Symbol = reelStrips.First(r => r.Id == ((position > MaxLine) ? (position - MaxLine) : position)).Symbol,
                    SymbolIndex = reelStrips.First(r => r.Id == ((position > MaxLine) ? (position - MaxLine) : position)).Id
                });

                reelNo += 5;
                position += 1;
            }

            return reelResultPositions;
        }

        private uint RandomPick()
        {
            var rng = new RNGCryptoServiceProvider();
            var byteArray = new byte[4];
            rng.GetBytes(byteArray);

            var randomInteger = BitConverter.ToUInt32(byteArray, 0);

            return (randomInteger % 25) + 1;
        }


        private List<ReelWinResult> CheckIfThereMatch(List<ReelResult> spinResult, decimal bet, string gameId, bool isCascade, int playerId)
        {
            var response = new List<ReelWinResult>();
            var tempReelWinResults = new List<ReelWinResult>();

            var bonusSpinCount = spinResult.Count(s => s.Symbol == "Bonus");

            if (bonusSpinCount >= 3)
            {
                _accountCredits.CreditBonusSpin(playerId);
            }

            for (var i = 1; i < WinCombinations; i++)
            {
                var winningCombinations = _win.GetWinningCombinations(i);

                var result = spinResult.Where(s => winningCombinations.Contains(s.WheelIndex)).ToList();

                foreach (var symbol in _symbols)
                {
                    var match = 0;
                    foreach (var item in result.OrderBy(r => r.WheelNumber))
                    {
                        if (item.Symbol == symbol)
                        {
                            match += 1;
                        }
                        else if (item.Symbol == "Wild")
                        {
                            match += 1;
                        }
                        else
                        {
                            break;
                        }

                        tempReelWinResults.Add(new ReelWinResult()
                        {
                            Symbol = item.Symbol,
                            Order = item.SymbolIndex,
                            Position = item.WheelIndex,
                            ReelNumber = item.WheelNumber,
                            WinCombination = i
                        });
                    }

                    if (match > 2)
                    {
                        var winAmount = _win.GetWin(symbol, match, bet);

                        response.AddRange(tempReelWinResults);
                        var creditResponse = _accountCredits.Credit(playerId, winAmount);

                        _transactionHistory.AddTransactionHistory(winAmount, playerId, "Win", gameId, i, symbol);

                        _statisticsDataStore.SymbolStat(symbol, match);
                        _statisticsDataStore.PayLineStat(i);

                        if (!string.IsNullOrEmpty(creditResponse.ErrorMessage))
                        {
                            throw new Exception(creditResponse.ErrorMessage);
                        }
                    }

                    tempReelWinResults.Clear();
                }
            }

            if (!response.Any() && !isCascade)
            {
                _transactionHistory.AddTransactionHistory(0, playerId, "Lose", gameId, 0, string.Empty);
            }

            return response;
        }

        #endregion
    }
}
