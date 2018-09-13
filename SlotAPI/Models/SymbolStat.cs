using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SlotAPI.Models
{
    public class SymbolStat
    {
        [Key]
        public int Id { get; set; }

        public string Symbol { get; set; }

        public int Stat { get; set; } = 0;
    }
}
