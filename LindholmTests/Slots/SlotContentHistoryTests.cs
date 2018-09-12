using System.Collections.Generic;
using Lindholm.Slots;
using LindholmTests.Bots;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LindholmTests.Slots
{
    [TestClass]
    public class SlotContentHistoryTests
    {

        [TestMethod]
        public void TestSlotContentHistoryLengthEqualToNumUpdates()
        {
            FakeSlotContentObserver observer = new FakeSlotContentObserver();
            SlotContentHistory sut = new SlotContentHistory(observer);
            
            List<SlotContent> blueSlots = new List<SlotContent>()
            {
                SlotContent.Empty, SlotContent.Empty, SlotContent.Bot, SlotContent.Player, SlotContent.Empty,
                SlotContent.Empty
            };
            List<SlotContent> redSlots = new List<SlotContent>()
            {
                SlotContent.Bot, SlotContent.Bot, SlotContent.Bot, SlotContent.Player, SlotContent.Player, SlotContent.Empty
            };

            Assert.AreEqual(sut.History(4).Count, 0);

            sut.Update();
            Assert.AreEqual(sut.History(0).Count, 1);

            sut.Update();
            Assert.AreEqual(sut.History(11).Count, 2);

        }

    }
}
