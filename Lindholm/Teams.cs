using Deltin.CustomGameAutomation;
using System.Collections.ObjectModel;

namespace Lindholm
{
    public partial class Game
    {
        public Collection<Team> Teams { get; } = new Collection<Team>() { Team.Blue, Team.Red };
    }

    public static class TeamExtension
    {
        public static BotTeam ToBotTeam(this Team team)
        {
            if (team == Team.Blue)
            {
                return BotTeam.Blue;
            }
            else
            {
                return BotTeam.Red;
            }
        }

        public static Team Other(this Team team)
        {
            if (team == Team.Blue)
            {
                return Team.Red;
            }
            else
            {
                return Team.Blue;
            }
        }
    }

    public enum Team // For designer use in referring to teams. Doesn't mirror any in game use of teams, like BotTeam or InviteTeam. No value generally indicates both teams.
    {
        Blue,
        Red,
    }

}
