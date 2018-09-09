using System.Collections.Generic;

namespace Lindholm.Bots
{
    public class BotRequester : IBotRequester
    {
        public List<BotRequest> BotRequests { get; private set; } = new List<BotRequest>();
        private readonly int _defaultMinPlayersOnTeam = 0;
        private readonly int _defaultMaxPlayersOnTeam = 5;

        internal BotRequester() { }

        public void RequestBot(AiHero hero, Difficulty difficulty, IBotRule rule)
        {
            RequestBot(Team.Blue, hero, difficulty, rule);
            RequestBot(Team.Red, hero, difficulty, rule);
        }

        public void RequestBot(Team team, AiHero hero, Difficulty difficulty, IBotRule rule)
        {
            RequestBot(team, hero, difficulty, rule, _defaultMinPlayersOnTeam, _defaultMaxPlayersOnTeam);
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