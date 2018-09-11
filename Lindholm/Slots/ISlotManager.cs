namespace Lindholm.Slots
{
    public interface ISlotManager
    {
        AllSlots All { get; }
        BotSlots Bots { get; }
        EmptySlots Empty { get; }
        FilledSlots Filled { get; }
        PlayerSlots Players { get; }
    }
}