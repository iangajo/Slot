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
        private readonly IAccountDetails _accountDetails;
        public Account(IAccountDetails accountDetails)
        {
            _accountDetails = accountDetails;
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

        public BaseResponse Register(string username, string password)
        {
            var response = new BaseResponse()
            {
                Success = true,
                ErrorMessage = string.Empty
            };

            try
            {
                var result = _accountDetails.Registration(username, password);
            }
            catch (Exception e)
            {
                response.ErrorMessage = e.Message;
                response.Success = false;
            }

            return response;

        }
    }
}
