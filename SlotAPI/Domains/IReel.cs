using SlotAPI.Models;
using System.Collections.Generic;

namespace SlotAPI.Domains
{
    public interface IReel
    {
        List<ReelStrip> GetReelStrips(int reelNumber);
    }
}
