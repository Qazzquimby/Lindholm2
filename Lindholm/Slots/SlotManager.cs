namespace Lindholm.Slots
{
    internal class SlotManager : ISlotManager
    {
        public BotSlots Bots { get; }
        public AllSlots All { get; }
        public EmptySlots Empty { get; }
        public FilledSlots Filled { get; }
        public PlayerSlots Players { get; }

        public SlotManager(ISlotContentHistory history)
        {
            Bots = new BotSlots(history);
            All = new AllSlots(history);
            Filled = new FilledSlots(history);
            Empty = new EmptySlots(history);
            Players = new PlayerSlots(history);
        }
    }
}