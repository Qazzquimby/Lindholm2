using System;
using System.Collections.Generic;
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

        public ISlotManager SlotBuilder(Deltin.CustomGameAutomation.AI ai, Func<List<int>> filledSlotsFunction, BotsModifiedFlag modifiedFlag)
        {
            BotDeltinObservation botDeltinObservation = new BotDeltinObservation(ai);
            DeltinSlotObservation deltinSlotObservation = new DeltinSlotObservation(botDeltinObservation, filledSlotsFunction);
            ISlotContentObserver observer = new SlotContentObserver(modifiedFlag, deltinSlotObservation);
            ISlotContentHistory history = new SlotContentHistory(observer);
            ISlotManager slots = new SlotManager(history);
            return slots;
        }

        public IBotManager BotBuilder(Deltin.CustomGameAutomation.AI ai, ISlotManager slots, BotsModifiedFlag modifiedFlag)
        {
            BotRequests requests = new BotRequests();
            IBotRequester requester = new BotRequester(requests);

            IBotExpectations expectations = new BotExpectations(requests, slots);
            IBotDeltinManipulator deltinManipulator = new BotDeltinManipulator(ai);
            
            IBotManipulation manipulation = new BotManipulation(slots, deltinManipulator, modifiedFlag);

            IBotCorruption corruption = new BotCorruption(slots.Bots, expectations);
            IBotRequestFulfillmentManager botRequestFulfillmentManager = new BotRequestFulfillmentManager(expectations, manipulation, corruption);

            IBotManager bots = new BotManager(requester, botRequestFulfillmentManager);
            return bots;
        }

    }
}
