namespace Lindholm.Slots
{
    class BotSlotManager : IBotSlotManager
    {
        public bool BotsModified { get; set; }
        public BotSlots BotSlots { get; }

        internal BotSlotManager(ISlotContentHistory history)
        {
            BotSlots = new BotSlots(history);
        }


    }

    internal class SlotManager : ISlotManager
    {
        private readonly ISlotContentHistory _history;
        
        public IBotSlotManager BotSlotManager { get; }
        public AllSlots All { get; }
        public EmptySlots Empty { get; }
        public FilledSlots Filled { get; }
        public PlayerSlots Players { get; }

        public SlotManager(ISlotContentHistory history, IBotSlotManager botSlotManager)
        {
            _history = history;
            BotSlotManager = botSlotManager;
            All = new AllSlots(_history);
            Filled = new FilledSlots(_history);
            Empty = new EmptySlots(_history);
            Players = new PlayerSlots(_history);
        }

        public void Update()
        {
            BotSlotManager.BotsModified = false;
        }
    }
}