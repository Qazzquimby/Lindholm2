using System.Collections.Generic;
using Lindholm.Slots;

namespace Lindholm.Bots
{
    internal class BotManipulation
    {
        private readonly SlotsManager _slots;
        private readonly BotDeltinManipulator _deltin;

        public BotManipulation(SlotsManager slots)
        {
            _slots = slots;
        }

        internal void AddBot(BotRequest request)
        {
            AddBot(request.Hero, request.Difficulty, request.BotTeam);
        }

        internal void AddBot(AiHero hero, Difficulty difficulty, Team team)
        {
            if (_slots.Empty.Count(team) > 0)
            {
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

        internal void RemoveBots()
        {
            _deltin.RemoveBots();
        }

        internal void RemoveBots(Team team)
        {
            List<int> botSlots = _slots.Bots.Slots(team);
            foreach (int slot in botSlots)
            {
                //todolater if returns false repair state
                _deltin.RemoveBot(slot);
            }
        }
    }
}