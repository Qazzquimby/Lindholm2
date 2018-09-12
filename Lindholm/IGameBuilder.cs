using System;
using System.Collections.Generic;
using Lindholm.Bots;
using Lindholm.Chat;
using Lindholm.Loop;
using Lindholm.Phases;
using Lindholm.Slots;

namespace Lindholm
{
    internal interface IGameBuilder
    {
        Deltin.CustomGameAutomation.CustomGame CustomGameBuilder();
        PhaseManager PhaseManagerBuilder();
        GameLoop LoopBuilder(PhaseManager phaseManager);
        IChatManager ChatBuilder(Deltin.CustomGameAutomation.Chat deltinChat);
        ISlotManager SlotBuilder(Deltin.CustomGameAutomation.AI ai, Func<List<int>> filledSlotsFunction, BotsModifiedFlag modifiedFlag);
        IBotManager BotBuilder(Deltin.CustomGameAutomation.AI ai, ISlotManager slots, BotsModifiedFlag modifiedFlag);
    }
}