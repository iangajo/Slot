using SlotAPI.Models;
using System.Collections.Generic;

namespace SlotAPI.Domains.Impl
{
    public class Reel : IReel
    {
      public List<ReelStrip> GetReelWheel(int reelNumber)
        {
            switch (reelNumber)
            {
                case 0:
                    return new List<ReelStrip>
                    {
                        new ReelStrip() {Id = 1, Symbol = "S0" },
                        new ReelStrip() {Id = 2, Symbol = "S1" },
                        new ReelStrip() {Id = 3, Symbol = "S0" },
                        new ReelStrip() {Id = 4, Symbol = "S2" },
                        new ReelStrip() {Id = 5, Symbol = "S3" },
                        new ReelStrip() {Id = 6, Symbol = "S1" },
                        new ReelStrip() {Id = 7, Symbol = "S3" },
                        new ReelStrip() {Id = 8, Symbol = "S0" },
                        new ReelStrip() {Id = 9, Symbol = "S0" },
                        new ReelStrip() {Id = 10, Symbol = "S2" },
                        new ReelStrip() {Id = 11, Symbol = "S4" },
                        new ReelStrip() {Id = 12, Symbol = "S2" },
                        new ReelStrip() {Id = 13, Symbol = "S1" },
                        new ReelStrip() {Id = 14, Symbol = "S1" },
                        new ReelStrip() {Id = 15, Symbol = "S4" },
                        new ReelStrip() {Id = 16, Symbol = "S2" },
                        new ReelStrip() {Id = 17, Symbol = "S5" },
                        new ReelStrip() {Id = 18, Symbol = "S6" },
                        new ReelStrip() {Id = 19, Symbol = "S7" },
                        new ReelStrip() {Id = 20, Symbol = "Bonus" },
                        new ReelStrip() {Id = 21, Symbol = "S6" },
                        new ReelStrip() {Id = 22, Symbol = "S0" },
                        new ReelStrip() {Id = 23, Symbol = "S5" },
                        new ReelStrip() {Id = 24, Symbol = "Bonus "},
                        new ReelStrip() {Id = 25, Symbol = "S0 "}
                    };
                case 1:
                    return new List<ReelStrip>
                    {
                        new ReelStrip() {Id = 1, Symbol = "S0" },
                        new ReelStrip() {Id = 2, Symbol = "S1" },
                        new ReelStrip() {Id = 3, Symbol = "S2" },
                        new ReelStrip() {Id = 4, Symbol = "S0" },
                        new ReelStrip() {Id = 5, Symbol = "S0" },
                        new ReelStrip() {Id = 6, Symbol = "S3" },
                        new ReelStrip() {Id = 7, Symbol = "S4" },
                        new ReelStrip() {Id = 8, Symbol = "S1" },
                        new ReelStrip() {Id = 9, Symbol = "S5" },
                        new ReelStrip() {Id = 10, Symbol = "Bonus" },
                        new ReelStrip() {Id = 11, Symbol = "S1" },
                        new ReelStrip() {Id = 12, Symbol = "Bonus" },
                        new ReelStrip() {Id = 13, Symbol = "S6" },
                        new ReelStrip() {Id = 14, Symbol = "S5" },
                        new ReelStrip() {Id = 15, Symbol = "S0" },
                        new ReelStrip() {Id = 16, Symbol = "S0" },
                        new ReelStrip() {Id = 17, Symbol = "S0" },
                        new ReelStrip() {Id = 18, Symbol = "S2" },
                        new ReelStrip() {Id = 19, Symbol = "S1" },
                        new ReelStrip() {Id = 20, Symbol = "S7" },
                        new ReelStrip() {Id = 21, Symbol = "S0" },
                        new ReelStrip() {Id = 22, Symbol = "Bonus" },
                        new ReelStrip() {Id = 23, Symbol = "S1" },
                        new ReelStrip() {Id = 24, Symbol = "S4"},
                        new ReelStrip() {Id = 25, Symbol = "Wild"}
                    };
                case 2:
                    return new List<ReelStrip>
                    {
                        new ReelStrip() {Id = 1, Symbol = "S0" },
                        new ReelStrip() {Id = 2, Symbol = "S2" },
                        new ReelStrip() {Id = 3, Symbol = "S3" },
                        new ReelStrip() {Id = 4, Symbol = "S4" },
                        new ReelStrip() {Id = 5, Symbol = "S1" },
                        new ReelStrip() {Id = 6, Symbol = "S1" },
                        new ReelStrip() {Id = 7, Symbol = "S5" },
                        new ReelStrip() {Id = 8, Symbol = "S7" },
                        new ReelStrip() {Id = 9, Symbol = "S1" },
                        new ReelStrip() {Id = 10, Symbol = "Bonus" },
                        new ReelStrip() {Id = 11, Symbol = "S2" },
                        new ReelStrip() {Id = 12, Symbol = "S1" },
                        new ReelStrip() {Id = 13, Symbol = "Bonus" },
                        new ReelStrip() {Id = 14, Symbol = "Wild" },
                        new ReelStrip() {Id = 15, Symbol = "Wild" },
                        new ReelStrip() {Id = 16, Symbol = "S2" },
                        new ReelStrip() {Id = 17, Symbol = "S2" },
                        new ReelStrip() {Id = 18, Symbol = "S1" },
                        new ReelStrip() {Id = 19, Symbol = "S1" },
                        new ReelStrip() {Id = 20, Symbol = "Bonus" },
                        new ReelStrip() {Id = 21, Symbol = "S1" },
                        new ReelStrip() {Id = 22, Symbol = "S3" },
                        new ReelStrip() {Id = 23, Symbol = "S3" },
                        new ReelStrip() {Id = 24, Symbol = ""},
                        new ReelStrip() {Id = 25, Symbol = ""}
                    };
                case 3:
                    return new List<ReelStrip>
                    {
                        new ReelStrip() {Id = 1, Symbol = "S3" },
                        new ReelStrip() {Id = 2, Symbol = "S3" },
                        new ReelStrip() {Id = 3, Symbol = "S4" },
                        new ReelStrip() {Id = 4, Symbol = "S5" },
                        new ReelStrip() {Id = 5, Symbol = "S1" },
                        new ReelStrip() {Id = 6, Symbol = "S1" },
                        new ReelStrip() {Id = 7, Symbol = "Wild" },
                        new ReelStrip() {Id = 8, Symbol = "Bonus" },
                        new ReelStrip() {Id = 9, Symbol = "Bonus" },
                        new ReelStrip() {Id = 10, Symbol = "S2" },
                        new ReelStrip() {Id = 11, Symbol = "S2" },
                        new ReelStrip() {Id = 12, Symbol = "S1" },
                        new ReelStrip() {Id = 13, Symbol = "Wild" },
                        new ReelStrip() {Id = 14, Symbol = "S6" },
                        new ReelStrip() {Id = 15, Symbol = "S7" },
                        new ReelStrip() {Id = 16, Symbol = "S2" },
                        new ReelStrip() {Id = 17, Symbol = "S5" },
                        new ReelStrip() {Id = 18, Symbol = "S2" },
                        new ReelStrip() {Id = 19, Symbol = "Wild" },
                        new ReelStrip() {Id = 20, Symbol = "S1" },
                        new ReelStrip() {Id = 21, Symbol = "S1" },
                        new ReelStrip() {Id = 22, Symbol = "S2" },
                        new ReelStrip() {Id = 23, Symbol = "" },
                        new ReelStrip() {Id = 24, Symbol = ""},
                        new ReelStrip() {Id = 25, Symbol = ""}
                    };
                case 4:
                    return new List<ReelStrip>
                    {
                        new ReelStrip() {Id = 1, Symbol = "S5" },
                        new ReelStrip() {Id = 2, Symbol = "S5" },
                        new ReelStrip() {Id = 3, Symbol = "S6" },
                        new ReelStrip() {Id = 4, Symbol = "S7" },
                        new ReelStrip() {Id = 5, Symbol = "S2" },
                        new ReelStrip() {Id = 6, Symbol = "S3" },
                        new ReelStrip() {Id = 7, Symbol = "S3" },
                        new ReelStrip() {Id = 8, Symbol = "S3" },
                        new ReelStrip() {Id = 9, Symbol = "S2" },
                        new ReelStrip() {Id = 10, Symbol = "S1" },
                        new ReelStrip() {Id = 11, Symbol = "S1" },
                        new ReelStrip() {Id = 12, Symbol = "S2" },
                        new ReelStrip() {Id = 13, Symbol = "S3" },
                        new ReelStrip() {Id = 14, Symbol = "S4" },
                        new ReelStrip() {Id = 15, Symbol = "S5" },
                        new ReelStrip() {Id = 16, Symbol = "S7" },
                        new ReelStrip() {Id = 17, Symbol = "S1" },
                        new ReelStrip() {Id = 18, Symbol = "S2" },
                        new ReelStrip() {Id = 19, Symbol = "S4" },
                        new ReelStrip() {Id = 20, Symbol = "S1" },
                        new ReelStrip() {Id = 21, Symbol = "S1" },
                        new ReelStrip() {Id = 22, Symbol = "S3" },
                        new ReelStrip() {Id = 23, Symbol = "S4" },
                        new ReelStrip() {Id = 24, Symbol = "S1"},
                        new ReelStrip() {Id = 25, Symbol = "S2"}
                    };
                default:
                    return new List<ReelStrip>();
            }
        }
    }
}
