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
        ISlotManager SlotBuilder();
        IBotManager BotBuilder(Deltin.CustomGameAutomation.AI ai, ISlotManager slots);
    }
}