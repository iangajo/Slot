using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SlotAPI.DataStore;
using SlotAPI.Models;

namespace SlotAPI.DataStores
{
    public class AccountWallet : IAccountCredits
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public AccountWallet(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public void Credit(int playerId, decimal amount)
        {
            var player = _applicationDbContext.AccountCredit.First(a => a.PlayerId == playerId);
            if (player != null)
            {
                player.Balance += amount;
                _applicationDbContext.SaveChanges();
            }
        }

        public void Debit(int playerId, decimal amount)
        {
            var player = _applicationDbContext.AccountCredit.First(a => a.PlayerId == playerId);
            if (player != null)
            {
                if (player.Balance >= amount)
                {
                    player.Balance -= amount;
                    _applicationDbContext.SaveChanges();
                }
            }
        }

        public decimal GetBalance(int playerId)
        {
            if (!_applicationDbContext.AccountCredit.Any(a => a.PlayerId == playerId)) return 0.00m;
            {
                var balance = _applicationDbContext.AccountCredit.First(a => a.PlayerId == playerId).Balance;

                return balance;
            }

        }
    }
}
