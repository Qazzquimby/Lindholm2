using System.Collections.Generic;

namespace Lindholm.Bots
{
    public interface IBotRequester
    {
        void RequestBot(AiHero hero, Difficulty difficulty, IBotRule rule);
        void RequestBot(Team team, AiHero hero, Difficulty difficulty, IBotRule rule);

        void RequestBot(AiHero hero, Difficulty difficulty, IBotRule rule, int minPlayersOnTeam,
            int maxPlayersOnTeam);

        void RequestBot(Team team, AiHero hero, Difficulty difficulty, IBotRule rule, int minPlayersOnTeam,
            int maxPlayersOnTeam);

        void ClearBotRequests();
    }
}