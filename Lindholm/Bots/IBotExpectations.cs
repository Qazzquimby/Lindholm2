using System.Collections.Generic;

namespace Lindholm.Bots
{
    internal interface IBotExpectations
    {
        Dictionary<Team, List<BotRequest>> Expectations { get; }
        Dictionary<Team, List<BotRequest>> PreviousExpectations { get; }
        void UpdateBotExpectations();
    }
}