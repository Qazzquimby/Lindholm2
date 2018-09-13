using System;
using System.Collections.Generic;
using Lindholm.Bots;

namespace Lindholm.Slots
{
    internal class DeltinSlotObservation : IDeltinSlotObservation
    {
        public IBotDeltinObservation BotObservation { get; }
        private Func<List<int>> FilledSlotsFunction { get; }

        public DeltinSlotObservation(IBotDeltinObservation botObservation, Func<List<int>> filledSlotsFunction)
        {
            BotObservation = botObservation;
            FilledSlotsFunction = filledSlotsFunction;
        }

        public List<int> FilledSlots => FilledSlotsFunction();

    }
}
