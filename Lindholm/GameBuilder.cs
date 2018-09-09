using Lindholm.Bots;
using Lindholm.Chat;
using Lindholm.Phases;
using Lindholm.Slots;

namespace Lindholm
{
    class GameBuilder : IGameBuilder
    {
        public Deltin.CustomGameAutomation.CustomGame CustomGameBuilder()
        {
            return new Deltin.CustomGameAutomation.CustomGame();
        }

        public PhaseManager PhaseManagerBuilder()
        {
            return new PhaseManager();
        }

        public GameLoop LoopBuilder(PhaseManager phaseManager)
        {
            return new GameLoop(phaseManager);
        }

        public IChatManager ChatBuilder(Deltin.CustomGameAutomation.Chat deltinChat)
        {
            IChatDeltinChannelSwapper deltinChannelSwapper = new ChatDeltinChannelSwapper(deltinChat);
            IChatChannelSwapper chatChannelSwapper = new ChatChannelSwapper(deltinChannelSwapper);

            IChatDecorator chatDecorator = new ChatDecorator();

            IChatDeltinPrinter deltinPrinter = new ChatDeltinPrinter(deltinChat);

            IChatManager chat = new ChatManager(chatChannelSwapper, chatDecorator, deltinPrinter);
            return chat;
        }

        public ISlotContentHistory SlotContentHistoryBuilder()
        {
            return new SlotContentHistory();
        }
        
        public ISlotManager SlotBuilder()
        {
            SlotContentHistory history = new SlotContentHistory();
            BotSlotManager botSlots = new BotSlotManager(history);
            SlotManager slots = new SlotManager(history, botSlots);
            return slots;
        }

        public IBotManager BotBuilder(Deltin.CustomGameAutomation.AI ai, ISlotManager slots)
        {
            IBotRequester requester = new BotRequester();

            IBotExpectations expectations = new BotExpectations(requester, slots);

            IBotDeltinManipulator deltinManipulator = new BotDeltinManipulator(ai);
            IBotManipulation manipulation = new BotManipulation(slots, deltinManipulator);
            IBotRequestFulfillmentManager botRequestFulfillmentManager = new BotRequestFulfillmentManager(expectations, manipulation, slots.BotSlotManager);

            IBotManager bots = new BotManager(requester, expectations, botRequestFulfillmentManager);
            return bots;
        }

    }
}
