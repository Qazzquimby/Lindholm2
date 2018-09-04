using System.Collections.Generic;
using System.Linq;

namespace Lindholm.Slots
{
    internal enum SlotContent
    {
        Empty,
        Player,
        Bot
    }

    internal class SlotContentHistory
    {
        private Dictionary<Team, List<List<SlotContent>>> _history = new Dictionary<Team, List<List<SlotContent>>>()
        {
            {Team.Blue, new List<List<SlotContent>>()},
            {Team.Red, new List<List<SlotContent>>()}
        };

        internal List<SlotContent> Current(Team team)
        {
            return _history[team][-1];
        }

        internal List<List<SlotContent>> History(Team team)
        {
            return _history[team];
        }

        internal void Update(List<SlotContent> blueSlots, List<SlotContent> redSlots)
        {
            _history[Team.Blue].Append(blueSlots);
            _history[Team.Red].Append(redSlots);
        }
    }
}