using System.Collections.Generic;
using Lindholm.Slots;

namespace Lindholm.Bots
{
    internal class BotManipulation : IBotManipulation
    {
        private readonly ISlotManager _slots;
        private readonly IBotDeltinManipulator _deltin;

        public BotManipulation(ISlotManager slots, IBotDeltinManipulator deltin)
        {
            _slots = slots;
            _deltin = deltin;
        }

        public void AddBots(List<BotRequest> requests)
        {
            foreach (BotRequest request in requests)
            {
                AddBot(request);
            }
        }

        public void AddBot(BotRequest request)
        {
            AddBot(request.Hero, request.Difficulty, request.BotTeam);
        }

        public void AddBot(AiHero hero, Difficulty difficulty, Team team)
        {
            if (_slots.Empty.Count(team) > 0)
            {
                _slots.BotSlotManager.BotsModified = true;
                _deltin.AddAi(hero, difficulty, team);
            }
        }

        internal void RemoveBotsIfAny()
        {
            if (_slots.BotSlotManager.BotSlots.Count() > 0)
            {
                RemoveBots();
            }
        }

        public void RemoveBots()
        {
            _slots.BotSlotManager.BotsModified = true;
            _deltin.RemoveBots();
        }

        public void RemoveBots(Team team)
        {
            List<int> botSlots = _slots.BotSlotManager.BotSlots.Slots(team);
            foreach (int slot in botSlots)
            {
                //todo later if returns false repair state
                _slots.BotSlotManager.BotsModified = true;
                _deltin.RemoveBot(slot);
            }
        }
    }
}