using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SlotAPI.Models;

namespace SlotAPI.DataStore
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<AccountCredit> AccountCredit { get; set; }
        public DbSet<TransactionHistory> TransactionHistory { get; set; }
        public DbSet<Account> Accounts { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().HasIndex(a => a.Username).IsUnique();
        }
    }
}
