using System;
using System.Collections.Generic;
using System.Linq;
using Lindholm;
using Lindholm.Slots;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LindholmTests.Slots
{
    [TestClass]
    public class SlotContentHistoryTests
    {

        [TestMethod]
        public void TestSlotContentHistoryLengthEqualToNumUpdates()
        {
            SlotContentHistory sut = new SlotContentHistory();
            
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

            sut.Update(blueSlots.Concat(redSlots).ToList());
            Assert.AreEqual(sut.History(0).Count, 1);

            sut.Update(blueSlots.Concat(redSlots).ToList());
            Assert.AreEqual(sut.History(11).Count, 2);

        }

    }
}
