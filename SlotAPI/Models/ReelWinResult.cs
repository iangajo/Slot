using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlotAPI.Models
{
    public class ReelWinResult
    {
        public int ReelNumber { get; set; }
        public int Order { get; set; }
        public string Symbol { get; set; }
        public int WinCombination { get; set; }
    }
}
