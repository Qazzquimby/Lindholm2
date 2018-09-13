using System;
using System.Collections.Generic;
using Lindholm.Bots;

namespace Lindholm.Slots
{
    internal interface IDeltinSlotObservation
    {
        List<int> FilledSlots { get; }
        IBotDeltinObservation BotObservation { get; }
    }
}