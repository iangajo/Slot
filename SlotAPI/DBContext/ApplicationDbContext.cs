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

        public DbSet<PayLineStat> PayLineWinStat { get; set; }
        public DbSet<SymbolStat> SymbolWinStat { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().HasIndex(a => a.Username).IsUnique();

            modelBuilder.Entity<PayLineStat>().HasData(
                new PayLineStat() { Id = 1, Stat = 0 },
                new PayLineStat() { Id = 2, Stat = 0 },
                new PayLineStat() { Id = 3, Stat = 0 },
                new PayLineStat() { Id = 4, Stat = 0 },
                new PayLineStat() { Id = 5, Stat = 0 },
                new PayLineStat() { Id = 6, Stat = 0 },
                new PayLineStat() { Id = 7, Stat = 0 },
                new PayLineStat() { Id = 8, Stat = 0 },
                new PayLineStat() { Id = 9, Stat = 0 },
                new PayLineStat() { Id = 10, Stat = 0 },
                new PayLineStat() { Id = 11, Stat = 0 },
                new PayLineStat() { Id = 12, Stat = 0 },
                new PayLineStat() { Id = 13, Stat = 0 },
                new PayLineStat() { Id = 14, Stat = 0 },
                new PayLineStat() { Id = 15, Stat = 0 },
                new PayLineStat() { Id = 16, Stat = 0 },
                new PayLineStat() { Id = 17, Stat = 0 },
                new PayLineStat() { Id = 18, Stat = 0 },
                new PayLineStat() { Id = 19, Stat = 0 },
                new PayLineStat() { Id = 20, Stat = 0 },
                new PayLineStat() { Id = 21, Stat = 0 },
                new PayLineStat() { Id = 22, Stat = 0 },
                new PayLineStat() { Id = 23, Stat = 0 },
                new PayLineStat() { Id = 24, Stat = 0 },
                new PayLineStat() { Id = 25, Stat = 0 },
                new PayLineStat() { Id = 26, Stat = 0 },
                new PayLineStat() { Id = 27, Stat = 0 },
                new PayLineStat() { Id = 28, Stat = 0 },
                new PayLineStat() { Id = 29, Stat = 0 },
                new PayLineStat() { Id = 30, Stat = 0 }
                );

            modelBuilder.Entity<SymbolStat>().HasData(
                new SymbolStat() { Id = 1, Symbol = "S0", ThreeKind = 0,FourKind = 0, FiveKind = 0},
                new SymbolStat() { Id = 2, Symbol = "S1", ThreeKind = 0, FourKind = 0, FiveKind = 0 },
                new SymbolStat() { Id = 3, Symbol = "S2", ThreeKind = 0, FourKind = 0, FiveKind = 0 },
                new SymbolStat() { Id = 4, Symbol = "S3", ThreeKind = 0, FourKind = 0, FiveKind = 0 },
                new SymbolStat() { Id = 5, Symbol = "S4", ThreeKind = 0, FourKind = 0, FiveKind = 0 },
                new SymbolStat() { Id = 6, Symbol = "S5", ThreeKind = 0, FourKind = 0, FiveKind = 0 },
                new SymbolStat() { Id = 7, Symbol = "S6", ThreeKind = 0, FourKind = 0, FiveKind = 0 },
                new SymbolStat() { Id = 8, Symbol = "S7", ThreeKind = 0, FourKind = 0, FiveKind = 0 }
            );
        }
    }
}
