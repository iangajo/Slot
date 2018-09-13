using SlotAPI.DataStore;
using SlotAPI.Models;
using System.Linq;

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
                Balance = 100.00m
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
