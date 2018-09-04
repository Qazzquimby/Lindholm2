using Lindholm.Slots;

namespace Lindholm.Bots
{
    public interface IBotRule
    {
        bool FollowsRule(Team team, SlotsManager slots);
    }


    public class BotRuleSmallerTeam : IBotRule
    {
        public bool FollowsRule(Team team, SlotsManager slots)
        {
            return slots.Players.TeamHasFewer(team);
        }
    }

    public class BotRuleEqualTeams : IBotRule
    {
        public bool FollowsRule(Team team, SlotsManager slots)
        {
            return slots.Players.TeamsHaveEqualCount;
        }
    }

    public class Larger : IBotRule
    {
        public bool FollowsRule(Team team, SlotsManager slots)
        {
            return slots.Players.TeamHasMore(team);
        }
    }

    public class Both : IBotRule
    {
        public bool FollowsRule(Team team, SlotsManager slots)
        {
            return true;
        }
    }
}
