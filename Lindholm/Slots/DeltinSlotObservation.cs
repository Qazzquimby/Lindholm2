using System;
using System.Collections.Generic;
using Lindholm.Bots;

namespace Lindholm.Slots
{
    internal class DeltinSlotObservation
    {
        private IBotDeltinObservation _botObservation;
        public Func<List<int>> FilledSlotsFunction { get; }

        public DeltinSlotObservation(IBotDeltinObservation botObservation, Func<List<int>> filledSlotsFunction)
        {
            _botObservation = botObservation;
            FilledSlotsFunction = filledSlotsFunction;
        }

        public List<int> FilledSlots => FilledSlotsFunction();

    }
}
