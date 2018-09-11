using System.Collections.Generic;
using Lindholm.Slots;

namespace Lindholm.Bots
{
    internal class BotManipulation : IBotManipulation
    {
        private readonly ISlotManager _slots;
        private readonly IBotDeltinManipulator _deltin;
        private readonly BotsModifiedFlag _modifiedFlag;

        public BotManipulation(ISlotManager slots, IBotDeltinManipulator deltin, BotsModifiedFlag modifiedFlag)
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
                _modifiedFlag.Flag();
                _deltin.AddAi(hero, difficulty, team);
            }
        }

        internal void RemoveBotsIfAny()
        {
            if (_slots.Bots.Count() > 0)
            {
                RemoveBots();
            }
        }

        public void RemoveBots()
        {
            _modifiedFlag.Flag();
            _deltin.RemoveBots();
        }

        public void RemoveBots(Team team)
        {
            List<int> botSlots = _slots.Bots.Slots(team);
            foreach (int slot in botSlots)
            {
                //todo later if returns false repair state
                _modifiedFlag.Flag();
                _deltin.RemoveBot(slot);
            }
        }
    }
}