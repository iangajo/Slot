using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SlotAPI.Models;

namespace SlotAPI.Domains.Impl
{
    public class Win : IWin
    {
        public int[] GetWinningCombinations(int winningCombination)
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

        public decimal GetWin(string symbol, int match, decimal bet)
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

        public List<Odds> GetOddsTable()
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
    }
}
