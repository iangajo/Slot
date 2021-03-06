﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace SlotAPI.Models
{
    public class AccountCredit
    {
        [Key]
        public int PlayerId { get; set; }

        [Required]
        [ConcurrencyCheck]
        public decimal Balance { get; set; }

        public int BonusSpin { get; set; } = 0;

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
