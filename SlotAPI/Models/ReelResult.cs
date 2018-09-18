using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlotAPI.Models
{
    public class ReelResult
    {
        public int WheelNumber { get; set; }
        public int WheelIndex { get; set; }
        public string Symbol { get; set; }
        public int SymbolIndex { get; set; }
        
    }
}
