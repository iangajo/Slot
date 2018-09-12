using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Pages.Internal.Account;
using SlotAPI.DataStore;
using SlotAPI.Models;

namespace SlotAPI.DataStores
{
    public class AccountDetailsDataStore : IAccountDetailsDataStore
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public AccountDetailsDataStore(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public int Registration(string username, string password)
        {
            var account = new Account()
            {
                Username = username,
                Password = password
            };
            _applicationDbContext.Accounts.Add(account);

            _applicationDbContext.AccountCredit.Add(new AccountCredit()
            {
                Balance = 0
            });

            _applicationDbContext.SaveChanges();

            return account.PlayerId;
        }

        public void SaveToken(int playerId, string token)
        {
            if (_applicationDbContext.Accounts.Any(a => a.PlayerId == playerId))
            {
                var player = _applicationDbContext.Accounts.First(a => a.PlayerId == playerId);

                if (player == null) return;

                player.Token = token;

                _applicationDbContext.SaveChanges();
            }
        }

        public string GetToken(int playerId)
        {
            if (_applicationDbContext.Accounts.Any(a => a.PlayerId == playerId))
            {
                var player = _applicationDbContext.Accounts.First(a => a.PlayerId == playerId);

                return player.Token;
            }

            return string.Empty;
        }
    }
}
