using System.Collections.Generic;

namespace Lindholm.Bots
{
    internal class BotFulfillment
    {
        private readonly BotExpectations _expectations;
        private readonly BotManipulation _manipulation;
        private readonly Deltin.CustomGameAutomation.AI _ai;

        internal BotFulfillment(Deltin.CustomGameAutomation.AI ai)
        {
            _ai = ai;
        }


        private List<BotRequest> _fulfilledBotExpectations;
        private List<BotRequest> _unfulfilledBotExpectations;
        private readonly Slots.SlotsManager _slots;

        internal BotFulfillment(BotExpectations expectations, BotManipulation manipulation, Slots.SlotsManager slots)
        {
            _expectations = expectations;
            _manipulation = manipulation;
            _slots = slots;
        }

        internal void FulfillBotExpectations()
        {
            FulfillBotExpectations(Team.Blue);
            FulfillBotExpectations(Team.Red);
            _slots.FlagBotsModified();
        }

        private void FulfillBotExpectations(Team team)
        {
            LogChanges(team);

            _fulfilledBotExpectations = new List<BotRequest>();
            _unfulfilledBotExpectations = new List<BotRequest>(_expectations.Expectations[team]);
            List<BotRequest> unprocessedOldBotRequests = new List<BotRequest>(_expectations.PreviousExpectations[team]);

            if (BotStateIsCorrupt)
            {
                RemoveBotsAndRefulfill(team);
            }
            else
            {
                ProcessOldBotRequests(team, unprocessedOldBotRequests);
                AddUnfulfilledBotRequests(_unfulfilledBotExpectations);
            }
        }

        private bool BotStateIsCorrupt
        {
            get
            {
                return false;
                //is state corrupt?
                //            if (IsBotStateCorrupt(team))
                //            {
                //                Dev.Log(
                //                    $"{team.ToString()} bots corrupted. Count is {wrapper.Slots.bots.Count(team)}, should be {PreviousExpectations[team].Count}");
                //                RemoveBots(team);
                //                foreach (BotRequest request in tempBotExpectations) AddBot(request);
                //            }
                //            else
                //            {

            }
        }

        private void LogChanges(Team team)
        {
            if (_expectations.Expectations[team].Count != _expectations.PreviousExpectations[team].Count)
                Dev.Log(
                    $"{team.ToString()} bots previously {_expectations.PreviousExpectations[team].Count}, now {_expectations.Expectations[team].Count}.");
        }

        private void ProcessOldBotRequests(Team team, List<BotRequest> oldBotRequests)
        {
            foreach (BotRequest oldRequest in oldBotRequests)
            {
                bool alreadyFulfilled = ProcessOldBotRequest(team, oldRequest);
                if (alreadyFulfilled)
                {
                    _unfulfilledBotExpectations.Remove(oldRequest);
                }
            }
        }

        private bool ProcessOldBotRequest(Team team, BotRequest oldBotRequest)
        {
            foreach (BotRequest newRequest in _unfulfilledBotExpectations)
            {
                if (oldBotRequest == newRequest)
                {
                    _fulfilledBotExpectations.Add(newRequest);
                    _unfulfilledBotExpectations.Remove(newRequest);
                    return true;
                }
            }

            RemoveBotsAndRefulfill(team);
            return false;
        }

        private void AddUnfulfilledBotRequests(List<BotRequest> unfulfilledBotRequests)
        {
            foreach (BotRequest request in _unfulfilledBotExpectations) _manipulation.AddBot(request);
        }
        


        private void RemoveBotsAndRefulfill(Team team)
        {
            _manipulation.RemoveBots(team);
            foreach (BotRequest request in _unfulfilledBotExpectations) _manipulation.AddBot(request);
            foreach (BotRequest request in _fulfilledBotExpectations) _manipulation.AddBot(request);
        }
    }
}