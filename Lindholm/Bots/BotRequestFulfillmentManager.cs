using System.Collections.Generic;
using Lindholm.Slots;

namespace Lindholm.Bots
{
    public class BotRequestFulfillmentManager : IBotRequestFulfillmentManager
    {
        private readonly IBotSlotManager _botSlots;
        private readonly IBotExpectations _expectations;
        private readonly IBotManipulation _manipulation;

        internal BotRequestFulfillmentManager(IBotExpectations expectations, IBotManipulation manipulation,
            IBotSlotManager botSlots)
        {
            _expectations = expectations;
            _manipulation = manipulation;
            _botSlots = botSlots;
        }

        public void FulfillBotExpectations()
        {
            FulfillBotExpectations(Team.Blue);
            FulfillBotExpectations(Team.Red);
        }

        private void FulfillBotExpectations(Team team)
        {
            LogChanges(team);

            if (BotStateIsCorrupt(team))
            {
                RemoveBotsAndRefulfill(team);
                return;
            }

            List<BotRequest> oldRequestsNotPresentInNewRequests =
                BotRequestSetSubtract(_expectations.PreviousExpectations[team], _expectations.Expectations[team]);
            if (oldRequestsNotPresentInNewRequests.Count > 0)
            {
                RemoveBotsAndRefulfill(team);
                return;
            }

            List<BotRequest> newRequestsNotPresentInOldRequests =
                BotRequestSetSubtract(_expectations.Expectations[team], _expectations.PreviousExpectations[team]);
            _manipulation.AddBots(newRequestsNotPresentInOldRequests);
        }

        private List<BotRequest> BotRequestSetSubtract(List<BotRequest> baseRequests,
            List<BotRequest> requestsToSubtract)
        {
            List<BotRequest> remainder = new List<BotRequest>(baseRequests);
            foreach (BotRequest requestToSubtract in requestsToSubtract)
            {
                foreach (BotRequest baseRequest in remainder)
                {
                    if (!baseRequest.Equals(requestToSubtract))
                    {
                        continue;
                    }

                    remainder.Remove(baseRequest);
                    break;
                }
            }

            return remainder;
        }

        private void RemoveBotsAndRefulfill(Team team)
        {
            _manipulation.RemoveBots(team);
            _manipulation.AddBots(_expectations.Expectations[team]);
        }

        private bool BotStateIsCorrupt(Team team)
        {
            int numExpectedBots = _expectations.PreviousExpectations[team].Count;
            int numActualBots = _botSlots.BotSlots.Count(team);

            return numExpectedBots != numActualBots;
        }

        private void LogChanges(Team team)
        {
            if (_expectations.Expectations[team].Count != _expectations.PreviousExpectations[team].Count)
            {
                Dev.Log(
                    $"{team.ToString()} bots previously {_expectations.PreviousExpectations[team].Count}, now {_expectations.Expectations[team].Count}.");
            }
        }
    }
}