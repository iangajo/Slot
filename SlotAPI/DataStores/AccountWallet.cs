using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        public BaseResponse Credit(int playerId, decimal amount)
        {
            var response = new BaseResponse()
            {
                ErrorMessage = string.Empty,
                Success = true
            };

            var player = _applicationDbContext.AccountCredit.First(a => a.PlayerId == playerId);
            if (player != null)
            {
                try
                {
                    player.Balance += amount;
                    _applicationDbContext.SaveChanges();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    response.ErrorMessage = e.Message;
                    response.Success = false;

                }
                catch (Exception e)
                {
                    response.ErrorMessage = e.Message;
                    response.Success = false;
                }

            }

            return response;
        }

        public BaseResponse Debit(int playerId, decimal amount)
        {
            var response = new BaseResponse()
            {
                ErrorMessage = string.Empty,
                Success = true
            };

            var player = _applicationDbContext.AccountCredit.First(a => a.PlayerId == playerId);
            if (player != null)
            {
                if (player.Balance >= amount)
                {
                    try
                    {
                        player.Balance -= amount;
                        _applicationDbContext.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException e)
                    {
                        response.ErrorMessage = e.Message;
                        response.Success = false;
                    }
                    catch (Exception e)
                    {
                        response.ErrorMessage = e.Message;
                        response.Success = false;
                    }

                }
            }

            return response;
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
