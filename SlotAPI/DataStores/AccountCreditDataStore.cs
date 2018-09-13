using Microsoft.EntityFrameworkCore;
using SlotAPI.DataStore;
using SlotAPI.Models;
using System;
using System.Linq;

namespace SlotAPI.DataStores
{
    public class AccountCreditDataStore : IAccountCreditsDataStore
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public AccountCreditDataStore(ApplicationDbContext applicationDbContext)
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
                    _applicationDbContext.Entry(player).OriginalValues["Balance"] = player.Balance;
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
                ErrorMessage = "Can't find Player",
                Success = false
            };


            if (!_applicationDbContext.AccountCredit.Any(a => a.PlayerId == playerId)) return response;
            {
                var player = _applicationDbContext.AccountCredit.First(a => a.PlayerId == playerId);
                if (player.Balance >= amount)
                {
                    try
                    {
                        _applicationDbContext.Entry(player).OriginalValues["Balance"] = player.Balance;
                        player.Balance -= amount;
                        _applicationDbContext.SaveChanges();

                        response.ErrorMessage = string.Empty;
                        response.Success = true;

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
                else
                {
                    throw new Exception("Insufficient Balance.");
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

        public BaseResponse CreditBonusSpin(int playerId)
        {
            var response = new BaseResponse()
            {
                ErrorMessage = "Can't find Player",
                Success = false
            };

            if (_applicationDbContext.AccountCredit.Any(a => a.PlayerId == playerId)) return response;
            {
                try
                {
                    var player = _applicationDbContext.AccountCredit.First(a => a.PlayerId == playerId);
                    player.BonusSpin += 6;

                    _applicationDbContext.SaveChanges();

                    response.ErrorMessage = string.Empty;
                    response.Success = true;

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

        public BaseResponse DebitBonusSpin(int playerId)
        {
            var response = new BaseResponse()
            {
                ErrorMessage = "Can't find Player",
                Success = false
            };

            if (!_applicationDbContext.AccountCredit.Any(a => a.PlayerId == playerId)) return response;

            try
            {
                var player = _applicationDbContext.AccountCredit.First(a => a.PlayerId == playerId);
                player.BonusSpin -= 1;

                _applicationDbContext.SaveChanges();

                response.ErrorMessage = string.Empty;
                response.Success = true;

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

            return response;
        }

        public int GetPlayerSpinBonus(int playerId)
        {
            if (!_applicationDbContext.AccountCredit.Any(a => a.PlayerId == playerId)) return 0;

            var player = _applicationDbContext.AccountCredit.First(a => a.PlayerId == playerId);
            return player.BonusSpin;

        }
    }
}
