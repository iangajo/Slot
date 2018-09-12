using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SlotAPI.DataStores;
using SlotAPI.Models;

namespace SlotAPI.Domains.Impl
{
    public class Account : IAccount
    {
        private readonly IAccountDetailsDataStore _accountDetails;
        private readonly IAccountCreditsDataStore _accountCredits;

        public Account(IAccountDetailsDataStore accountDetails, IAccountCreditsDataStore accountCredits)
        {
            _accountDetails = accountDetails;
            _accountCredits = accountCredits;
        }

        public AuthResponse GenerateToken(int playerId)
        {
            var token = Guid.NewGuid().ToString();

            _accountDetails.SaveToken(playerId, token);

            return new AuthResponse()
            {
                Token = token,
            };
        }

        public RegistrationResponse Register(string username, string password)
        {
            var response = new RegistrationResponse()
            {
                Success = true,
                ErrorMessage = string.Empty
            };

            try
            {
                var playerId = _accountDetails.Registration(username, password);
                response.PlayerId = playerId;

            }
            catch (Exception e)
            {
                response.ErrorMessage = e.Message;
                response.Success = false;
            }

            return response;

        }

        public decimal GetBalance(int playerId)
        {
           return _accountCredits.GetBalance(playerId);
        }

        public string GetToken(int playerId)
        {
           return _accountDetails.GetToken(playerId);
        }
    }
}
