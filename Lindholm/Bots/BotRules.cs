using Lindholm.Slots;

namespace Lindholm.Bots
{
    public interface IBotRule
    {
        bool FollowsRule(Team team, ISlotManager slots);
    }


    public class BotRuleSmallerTeam : IBotRule
    {
        bool IBotRule.FollowsRule(Team team, ISlotManager slots)
        {
            return slots.Players.TeamHasFewer(team);
        }
    }

    public class BotRuleEqualTeams : IBotRule
    {
        bool IBotRule.FollowsRule(Team team, ISlotManager slots)
        {
            return slots.Players.TeamsHaveEqualCount;
        }
    }

    public class BotRuleLargerTeam : IBotRule
    {
        bool IBotRule.FollowsRule(Team team, ISlotManager slots)
        {
            return slots.Players.TeamHasMore(team);
        }
    }

    public class BotRuleBothTeams : IBotRule
    {
        bool IBotRule.FollowsRule(Team team, ISlotManager slots)
        {
            return true;
        }
    }
}
