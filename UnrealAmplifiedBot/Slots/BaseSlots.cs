using System.Collections.Generic;
using System.Linq;

namespace Lindholm.Slots
{
    public abstract class BaseSlots
    {
        private readonly SlotContentHistory _history;

        internal BaseSlots(SlotContentHistory history)
        {
            _history = history;
        }

        internal abstract bool SlotContentIsInCategory(SlotContent content);

        private List<int> BlueSlots => GetSlotsForTeam(Team.Blue);

        private List<int> RedSlots => GetSlotsForTeam(Team.Red);

        private List<int> GetSlotsForTeam(Team team)
        {
            List<int> slots = new List<int>();
            foreach (int slot in SlotConstants.AllSlots(team))
            {
                if (SlotContentIsInCategory(_history.Current(team)[slot]))
                {
                    slots.Append(slot);
                }
            }
            return slots;
        }

        public List<int> Slots()
        {
            return Slots(Team.Blue).Union(Slots(Team.Red)).ToList();
        }

        public List<int> Slots(Team team)
        {
            if (team == Team.Blue)
            {
                return BlueSlots;
            }
            return RedSlots;
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
            else
            {
                return Count(Team.Red) > Count(Team.Blue);
            }
        }

        public bool TeamHasFewer(Team team)
        {
            if (team == Team.Blue)
            {
                return Count(Team.Blue) < Count(Team.Red);
            }
            else
            {
                return Count(Team.Red) < Count(Team.Blue);
            }
        }

        public Team TeamWithMoreOrEqual()
        {
            if (TeamHasMore(Team.Blue))
            {
                return Team.Blue;
            }
            else
            {
                return Team.Red;
            }
        }

        public bool TeamsHaveEqualCount => Count(Team.Blue) == Count(Team.Red);
    }
}