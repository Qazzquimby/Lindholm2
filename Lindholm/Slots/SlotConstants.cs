using System;
using System.Collections.Generic;

namespace Lindholm.Slots
{
    public static class SlotConstants
    {
        private static List<int> BlueSlots { get; } = new List<int> {0, 1, 2, 3, 4, 5};
        private static List<int> RedSlots { get; } = new List<int> {6, 7, 8, 9, 10, 11};

        public static List<int> AllSlots(Team team)
        {
            if (team == Team.Blue)
            {
                return BlueSlots;
            }

            return RedSlots;
        }

        public static Team TeamWithSlot(int slot)
        {
            if (BlueSlots.Contains(slot))
            {
                return Team.Blue;
            }

            if (RedSlots.Contains(slot))
            {
                return Team.Red;
            }

            throw new ArgumentOutOfRangeException(nameof(slot), "Must belong to red or blue.");
        }
    }
}