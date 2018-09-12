using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Pages.Internal.Account;
using SlotAPI.DataStore;
using SlotAPI.Models;

namespace SlotAPI.DataStores
{
    public class AccountDetails : IAccountDetails
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public AccountDetails(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public bool Registration(string username, string password)
        {
            _applicationDbContext.Accounts.Add(new Account()
            {
                Username = username,
                Password = password
            });

            _applicationDbContext.AccountCredit.Add(new AccountCredit()
            {
                Balance = 0
            });

            var response = _applicationDbContext.SaveChanges();

            return response > 0;
        }

        public void SaveToken(int playerId, string token)
        {
            var player = _applicationDbContext.Accounts.First(a => a.PlayerId == playerId);

            if (player == null) return;

            player.Token = token;

            _applicationDbContext.SaveChanges();
        }
    }
}
