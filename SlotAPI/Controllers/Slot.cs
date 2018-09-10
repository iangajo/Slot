using System;
using Microsoft.AspNetCore.Mvc;
using SlotAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace SlotAPI.Controllers
{

    [ApiController]
    public class Slot : ControllerBase
    {
        private const int MaxReel = 5;
        private const int MaxLine = 25;

        [HttpPost]
        [Route("api/spin")]
        public ActionResult Spin()
        {
            var result = new List<ReelResult>();

            for (var i = 1; i < MaxReel; i++)
            {
                var r = DoSpinPerReel(i);
                result.AddRange(r);

            }

            return Ok(result);
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
                    Position = position,
                    Symbol = reelStrips.First(r => r.Id == spinResult).Symbol
                });

                position += 1;

                reelResultPositions.Add(new ReelResult()
                {
                    Position = (position > MaxLine) ? (position - MaxLine) : position,
                    Symbol = reelStrips.First(r => r.Id == ((position > MaxLine) ? (MaxLine - position) + 1 : position)).Symbol
                });

                position += 1;

                reelResultPositions.Add(new ReelResult()
                {
                    Position = (position > MaxLine) ? (position - MaxLine) : position,
                    Symbol = reelStrips.First(r => r.Id == ((position > MaxLine) ? (MaxLine - position) + 1 : position)).Symbol
                });
            }
            else
            {
                reelResultPositions.Add(new ReelResult()
                {
                    Position = reelNumber,
                    Symbol = reelStrips.First(r => r.Id == spinResult).Symbol
                });
                reelResultPositions.Add(new ReelResult()
                {
                    Position = reelNumber + MaxReel,
                    Symbol = reelStrips.First(r => r.Id == spinResult + MaxReel).Symbol
                });
                reelResultPositions.Add(new ReelResult()
                {
                    Position = reelNumber + MaxReel + MaxReel,
                    Symbol = reelStrips.First(r => r.Id == spinResult + MaxReel).Symbol
                });
            }

            return reelResultPositions;
        }

        private void CheckWinningCombinations(List<ReelResult> spinResult)
        {

        }

        private int[] GetWinningCombinations(int winningCombinations)
        {
            switch (winningCombinations)
            {
                case 1:
                    return new[] {6, 7, 8, 9, 10};
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
                    return new[] { 6, 7, 8, 9, 10 };
                case 9:
                    return new[] { 6, 7, 8, 9, 10 };
                case 10:
                    return new[] { 6, 7, 8, 9, 10 };
                case 11:
                    return new[] { 6, 7, 8, 9, 10 };
                case 12:
                    return new[] { 6, 7, 8, 9, 10 };
                case 13:
                    return new[] {6, 7, 8, 9, 10};
                case 14:
                    return new[] { 6, 7, 8, 9, 10 };
                case 15:
                    return new[] { 6, 7, 8, 9, 10 };
                case 17:
                    return new[] { 6, 7, 8, 9, 10 };
                case 18:
                    return new[] { 6, 7, 8, 9, 10 };
                case 19:
                    return new[] { 6, 7, 8, 9, 10 };
                case 20:
                    return new[] { 6, 7, 8, 9, 10 };
                case 21:
                    return new[] { 6, 7, 8, 9, 10 };
                case 22:
                    return new[] { 6, 7, 8, 9, 10 };
                case 23:
                    return new[] { 6, 7, 8, 9, 10 };
                case 24:
                    return new[] { 6, 7, 8, 9, 10 };
                case 25:
                    return new[] { 6, 7, 8, 9, 10 };
                case 26:
                    return new[] { 6, 7, 8, 9, 10 };
                case 27:
                    return new[] { 6, 7, 8, 9, 10 };
                case 28:
                    return new[] { 6, 7, 8, 9, 10 };
                case 29:
                    return new[] { 6, 7, 8, 9, 10 };
                case 30:
                    return new[] { 6, 7, 8, 9, 10 };
                default: return new int[] { };

            }
        }

        #endregion
    }
}
