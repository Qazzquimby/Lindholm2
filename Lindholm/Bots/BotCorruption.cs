using Lindholm.Slots;

namespace Lindholm.Bots
{
    class BotCorruption : IBotCorruption
    {
        private readonly BotSlots _slots;
        private readonly IBotExpectations _expectations;

        public BotCorruption(BotSlots slots, IBotExpectations expectations)
        {
            _slots = slots;
            _expectations = expectations;
        }

        public bool IsCorrupt(Team team)
        {
            int numExpectedBots = _expectations.PreviousExpectations[team].Count;
            int numActualBots = _slots.Count(team);

            return numExpectedBots != numActualBots;
        }
    }
}