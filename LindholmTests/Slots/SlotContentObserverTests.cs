using System;
using System.Collections.Generic;
using Lindholm.Bots;
using Lindholm.Slots;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LindholmTests.Slots
{
    [TestClass()]
    public class SlotContentObserverTests
    {
        class FakeBotDeltinObservation : IBotDeltinObservation
        {
            public bool SafeIsAi(int slot)
            {
                return slot % 2 == 0;
            }

            public List<int> SafeGetBotSlots()
            {
                throw new NotImplementedException();
            }

            public bool FastIsAi(int slot)
            {
                throw new NotImplementedException();
            }

            public List<int> FastGetBotSlots()
            {
                throw new NotImplementedException();
            }

            public void CalibrateSafeIsAi()
            {
                
            }
        }

        class FakeDeltinObservations : IDeltinSlotObservation
        {
            public List<int> FilledSlots => new List<int>(){0, 1, 2, 3, 4, 6, 7, 8, 9, 10};
            public IBotDeltinObservation BotObservation { get; }

            public FakeDeltinObservations(IBotDeltinObservation botObservation)
            {
                BotObservation = botObservation;
            }
        }

        [TestMethod()]
        public void TestWhenBotsModifiedThenUpdatesBotLocations()
        {
            BotsModifiedFlag flag = new BotsModifiedFlag();
            flag.Flag();

            FakeBotDeltinObservation botDeltinObservation = new FakeBotDeltinObservation();
            FakeDeltinObservations observation = new FakeDeltinObservations(botDeltinObservation);

            Dictionary<int, List<SlotContent> > fakeHistory = new Dictionary<int, List<SlotContent>>()
            {
                {0, new List<SlotContent>(){ SlotContent.Player} },
                {2, new List<SlotContent>(){SlotContent.Player} },
                {4, new List<SlotContent>(){SlotContent.Player} },
                {6, new List<SlotContent>(){ SlotContent.Player} },
                {8, new List<SlotContent>(){SlotContent.Player} },
                {10, new List<SlotContent>(){SlotContent.Player} },

                { 1, new List<SlotContent>(){SlotContent.Bot} },
                {3, new List<SlotContent>(){SlotContent.Bot} },
                {7, new List<SlotContent>(){SlotContent.Bot} },
                {9, new List<SlotContent>(){SlotContent.Bot} },

                {5, new List<SlotContent>(){SlotContent.Empty} },
                {11,new List<SlotContent>(){ SlotContent.Empty} },
            };


            SlotContentObserver sut = new SlotContentObserver(flag, observation);

            List<SlotContent> result = sut.Observe(fakeHistory);
            
            Assert.IsTrue(result[0] == SlotContent.Bot);
            Assert.IsTrue(result[1] == SlotContent.Player);
            Assert.IsTrue(result[2] == SlotContent.Bot);
            Assert.IsTrue(result[3] == SlotContent.Player);
            Assert.IsTrue(result[4] == SlotContent.Bot);
            
            Assert.IsTrue(result[6] == SlotContent.Bot);
            Assert.IsTrue(result[7] == SlotContent.Player);
            Assert.IsTrue(result[8] == SlotContent.Bot);
            Assert.IsTrue(result[9] == SlotContent.Player);
            Assert.IsTrue(result[10] == SlotContent.Bot);
        }

        [TestMethod()]
        public void TestWhenBotsNotModifiedThenUsesOldBotLocations()
        {
            BotsModifiedFlag flag = new BotsModifiedFlag();
            FakeBotDeltinObservation botDeltinObservation = new FakeBotDeltinObservation();
            FakeDeltinObservations observation = new FakeDeltinObservations(botDeltinObservation);

            Dictionary<int, List<SlotContent>> fakeHistory = new Dictionary<int, List<SlotContent>>()
            {
                {0, new List<SlotContent>(){ SlotContent.Player} },
                {2, new List<SlotContent>(){SlotContent.Player} },
                {4, new List<SlotContent>(){SlotContent.Player} },
                {6, new List<SlotContent>(){ SlotContent.Player} },
                {8, new List<SlotContent>(){SlotContent.Player} },
                {10, new List<SlotContent>(){SlotContent.Player} },

                { 1, new List<SlotContent>(){SlotContent.Bot} },
                {3, new List<SlotContent>(){SlotContent.Bot} },
                {7, new List<SlotContent>(){SlotContent.Bot} },
                {9, new List<SlotContent>(){SlotContent.Bot} },

                {5, new List<SlotContent>(){SlotContent.Empty} },
                {11,new List<SlotContent>(){ SlotContent.Empty} },
            };


            SlotContentObserver sut = new SlotContentObserver(flag, observation);

            List<SlotContent> result = sut.Observe(fakeHistory);

            
            Assert.IsTrue(result[0] == SlotContent.Player);
            Assert.IsTrue(result[1] == SlotContent.Bot);
            Assert.IsTrue(result[2] == SlotContent.Player);
            Assert.IsTrue(result[3] == SlotContent.Bot);
            Assert.IsTrue(result[4] == SlotContent.Player);

            Assert.IsTrue(result[6] == SlotContent.Player);
            Assert.IsTrue(result[7] == SlotContent.Bot);
            Assert.IsTrue(result[8] == SlotContent.Player);
            Assert.IsTrue(result[9] == SlotContent.Bot);
            Assert.IsTrue(result[10] == SlotContent.Player);
        }


    }
}