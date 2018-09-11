using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlotAPI.Models
{
    public class Odds
    {
        public string Symbol { get; set; }
        public int FiveKind { get; set; }
        public int FourKind { get; set; }
        public int ThreeKind { get; set; }
        public int TwoKind { get; set; }
    }
}
