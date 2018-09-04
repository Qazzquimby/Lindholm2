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

        public ChatManagerManager ChatBuilder(Deltin.CustomGameAutomation.Chat deltinChat)
        {
            ChatDeltinChannelSwapper deltinChannelSwapper = new ChatDeltinChannelSwapper(deltinChat);
            ChatChannelSwapper chatChannelSwapper = new ChatChannelSwapper(deltinChannelSwapper);

            ChatDecorator chatDecorator = new ChatDecorator();

            ChatDeltinPrinter deltinPrinter = new ChatDeltinPrinter(deltinChat);

            ChatManagerManager chat = new ChatManagerManager(chatChannelSwapper, chatDecorator, deltinPrinter);
            return chat;
        }

        public SlotsManager SlotBuilder()
        {
            SlotsManager slots = new SlotsManager();
            return slots;
        }

        public BotManager BotBuilder(SlotsManager slots)
        {
            BotRequester requester = new BotRequester();
            BotExpectations expectations = new BotExpectations(requester, slots);
            BotManager bots = new BotManager(requester, expectations);
            return bots;
        }

    }
}
