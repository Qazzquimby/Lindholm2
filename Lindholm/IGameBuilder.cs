using Deltin.CustomGameAutomation;
using Lindholm.Bots;
using Lindholm.Chat;
using Lindholm.Phases;
using Lindholm.Slots;

namespace Lindholm
{
    interface IGameBuilder
    {
        ChatManagerManager ChatBuilder(Deltin.CustomGameAutomation.Chat deltinChat);
        CustomGame CustomGameBuilder();
        GameLoop LoopBuilder(PhaseManager phaseManager);
        PhaseManager PhaseManagerBuilder();
        SlotsManager SlotBuilder();
        BotManager BotBuilder(SlotsManager slots);
    }
}