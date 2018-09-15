using SlotAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using SlotAPI.DataStores;
using ReelResult = SlotAPI.Models.ReelResult;

namespace SlotAPI.Domains.Impl
{
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

        public Game(IReel reel, ITransactionHistoryDataStore transactionHistory, IAccountCreditsDataStore accountCredits, IStatisticsDataStore statisticsDataStore, IWin win)
        {
            _reel = reel;
            _transactionHistory = transactionHistory;
            _accountCredits = accountCredits;
            _statisticsDataStore = statisticsDataStore;
            _win = win;
        }

        public List<ReelResult> Spin()
        {
            var reelResults = new List<ReelResult>();

            for (var i = 1; i < MaxReel + 1; i++)
            {
                var r = DoSpinPerReel(i);
                reelResults.AddRange(r);

            }

            return reelResults;
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
                        var minimumOrder = spinResults.First(s => s.ReelNumber == reelNumber && s.Position == number).Order;

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
                                Position = position,
                                Order = reel.Id,
                                Symbol = reel.Symbol,
                                ReelNumber = number
                            });
                            position += 5;
                        }
                        
                        var remainingReels = spinResults.Where(r => r.ReelNumber == number && !winnerReelSymbol.Select(w => w.Order).Contains(r.Order));

                        foreach (var remainingReel in remainingReels)
                        {

                            tempSpinResult.Add(new ReelResult()
                            {
                                Position = position,
                                Order = remainingReel.Order,
                                ReelNumber = number,
                                Symbol = remainingReel.Symbol
                            });

                            position += 5;
                        }


                        spinResults.RemoveAll(s => s.ReelNumber == number);

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
                    ReelNumber = reelNumber,
                    Position = reelNo,
                    Symbol = reelStrips.First(r => r.Id == ((position > MaxLine) ? (position - MaxLine) : position)).Symbol,
                    Order = reelStrips.First(r => r.Id == ((position > MaxLine) ? (position - MaxLine) : position)).Id
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

                var result = spinResult.Where(s => winningCombinations.Contains(s.Position)).ToList();

                foreach (var symbol in _symbols)
                {
                    var match = 0;
                    foreach (var item in result)
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
                            Order = item.Order,
                            Position = item.Position,
                            ReelNumber = item.ReelNumber,
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
