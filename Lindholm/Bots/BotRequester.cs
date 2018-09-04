using System.Collections.Generic;

namespace Lindholm.Bots
{
    public class BotRequester
    {
        internal List<BotRequest> BotRequests { get; private set; } = new List<BotRequest>();

        public void RequestBot(AiHero hero, Difficulty difficulty, IBotRule rule)
        {
            RequestBot(Team.Blue, hero, difficulty, rule);
            RequestBot(Team.Red, hero, difficulty, rule);
        }

        public void RequestBot(Team team, AiHero hero, Difficulty difficulty, IBotRule rule)
        {
            RequestBot(team, hero, difficulty, rule, 0, 5);
        }

        public void RequestBot(AiHero hero, Difficulty difficulty, IBotRule rule, int minPlayersOnTeam,
            int maxPlayersOnTeam)
        {
            RequestBot(Team.Blue, hero, difficulty, rule, minPlayersOnTeam, maxPlayersOnTeam);
            RequestBot(Team.Red, hero, difficulty, rule, minPlayersOnTeam, maxPlayersOnTeam);
        }


        public void RequestBot(Team team, AiHero hero, Difficulty difficulty, IBotRule rule, int minPlayersOnTeam,
            int maxPlayersOnTeam)
        {
            BotRequest newRequest = new BotRequest(team, hero, difficulty, rule, minPlayersOnTeam, maxPlayersOnTeam);
            BotRequests.Add(newRequest);
        }

        public void ClearBotRequests()
        {
            BotRequests = new List<BotRequest>();
        }
    }
}