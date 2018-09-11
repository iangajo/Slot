using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace SlotAPI.Models
{
    public class TransactionHistory
    {
        [Key]
        public int Id { get; set; }

        public int PlayerId { get; set; }

        public string GameId { get; set; }

        public string Transaction { get; set; }

        public decimal Amount { get; set; }
    }
}
