using System.Collections.Generic;
using Lindholm.Slots;

namespace Lindholm.Bots
{
    internal class BotExpectations : IBotExpectations
    {
        private readonly IBotRequester _botRequester;
        private readonly ISlotManager _slots;

        internal BotExpectations(IBotRequester botRequester, ISlotManager slots)
        {
            _botRequester = botRequester;
            _slots = slots;
        }

        public Dictionary<Team, List<BotRequest>> Expectations { get; }
            = new Dictionary<Team, List<BotRequest>>
            {
                {Team.Blue, new List<BotRequest>()},
                {Team.Red, new List<BotRequest>()}
            };

        public Dictionary<Team, List<BotRequest>> PreviousExpectations { get; }
            = new Dictionary<Team, List<BotRequest>>
            {
                {Team.Blue, new List<BotRequest>()},
                {Team.Red, new List<BotRequest>()}
            };

        public void UpdateBotExpectations()
        {
            UpdateBotExpectations(Team.Blue);
            UpdateBotExpectations(Team.Red);
        }

        private void UpdateBotExpectations(Team team)
        {
            PreviousExpectations[team] = new List<BotRequest>(Expectations[team]);

            Expectations[team] = new List<BotRequest>();

            foreach (BotRequest request in _botRequester.BotRequests)
            {
                AddRequestIfRelevant(request, team);
            }
        }

        private void AddRequestIfRelevant(BotRequest request, Team team)
        {
            if(RequestIsRelevant(request, team))
            {
                Expectations[team].Add(request);
            }
        }

        private bool RequestIsRelevant(BotRequest request, Team team)
        {
            bool requestIsOnCorrectTeam = request.BotTeam == team;
            bool playerCountInRange = _slots.Players.Count(team) >= request.MinPlayersOnTeam && _slots.Players.Count(team) <= request.MaxPlayersOnTeam;
            bool ruleFollowed = GetRuleIsFollowed(request.Rule, team);
            return requestIsOnCorrectTeam && playerCountInRange && ruleFollowed;
        }

        private bool GetRuleIsFollowed(IBotRule rule, Team team)
        {
            return rule.FollowsRule(team, _slots);
        }

    }
}