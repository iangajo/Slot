using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SlotAPI.Models;

namespace SlotAPI.Domains
{
    public interface IGame
    {
        List<ReelResult> Spin();

        string GenerateGameId();

        BaseResponse CheckIfPlayerWin(List<ReelResult> spinResults, decimal bet, int playerId);

    }
}
