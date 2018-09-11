using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlotAPI.Models
{
    public class ReelResult
    {
        public int Position { get; set; }
        public string Symbol { get; set; }
        public int Order { get; set; }
        public int ReelNumber { get; set; }
    }
}
