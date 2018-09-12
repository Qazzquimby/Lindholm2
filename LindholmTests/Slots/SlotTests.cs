using System;
using System.Collections.Generic;
using System.Linq;
using Lindholm;
using Lindholm.Slots;
using LindholmTests.Bots;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LindholmTests.Slots
{
    [TestClass]
    public class SlotTests
    {
        class FakeEmptyContentHistory : ISlotContentHistory
        {
            public SlotContent Current(int slot)
            {
                throw new NotImplementedException();
            }

            public List<SlotContent> History(int slot)
            {
                throw new NotImplementedException();
            }

            public void Update()
            {
                throw new NotImplementedException();
            }

            public void PurgeHistory()
            {
                throw new NotImplementedException();
            }
        }


        [TestMethod]
        public void TestPlayersContentInPlayersSlots()
        {
            PlayerSlots sut = new PlayerSlots(new FakeEmptyContentHistory());

            Assert.IsFalse(sut.SlotContentIsInCategory(SlotContent.Empty));
            Assert.IsFalse(sut.SlotContentIsInCategory(SlotContent.Bot));
            Assert.IsTrue(sut.SlotContentIsInCategory(SlotContent.Player));
        }

        [TestMethod]
        public void TestBotContentInBotSlots()
        {
            BotSlots sut = new BotSlots(new FakeEmptyContentHistory());

            Assert.IsFalse(sut.SlotContentIsInCategory(SlotContent.Player));
            Assert.IsFalse(sut.SlotContentIsInCategory(SlotContent.Empty));
            Assert.IsTrue(sut.SlotContentIsInCategory(SlotContent.Bot));
        }

        [TestMethod]
        public void TestNoContentInEmptySlots()
        {
            EmptySlots sut = new EmptySlots(new FakeEmptyContentHistory());

            Assert.IsFalse(sut.SlotContentIsInCategory(SlotContent.Player));
            Assert.IsFalse(sut.SlotContentIsInCategory(SlotContent.Bot));
            Assert.IsTrue(sut.SlotContentIsInCategory(SlotContent.Empty));
        }

        [TestMethod]
        public void TestPlayerAndBotContentInFilledSlots()
        {
            FilledSlots sut = new FilledSlots(new FakeEmptyContentHistory());

            Assert.IsTrue(sut.SlotContentIsInCategory(SlotContent.Player));
            Assert.IsTrue(sut.SlotContentIsInCategory(SlotContent.Bot));
            Assert.IsFalse(sut.SlotContentIsInCategory(SlotContent.Empty));
        }

        [TestMethod]
        public void TestAllContentInAllSlots()
        {
            AllSlots sut = new AllSlots(new FakeEmptyContentHistory());

            Assert.IsTrue(sut.SlotContentIsInCategory(SlotContent.Player));
            Assert.IsTrue(sut.SlotContentIsInCategory(SlotContent.Bot));
            Assert.IsTrue(sut.SlotContentIsInCategory(SlotContent.Empty));
        }

        [TestMethod]
        public void TestAllSlotsHoldsEverySlot()
        {
            ISlotContentObserver observer = new FakeSlotContentObserver();
            SlotContentHistory history = new SlotContentHistory(observer);
            AllSlots sut = new AllSlots(history);

            List<SlotContent> blueSlots = new List<SlotContent>()
            {
                SlotContent.Empty, SlotContent.Empty, SlotContent.Bot, SlotContent.Player, SlotContent.Empty,
                SlotContent.Empty
            };
            List<SlotContent> redSlots =  new List<SlotContent>()
            {
                SlotContent.Bot, SlotContent.Bot, SlotContent.Bot, SlotContent.Player, SlotContent.Player, SlotContent.Empty
            };

            history.Update();

            List<int> blueResult = sut.Slots(Team.Blue);
            List<int> redResult = sut.Slots(Team.Red);

            List<int> blueExpectation = new List<int>() {0, 1, 2, 3, 4, 5};
            List<int> redExpectation = new List<int>() {6, 7, 8, 9, 10, 11};

            Assert.IsTrue(blueResult.SequenceEqual(blueExpectation));
            Assert.IsTrue(redResult.SequenceEqual((redExpectation)));
        }

        [TestMethod]
        public void TestPlayerSlots()
        {
            FakeSlotContentObserver observer = new FakeSlotContentObserver();
            SlotContentHistory history = new SlotContentHistory(observer);
            PlayerSlots sut = new PlayerSlots(history);

            List<SlotContent> blueSlots = new List<SlotContent>()
            {
                SlotContent.Empty, SlotContent.Empty, SlotContent.Bot, SlotContent.Player, SlotContent.Empty,
                SlotContent.Empty
            };
            List<SlotContent> redSlots = new List<SlotContent>()
            {
                SlotContent.Bot, SlotContent.Bot, SlotContent.Bot, SlotContent.Player, SlotContent.Player, SlotContent.Empty
            };
            observer.SetObserve(blueSlots, redSlots);

            history.Update();

            List<int> blueResult = sut.Slots(Team.Blue);
            List<int> redResult = sut.Slots(Team.Red);

            List<int> blueExpectation = new List<int>() { 3 };
            List<int> redExpectation = new List<int>() { 9, 10 };

            Assert.IsTrue(blueResult.SequenceEqual(blueExpectation));
            Assert.IsTrue(redResult.SequenceEqual((redExpectation)));
        }

        [TestMethod]
        public void TestWhenPlayerCountDifferentThenTeamsHaveEqualCountIsFalse()
        {
            FakeSlotContentObserver observer = new FakeSlotContentObserver();
            SlotContentHistory history = new SlotContentHistory(observer);
            PlayerSlots sut = new PlayerSlots(history);

            List<SlotContent> blueSlots = new List<SlotContent>()
            {
                SlotContent.Empty, SlotContent.Empty, SlotContent.Bot, SlotContent.Player, SlotContent.Empty,
                SlotContent.Empty
            };
            List<SlotContent> redSlots = new List<SlotContent>()
            {
                SlotContent.Bot, SlotContent.Bot, SlotContent.Bot, SlotContent.Player, SlotContent.Player, SlotContent.Empty
            };
            observer.SetObserve(blueSlots, redSlots);

            history.Update();

            Assert.IsFalse(sut.TeamsHaveEqualCount);
        }

        [TestMethod]
        public void TestWhenPlayerCountSameThenTeamsHaveEqualCountIsTrue()
        {
            FakeSlotContentObserver observer = new FakeSlotContentObserver();
            SlotContentHistory history = new SlotContentHistory(observer);
            PlayerSlots sut = new PlayerSlots(history);

            List<SlotContent> blueSlots = new List<SlotContent>()
            {
                SlotContent.Empty, SlotContent.Empty, SlotContent.Bot, SlotContent.Player, SlotContent.Empty,
                SlotContent.Empty
            };
            List<SlotContent> redSlots = new List<SlotContent>()
            {
                SlotContent.Bot, SlotContent.Bot, SlotContent.Bot, SlotContent.Player, SlotContent.Bot, SlotContent.Empty
            };
            observer.SetObserve(blueSlots, redSlots);

            history.Update();

            Assert.IsTrue(sut.TeamsHaveEqualCount);
        }


        [TestMethod]
        public void TestWhenBlueHasFewerBotsThenBlueHasMoreBotsIsFalse()
        {
            FakeSlotContentObserver observer = new FakeSlotContentObserver();
            SlotContentHistory history = new SlotContentHistory(observer);
            BotSlots sut = new BotSlots(history);

            List<SlotContent> blueSlots = new List<SlotContent>()
            {
                SlotContent.Empty, SlotContent.Empty, SlotContent.Bot, SlotContent.Player, SlotContent.Empty,
                SlotContent.Empty
            };
            List<SlotContent> redSlots = new List<SlotContent>()
            {
                SlotContent.Bot, SlotContent.Bot, SlotContent.Bot, SlotContent.Player, SlotContent.Player, SlotContent.Empty
            };
            observer.SetObserve(blueSlots, redSlots);

            history.Update();

            Assert.IsFalse(sut.TeamHasMore(Team.Blue));
        }

        [TestMethod]
        public void TestWhenRedHasMoreBotsThenRedHasMoreBotsIsTrue()
        {
            FakeSlotContentObserver observer = new FakeSlotContentObserver();
            SlotContentHistory history = new SlotContentHistory(observer);
            BotSlots sut = new BotSlots(history);

            List<SlotContent> blueSlots = new List<SlotContent>()
            {
                SlotContent.Empty, SlotContent.Empty, SlotContent.Bot, SlotContent.Player, SlotContent.Empty,
                SlotContent.Empty
            };
            List<SlotContent> redSlots = new List<SlotContent>()
            {
                SlotContent.Bot, SlotContent.Bot, SlotContent.Bot, SlotContent.Player, SlotContent.Player, SlotContent.Empty
            };
            observer.SetObserve(blueSlots, redSlots);

            history.Update();

            Assert.IsTrue(sut.TeamHasMore(Team.Red));
        }

        [TestMethod]
        public void TestWhenBlueHasFewerBotsThenBlueHasFewerBotsIsTrue()
        {
            FakeSlotContentObserver observer = new FakeSlotContentObserver();
            SlotContentHistory history = new SlotContentHistory(observer);
            BotSlots sut = new BotSlots(history);

            List<SlotContent> blueSlots = new List<SlotContent>()
            {
                SlotContent.Empty, SlotContent.Empty, SlotContent.Bot, SlotContent.Player, SlotContent.Empty,
                SlotContent.Empty
            };
            List<SlotContent> redSlots = new List<SlotContent>()
            {
                SlotContent.Bot, SlotContent.Bot, SlotContent.Bot, SlotContent.Player, SlotContent.Player, SlotContent.Empty
            };

            observer.SetObserve(blueSlots, redSlots);


            history.Update();

            Assert.IsTrue(sut.TeamHasFewer(Team.Blue));
        }

        [TestMethod]
        public void TestWhenRedHasMoreBotsThenRedHasFewerBotsIsFalse()
        {
            FakeSlotContentObserver observer = new FakeSlotContentObserver();
            SlotContentHistory history = new SlotContentHistory(observer);
            BotSlots sut = new BotSlots(history);

            List<SlotContent> blueSlots = new List<SlotContent>()
            {
                SlotContent.Empty, SlotContent.Empty, SlotContent.Bot, SlotContent.Player, SlotContent.Empty,
                SlotContent.Empty
            };
            List<SlotContent> redSlots = new List<SlotContent>()
            {
                SlotContent.Bot, SlotContent.Bot, SlotContent.Bot, SlotContent.Player, SlotContent.Player, SlotContent.Empty
            };
            observer.SetObserve(blueSlots, redSlots);

            history.Update();

            Assert.IsFalse(sut.TeamHasFewer(Team.Red));
        }

    }
}
