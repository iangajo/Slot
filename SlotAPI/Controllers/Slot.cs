using System;
using Microsoft.AspNetCore.Mvc;
using SlotAPI.Models;
using System.Collections.Generic;
using System.Linq;
using SlotAPI.DataStore;

namespace SlotAPI.Controllers
{

    [ApiController]
    public class Slot : ControllerBase
    {
        private readonly ApplicationDbContext _applicationDbContext;

        private const int MaxReel = 5;
        private const int MaxLine = 25;
        private const int WinCombinations = 30;
        private readonly string[] _symbols = { "S7", "S6", "S5", "S4", "S3", "S2", "S1", "S0" };

        public Slot(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        [HttpPost]
        [Route("api/spin")]
        public ActionResult Spin()
        {
            var result = new List<ReelResult>();

            for (var i = 1; i < MaxReel + 1; i++)
            {
                var r = DoSpinPerReel(i);
                result.AddRange(r);

            }

            var stillWinning = true;

            var gameId = Guid.NewGuid().ToString();

            while (stillWinning)
            {
                var winResults = CheckIfThereMatch(result, 10, gameId, out var match);

                if (!winResults.Any())
                {
                    stillWinning = false;
                    break;
                }

                for (var i = 1; i < (match + 1); i++)
                {
                    var reelResults = winResults.Where(w => w.ReelNumber == i);

                    var line1 = reelResults.First().Order - 3;
                    var line2 = line1 + 1;
                    var line3 = line1 + 2;

                    if (line1 <= 0)
                    {
                        line1 = (25 + line1);
                        line2 = (line1 + 1) > MaxLine ? ((line1 + 1) - MaxLine) : (line1 + 1);
                        line3 = (line1 + 2) > MaxLine ? ((line1 + 2) - MaxLine) : (line1 + 2);
                    }

                    //remove all the reelNumber
                    result.RemoveAll(r => r.ReelNumber == i);

                    //Get reelStrips
                    var reelStrips = GetReelStrips(i);

                    //cascade
                    var addToResults = new List<ReelResult>()
                    {
                        new ReelResult()
                        {
                            ReelNumber = i,
                            Position = i,
                            Symbol = reelStrips.First(r => r.Id == line1).Symbol,
                            Order = reelStrips.First(r => r.Id == line1).Id
                        },

                        new ReelResult()
                        {
                            ReelNumber = i,
                            Position = i + 5,
                            Symbol = reelStrips.First(r => r.Id == line2).Symbol,
                            Order = reelStrips.First(r => r.Id == line2).Id
                        },

                        new ReelResult()
                        {
                            ReelNumber = i,
                            Position = i + 10,
                            Symbol = reelStrips.First(r => r.Id == line3).Symbol,
                            Order = reelStrips.First(r => r.Id == line3).Id
                        },
                    };

                    result.AddRange(addToResults);

                }
            }



            //var winAmount = CheckIfPlayerWin(result, 10);

            return Ok();
        }



        #region PrivateMethods

        private List<Reel> GetReelStrips(int reelNumber)
        {
            switch (reelNumber)
            {
                case 1:
                    return new List<Reel>
                    {
                        new Reel() {Id = 1, Symbol = "S0" },
                        new Reel() {Id = 2, Symbol = "S1" },
                        new Reel() {Id = 3, Symbol = "S0" },
                        new Reel() {Id = 4, Symbol = "S2" },
                        new Reel() {Id = 5, Symbol = "S3" },
                        new Reel() {Id = 6, Symbol = "S1" },
                        new Reel() {Id = 7, Symbol = "S3" },
                        new Reel() {Id = 8, Symbol = "S0" },
                        new Reel() {Id = 9, Symbol = "S0" },
                        new Reel() {Id = 10, Symbol = "S2" },
                        new Reel() {Id = 11, Symbol = "S4" },
                        new Reel() {Id = 12, Symbol = "S2" },
                        new Reel() {Id = 13, Symbol = "S1" },
                        new Reel() {Id = 14, Symbol = "S1" },
                        new Reel() {Id = 15, Symbol = "S4" },
                        new Reel() {Id = 16, Symbol = "S2" },
                        new Reel() {Id = 17, Symbol = "S5" },
                        new Reel() {Id = 18, Symbol = "S6" },
                        new Reel() {Id = 19, Symbol = "S7" },
                        new Reel() {Id = 20, Symbol = "Bonus" },
                        new Reel() {Id = 21, Symbol = "S6" },
                        new Reel() {Id = 22, Symbol = "S0" },
                        new Reel() {Id = 23, Symbol = "S5" },
                        new Reel() {Id = 24, Symbol = "Bonus "},
                        new Reel() {Id = 25, Symbol = "S0 "}
                    };
                case 2:
                    return new List<Reel>
                    {
                        new Reel() {Id = 1, Symbol = "S0" },
                        new Reel() {Id = 2, Symbol = "S1" },
                        new Reel() {Id = 3, Symbol = "S2" },
                        new Reel() {Id = 4, Symbol = "S0" },
                        new Reel() {Id = 5, Symbol = "S0" },
                        new Reel() {Id = 6, Symbol = "S3" },
                        new Reel() {Id = 7, Symbol = "S4" },
                        new Reel() {Id = 8, Symbol = "S1" },
                        new Reel() {Id = 9, Symbol = "S5" },
                        new Reel() {Id = 10, Symbol = "Bonus" },
                        new Reel() {Id = 11, Symbol = "S1" },
                        new Reel() {Id = 12, Symbol = "Bonus" },
                        new Reel() {Id = 13, Symbol = "S6" },
                        new Reel() {Id = 14, Symbol = "S5" },
                        new Reel() {Id = 15, Symbol = "S0" },
                        new Reel() {Id = 16, Symbol = "S0" },
                        new Reel() {Id = 17, Symbol = "S0" },
                        new Reel() {Id = 18, Symbol = "S2" },
                        new Reel() {Id = 19, Symbol = "S1" },
                        new Reel() {Id = 20, Symbol = "S7" },
                        new Reel() {Id = 21, Symbol = "S0" },
                        new Reel() {Id = 22, Symbol = "Bonus" },
                        new Reel() {Id = 23, Symbol = "S1" },
                        new Reel() {Id = 24, Symbol = "S4"},
                        new Reel() {Id = 25, Symbol = "Wild"}
                    };
                case 3:
                    return new List<Reel>
                    {
                        new Reel() {Id = 1, Symbol = "S0" },
                        new Reel() {Id = 2, Symbol = "S2" },
                        new Reel() {Id = 3, Symbol = "S3" },
                        new Reel() {Id = 4, Symbol = "S4" },
                        new Reel() {Id = 5, Symbol = "S1" },
                        new Reel() {Id = 6, Symbol = "S1" },
                        new Reel() {Id = 7, Symbol = "S5" },
                        new Reel() {Id = 8, Symbol = "S7" },
                        new Reel() {Id = 9, Symbol = "S1" },
                        new Reel() {Id = 10, Symbol = "Bonus" },
                        new Reel() {Id = 11, Symbol = "S2" },
                        new Reel() {Id = 12, Symbol = "S1" },
                        new Reel() {Id = 13, Symbol = "Bonus" },
                        new Reel() {Id = 14, Symbol = "Wild" },
                        new Reel() {Id = 15, Symbol = "Wild" },
                        new Reel() {Id = 16, Symbol = "S2" },
                        new Reel() {Id = 17, Symbol = "S2" },
                        new Reel() {Id = 18, Symbol = "S1" },
                        new Reel() {Id = 19, Symbol = "S1" },
                        new Reel() {Id = 20, Symbol = "Bonus" },
                        new Reel() {Id = 21, Symbol = "S1" },
                        new Reel() {Id = 22, Symbol = "S3" },
                        new Reel() {Id = 23, Symbol = "S3" },
                        new Reel() {Id = 24, Symbol = ""},
                        new Reel() {Id = 25, Symbol = ""}
                    };
                case 4:
                    return new List<Reel>
                    {
                        new Reel() {Id = 1, Symbol = "S3" },
                        new Reel() {Id = 2, Symbol = "S3" },
                        new Reel() {Id = 3, Symbol = "S4" },
                        new Reel() {Id = 4, Symbol = "S5" },
                        new Reel() {Id = 5, Symbol = "S1" },
                        new Reel() {Id = 6, Symbol = "S1" },
                        new Reel() {Id = 7, Symbol = "Wild" },
                        new Reel() {Id = 8, Symbol = "Bonus" },
                        new Reel() {Id = 9, Symbol = "Bonus" },
                        new Reel() {Id = 10, Symbol = "S2" },
                        new Reel() {Id = 11, Symbol = "S2" },
                        new Reel() {Id = 12, Symbol = "S1" },
                        new Reel() {Id = 13, Symbol = "Wild" },
                        new Reel() {Id = 14, Symbol = "S6" },
                        new Reel() {Id = 15, Symbol = "S7" },
                        new Reel() {Id = 16, Symbol = "S2" },
                        new Reel() {Id = 17, Symbol = "S5" },
                        new Reel() {Id = 18, Symbol = "S2" },
                        new Reel() {Id = 19, Symbol = "Wild" },
                        new Reel() {Id = 20, Symbol = "S1" },
                        new Reel() {Id = 21, Symbol = "S1" },
                        new Reel() {Id = 22, Symbol = "S2" },
                        new Reel() {Id = 23, Symbol = "" },
                        new Reel() {Id = 24, Symbol = ""},
                        new Reel() {Id = 25, Symbol = ""}
                    };
                case 5:
                    return new List<Reel>
                    {
                        new Reel() {Id = 1, Symbol = "S5" },
                        new Reel() {Id = 2, Symbol = "S5" },
                        new Reel() {Id = 3, Symbol = "S6" },
                        new Reel() {Id = 4, Symbol = "S7" },
                        new Reel() {Id = 5, Symbol = "S2" },
                        new Reel() {Id = 6, Symbol = "S3" },
                        new Reel() {Id = 7, Symbol = "S3" },
                        new Reel() {Id = 8, Symbol = "S3" },
                        new Reel() {Id = 9, Symbol = "S2" },
                        new Reel() {Id = 10, Symbol = "S1" },
                        new Reel() {Id = 11, Symbol = "S1" },
                        new Reel() {Id = 12, Symbol = "S2" },
                        new Reel() {Id = 13, Symbol = "S3" },
                        new Reel() {Id = 14, Symbol = "S4" },
                        new Reel() {Id = 15, Symbol = "S5" },
                        new Reel() {Id = 16, Symbol = "S7" },
                        new Reel() {Id = 17, Symbol = "S1" },
                        new Reel() {Id = 18, Symbol = "S2" },
                        new Reel() {Id = 19, Symbol = "S4" },
                        new Reel() {Id = 20, Symbol = "S1" },
                        new Reel() {Id = 21, Symbol = "S1" },
                        new Reel() {Id = 22, Symbol = "S3" },
                        new Reel() {Id = 23, Symbol = "S4" },
                        new Reel() {Id = 24, Symbol = "S1"},
                        new Reel() {Id = 25, Symbol = "S2"}
                    };
                default:
                    return new List<Reel>();
            }
        }

        private int RandomPick()
        {
            var random = new Random();
            return random.Next(1, MaxLine);
        }

        private List<ReelResult> DoSpinPerReel(int reelNumber)
        {
            var reelResultPositions = new List<ReelResult>();

            var reelStrips = GetReelStrips(reelNumber);

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

        private List<ReelResult> CheckIfThereMatch(List<ReelResult> spinResult, decimal bet, string gameId, out int matchResult)
        {
            matchResult = 0;
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
                    }

                    if (match > 2)
                    {
                        matchResult = match;
                        var winAmount = GetWin(symbol, match, bet);
                        //TODO

                        _applicationDbContext.TransactionHistory.Add(new TransactionHistory()
                        {
                            Amount = winAmount,
                            PlayerId = 123,
                            Transaction = "Win"
                        });

                        _applicationDbContext.SaveChanges();
                        //Save game Id and winAmount
                        return result;
                    }
                } 
            }

            _applicationDbContext.TransactionHistory.Add(new TransactionHistory()
            {
                Amount = 0,
                PlayerId = 123,
                Transaction = "Lose"
            });

            _applicationDbContext.SaveChanges();

            return new List<ReelResult>();
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
