namespace Lindholm.Slots
{
    public interface ISlotManager
    {
        AllSlots All { get; }
        IBotSlotManager BotSlotManager { get; }
        EmptySlots Empty { get; }
        FilledSlots Filled { get; }
        PlayerSlots Players { get; }
        void Update();
    }
}