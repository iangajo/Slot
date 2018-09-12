using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlotAPI.Models
{
    public class SpinRequest
    {
        public int PlayerId { get; set; }
        public decimal Bet { get; set; }
        public string AuthToken { get; set; }
    }
}
