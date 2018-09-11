namespace Lindholm.Slots
{
    internal class SlotManager : ISlotManager
    {
        private readonly ISlotContentHistory _history;
        
        public BotSlots Bots { get; }
        public AllSlots All { get; }
        public EmptySlots Empty { get; }
        public FilledSlots Filled { get; }
        public PlayerSlots Players { get; }

        public SlotManager(ISlotContentHistory history)
        {
            _history = history;
            Bots = new BotSlots(_history);
            All = new AllSlots(_history);
            Filled = new FilledSlots(_history);
            Empty = new EmptySlots(_history);
            Players = new PlayerSlots(_history);
        }
    }
}