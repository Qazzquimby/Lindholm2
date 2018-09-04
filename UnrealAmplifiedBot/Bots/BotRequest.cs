using System;

namespace Lindholm.Bots
{
    internal class BotRequest : IEquatable<BotRequest>
    {
        public Team BotTeam;
        public AiHero Hero;
        public Difficulty Difficulty;
        public IBotRule Rule;
        public int MinPlayersOnTeam;
        public int MaxPlayersOnTeam;

        internal BotRequest(Team team, AiHero hero, Difficulty difficulty, IBotRule rule, int minPlayersOnTeam, int maxPlayersOnTeam)
        {
            if (minPlayersOnTeam < 0 || minPlayersOnTeam > 5)
            {
                throw (new ArgumentOutOfRangeException(nameof(minPlayersOnTeam), "Must be in range 0 - 5 inclusive."));
            }
            if (maxPlayersOnTeam < 0 || maxPlayersOnTeam > 5)
            {
                throw (new ArgumentOutOfRangeException(nameof(maxPlayersOnTeam),  "Must be in range 0 - 5 inclusive."));
            }
            if (minPlayersOnTeam > maxPlayersOnTeam)
            {
                throw (new ArgumentOutOfRangeException(nameof(minPlayersOnTeam), "Must not be greater than maxPlayers"));
            }

            BotTeam = team;
            Hero = hero;
            Difficulty = difficulty;
            Rule = rule;
            MinPlayersOnTeam = minPlayersOnTeam;
            MaxPlayersOnTeam = maxPlayersOnTeam;
        }

        public bool Equals(BotRequest other)
        {
            return other != null && (Hero == other.Hero && Difficulty == other.Difficulty);
        }
    }
}