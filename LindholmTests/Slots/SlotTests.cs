using System;
using System.Collections.Generic;
using System.Linq;
using Lindholm;
using Lindholm.Slots;
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

            public void Update(List<SlotContent> slots)
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
            SlotContentHistory history = new SlotContentHistory();
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

            history.Update(blueSlots.Concat(redSlots).ToList());

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
            SlotContentHistory history = new SlotContentHistory();
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

            history.Update(blueSlots.Concat(redSlots).ToList());

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
            SlotContentHistory history = new SlotContentHistory();
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

            history.Update(blueSlots.Concat(redSlots).ToList());

            Assert.IsFalse(sut.TeamsHaveEqualCount);
        }

        [TestMethod]
        public void TestWhenPlayerCountSameThenTeamsHaveEqualCountIsTrue()
        {
            SlotContentHistory history = new SlotContentHistory();
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

            history.Update(blueSlots.Concat(redSlots).ToList());

            Assert.IsTrue(sut.TeamsHaveEqualCount);
        }


        [TestMethod]
        public void TestWhenBlueHasFewerBotsThenBlueHasMoreBotsIsFalse()
        {
            SlotContentHistory history = new SlotContentHistory();
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

            history.Update(blueSlots.Concat(redSlots).ToList());

            Assert.IsFalse(sut.TeamHasMore(Team.Blue));
        }

        [TestMethod]
        public void TestWhenRedHasMoreBotsThenRedHasMoreBotsIsTrue()
        {
            SlotContentHistory history = new SlotContentHistory();
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

            history.Update(blueSlots.Concat(redSlots).ToList());

            Assert.IsTrue(sut.TeamHasMore(Team.Red));
        }

        [TestMethod]
        public void TestWhenBlueHasFewerBotsThenBlueHasFewerBotsIsTrue()
        {
            SlotContentHistory history = new SlotContentHistory();
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

            history.Update(blueSlots.Concat(redSlots).ToList());

            Assert.IsTrue(sut.TeamHasFewer(Team.Blue));
        }

        [TestMethod]
        public void TestWhenRedHasMoreBotsThenRedHasFewerBotsIsFalse()
        {
            SlotContentHistory history = new SlotContentHistory();
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

            history.Update(blueSlots.Concat(redSlots).ToList());

            Assert.IsFalse(sut.TeamHasFewer(Team.Red));
        }

    }
}
