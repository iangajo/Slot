using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SlotAPI.Models;

namespace SlotAPI.Domains
{
    public interface IAccount
    {
        AuthResponse GenerateToken(int playerId);

        RegistrationResponse Register(string username, string password);

        decimal GetBalance(int playerId);

        string GetToken(int playerId);
    }
}
