using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SlotAPI.Models
{
    public class PayLineStat
    {
        [Key]
        public int Id { get; set; }

        public int Stat { get; set; } = 0;
    }
}
