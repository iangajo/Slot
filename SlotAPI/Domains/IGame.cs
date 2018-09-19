using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SlotAPI.Models;

namespace SlotAPI.Domains
{
    public interface IGame
    {
        string[,] Spin(int playerId, decimal betAmount);

        string GenerateGameId();

    }
}
