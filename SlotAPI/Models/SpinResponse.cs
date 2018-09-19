using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlotAPI.Models
{
    public class SpinResponse : BaseResponse
    {
        public string Transaction { get; set; }
        public decimal Balance { get; set; }
        public string[,] SpinResult { get; set; }
    }
}
