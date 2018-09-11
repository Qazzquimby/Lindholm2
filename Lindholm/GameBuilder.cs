using Lindholm.Bots;
using Lindholm.Chat;
using Lindholm.Loop;
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
        
        public ISlotManager SlotBuilder()
        {
            SlotContentObserver observer = new SlotContentObserver();
            ISlotContentHistory history = new SlotContentHistory(observer);
            ISlotManager slots = new SlotManager(history);
            return slots;
        }

        public IBotManager BotBuilder(Deltin.CustomGameAutomation.AI ai, ISlotManager slots)
        {
            IBotRequester requester = new BotRequester();

            IBotExpectations expectations = new BotExpectations(requester, slots);
            IBotDeltinManipulator deltinManipulator = new BotDeltinManipulator(ai);
            BotsModifiedFlag modifiedFlag = new BotsModifiedFlag();
            IBotManipulation manipulation = new BotManipulation(slots, deltinManipulator, modifiedFlag);

            IBotCorruption corruption = new BotCorruption(slots.Bots, expectations);
            IBotRequestFulfillmentManager botRequestFulfillmentManager = new BotRequestFulfillmentManager(expectations, manipulation, corruption);

            IBotManager bots = new BotManager(requester, botRequestFulfillmentManager);
            return bots;
        }

    }
}
