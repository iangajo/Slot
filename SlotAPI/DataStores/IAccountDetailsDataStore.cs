using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SlotAPI.Models;

namespace SlotAPI.DataStores
{
    public interface IAccountDetailsDataStore
    {
        int Registration(string username, string password);

        void SaveToken(int playerId, string token);

        string GetToken(int playerId);
    }
}
