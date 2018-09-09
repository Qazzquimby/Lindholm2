using System.Collections.Generic;
using System.Linq;

namespace Lindholm.Slots
{
    public abstract class BaseSlots
    {
        private readonly ISlotContentHistory _history;

        internal BaseSlots(ISlotContentHistory history)
        {
            _history = history;
        }

        public bool TeamsHaveEqualCount => Count(Team.Blue) == Count(Team.Red);

        internal abstract bool SlotContentIsInCategory(SlotContent content);

        public List<int> Slots(Team team)
        {
            List<int> slots = new List<int>();
            foreach (int slot in SlotConstants.AllSlots(team))
            {
                SlotContent currentSlotContent = _history.Current(slot);
                if (SlotContentIsInCategory(currentSlotContent))
                {
                    slots.Add(slot);
                }
            }

            return slots; 
        }

        public List<int> Slots()
        {
            return Slots(Team.Blue).Union(Slots(Team.Red)).ToList();
        }

        public int Count()
        {
            return Slots().Count;
        }

        public int Count(Team team)
        {
            return Slots(team).Count;
        }

        public bool TeamHasMore(Team team)
        {
            if (team == Team.Blue)
            {
                return Count(Team.Blue) > Count(Team.Red);
            }

            return Count(Team.Red) > Count(Team.Blue);
        }

        public bool TeamHasFewer(Team team)
        {
            if (team == Team.Blue)
            {
                return Count(Team.Blue) < Count(Team.Red);
            }

            return Count(Team.Red) < Count(Team.Blue);
        }

        public Team TeamWithMoreOrEqual()
        {
            if (TeamHasMore(Team.Blue))
            {
                return Team.Blue;
            }

            return Team.Red;
        }
    }
}