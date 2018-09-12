using SlotAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using SlotAPI.DataStores;

namespace SlotAPI.Domains.Impl
{
    public class Game : IGame
    {
        private const int MaxLine = 25;
        private const int MaxReel = 5;
        private const int WinCombinations = 30;
        private readonly string[] _symbols = { "S7", "S6", "S5", "S4", "S3", "S2", "S1", "S0" };

        private readonly ITransactionHistory _transactionHistory;
        private readonly IReel _reel;
        private readonly IAccountCredits _accountCredits;
        public Game(IReel reel, ITransactionHistory transactionHistory, IAccountCredits accountCredits)
        {
            _reel = reel;
            _transactionHistory = transactionHistory;
            _accountCredits = accountCredits;
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

                _accountCredits.Debit(playerId, bet);

                while (stillWinning)
                {
                    var winResults = CheckIfThereMatch(spinResults, 10, GenerateGameId(), isCascade, playerId);

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

                        //remove winning reel
                        spinResults.RemoveAll(r => winnerReelSymbol.Select(w => w.Order).Contains(r.Order) && r.ReelNumber == number);

                        //Get remaining symbol per reel
                        var remainingSymbol = spinResults.Where(r => r.ReelNumber == number).ToList();

                        var remainingSymbolCount = remainingSymbol.Count();

                        //Move down the remaining reel symbol
                        bool hasLastItem = false;
                        if (remainingSymbol.Any())
                        {

                            foreach (var item in remainingSymbol.OrderByDescending(r => r.Position))
                            {
                                var movePosition = item.Position;

                                if (remainingSymbolCount == 1 && item.Position < 6)
                                {
                                    movePosition += 10;
                                }
                                else if (remainingSymbolCount == 1 && item.Position > 5 && item.Position < 11)
                                {
                                    movePosition += 5;
                                }
                                else if (remainingSymbolCount > 1 && item.Position > 10)
                                {
                                    hasLastItem = true;
                                }
                                else if (remainingSymbolCount > 1 && item.Position < 6)
                                {
                                    if (hasLastItem)
                                        movePosition += 5;
                                    else
                                        movePosition += 10;

                                }
                                else if (remainingSymbolCount > 1 && item.Position > 5 && item.Position < 11)
                                {
                                    if (!hasLastItem)
                                        movePosition += 5;
                                }

                                if (!hasLastItem)
                                {
                                    spinResults.First(r => r.ReelNumber == number
                                                           && r.Symbol == item.Symbol
                                                           && r.Order == item.Order
                                                           && r.Position == item.Position).Position = movePosition;
                                }

                            }
                        }

                        var reel = _reel.GetReelStrips(number);

                        if (remainingSymbolCount > 0)
                            reel.RemoveAll(r => winnerReelSymbol.Select(w => w.Order).Contains(r.Id));

                        var numberOfSymbolsToCascade = (3 - remainingSymbolCount);

                        var minimumOrder = 0;

                        minimumOrder = numberOfSymbolsToCascade < 3 ? remainingSymbol.OrderBy(r => r.Order).First(r => r.ReelNumber == number).Order : winnerReelSymbol.OrderBy(r => r.Order).First(r => r.ReelNumber == number).Order;

                        var order = (minimumOrder - 1);

                        var counter = 0;

                        while (counter != numberOfSymbolsToCascade)
                        {

                            if (order <= 0)
                                order = 25;

                            if (reel.Any(r => r.Id == order))
                            {
                                counter += 1;
                                var x = reel.First(r => r.Id == order);

                                spinResults.Add(new ReelResult()
                                {
                                    ReelNumber = number,
                                    Order = x.Id,
                                    Symbol = x.Symbol,
                                    Position = counter * 5
                                });

                            }

                            order -= 1;
                        }
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

            if (spinResult >= 24)
            {
                var position = spinResult;

                reelResultPositions.Add(new ReelResult()
                {
                    ReelNumber = reelNumber,
                    Position = reelNumber,
                    Symbol = reelStrips.First(r => r.Id == position).Symbol,
                    Order = reelStrips.First(r => r.Id == position).Id,
                });

                position += 1;

                reelResultPositions.Add(new ReelResult()
                {
                    ReelNumber = reelNumber,
                    Position = reelNumber + 5,
                    Symbol = reelStrips.First(r => r.Id == ((position > MaxLine) ? (position - MaxLine) : position)).Symbol,
                    Order = reelStrips.First(r => r.Id == ((position > MaxLine) ? (position - MaxLine) : position)).Id

                });

                position += 1;

                reelResultPositions.Add(new ReelResult()
                {
                    Position = reelNumber + 10,
                    Symbol = reelStrips.First(r => r.Id == ((position > MaxLine) ? (position - MaxLine) : position)).Symbol,
                    Order = reelStrips.First(r => r.Id == ((position > MaxLine) ? (position - MaxLine) : position)).Id,
                    ReelNumber = reelNumber
                });
            }
            else
            {
                reelResultPositions.Add(new ReelResult()
                {
                    Position = reelNumber,
                    Symbol = reelStrips.First(r => r.Id == spinResult).Symbol,
                    Order = reelStrips.First(r => r.Id == spinResult).Id,
                    ReelNumber = reelNumber
                });

                reelResultPositions.Add(new ReelResult()
                {
                    Position = reelNumber + 5,
                    Symbol = reelStrips.First(r => r.Id == spinResult + 1).Symbol,
                    Order = reelStrips.First(r => r.Id == spinResult + 1).Id,
                    ReelNumber = reelNumber
                });

                reelResultPositions.Add(new ReelResult()
                {
                    Position = reelNumber + 10,
                    Symbol = reelStrips.First(r => r.Id == spinResult + 2).Symbol,
                    Order = reelStrips.First(r => r.Id == spinResult + 2).Id,
                    ReelNumber = reelNumber
                });
            }

            return reelResultPositions;
        }

        private int RandomPick()
        {
            var random = new Random();
            return random.Next(1, MaxLine);
        }


        private List<ReelWinResult> CheckIfThereMatch(List<ReelResult> spinResult, decimal bet, string gameId, bool isCascade, int playerId)
        {
            var response = new List<ReelWinResult>();
            var tempReelWinResults = new List<ReelWinResult>();

            for (var i = 1; i < WinCombinations; i++)
            {
                var winningCombinations = GetWinningCombinations(i);

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
                        var winAmount = GetWin(symbol, match, bet);

                        response.AddRange(tempReelWinResults);

                        var winnings = string.Join(",", winningCombinations);

                        _transactionHistory.AddTransactionHistory(winAmount, playerId, "Win", gameId, i, winnings);

                        var creditResponse = _accountCredits.Credit(playerId, winAmount);

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


        private int[] GetWinningCombinations(int winningCombination)
        {
            switch (winningCombination)
            {
                case 1:
                    return new[] { 6, 7, 8, 9, 10 };
                case 2:
                    return new[] { 1, 2, 3, 4, 5 };
                case 3:
                    return new[] { 11, 12, 13, 14, 15 };
                case 4:
                    return new[] { 1, 2, 8, 9, 10 };
                case 5:
                    return new[] { 11, 12, 8, 9, 10 };
                case 6:
                    return new[] { 1, 2, 13, 14, 15 };
                case 7:
                    return new[] { 6, 7, 8, 9, 10 };
                case 8:
                    return new[] { 6, 7, 3, 9, 15 };
                case 9:
                    return new[] { 6, 7, 13, 9, 5 };
                case 10:
                    return new[] { 1, 7, 3, 4, 5 };
                case 11:
                    return new[] { 11, 7, 13, 14, 15 };
                case 12:
                    return new[] { 6, 2, 3, 4, 10 };
                case 13:
                    return new[] { 6, 12, 13, 14, 10 };
                case 14:
                    return new[] { 6, 2, 8, 4, 5 };
                case 15:
                    return new[] { 6, 12, 8, 14, 15 };
                case 16:
                    return new[] { 1, 7, 8, 14, 15 };
                case 17:
                    return new[] { 11, 7, 8, 4, 5 };
                case 18:
                    return new[] { 6, 2, 13, 9, 10 };
                case 19:
                    return new[] { 6, 12, 3, 9, 10 };
                case 20:
                    return new[] { 1, 7, 13, 14, 15 };
                case 21:
                    return new[] { 11, 7, 3, 4, 5 };
                case 22:
                    return new[] { 1, 12, 3, 9, 15 };
                case 23:
                    return new[] { 11, 2, 13, 9, 5 };
                case 24:
                    return new[] { 1, 12, 8, 4, 10 };
                case 25:
                    return new[] { 11, 2, 8, 14, 10 };
                case 26:
                    return new[] { 1, 12, 13, 4, 5 };
                case 27:
                    return new[] { 11, 2, 3, 14, 15 };
                case 28:
                    return new[] { 6, 7, 8, 4, 5 };
                case 29:
                    return new[] { 6, 7, 8, 14, 15 };
                case 30:
                    return new[] { 1, 12, 13, 14, 15 };
                default: return new int[] { };

            }
        }


        private decimal GetWin(string symbol, int match, decimal bet)
        {
            var odds = GetOddsTable().Where(g => g.Symbol == symbol);

            switch (match)
            {
                case 5:
                    return odds.First().FiveKind * bet;
                case 4:
                    return odds.First().FourKind * bet;
                case 3:
                    return odds.First().ThreeKind * bet;
                default: return 0;
            }
        }

        private List<Odds> GetOddsTable()
        {
            return new List<Odds>()
            {
                new Odds() { Symbol =  "S0", FiveKind = 60, FourKind = 30, ThreeKind = 5, TwoKind = 0 },
                new Odds() { Symbol =  "S1", FiveKind = 80, FourKind = 30, ThreeKind = 10, TwoKind = 0 },
                new Odds() { Symbol =  "S2", FiveKind = 80, FourKind = 30, ThreeKind = 10, TwoKind = 0 },
                new Odds() { Symbol =  "S3", FiveKind = 100, FourKind = 40, ThreeKind = 20, TwoKind = 0 },
                new Odds() { Symbol =  "S4", FiveKind = 130, FourKind = 40, ThreeKind = 20, TwoKind = 0 },
                new Odds() { Symbol =  "S5", FiveKind = 150, FourKind = 50, ThreeKind = 30, TwoKind = 0 },
                new Odds() { Symbol =  "S6", FiveKind = 200, FourKind = 60, ThreeKind = 30, TwoKind = 0 },
                new Odds() { Symbol =  "S7", FiveKind = 1000, FourKind = 300, ThreeKind = 50, TwoKind = 0 },
            };
        }

        #endregion
    }
}
