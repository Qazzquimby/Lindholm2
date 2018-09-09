using System;
using System.Collections.Generic;
using System.Linq;
using Lindholm;
using Lindholm.Slots;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LindholmTests.Slots
{
    [TestClass]
    public class SlotConstantsTests
    {

        public void GivesTeamWithSlotTooSmallSlot()
        {
            SlotConstants.TeamWithSlot(-1);
        }

        [TestMethod]
        public void TestWhenTeamWithSlotGivenTooSmallSlotThenThrows()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>((Action) GivesTeamWithSlotTooSmallSlot);
        }

        public void GivesTeamWithSlotTooLargeSlot()
        {
            SlotConstants.TeamWithSlot(-1);
        }

        [TestMethod]
        public void TestWhenTeamWithSlotGivenTooLargeSlotThenThrows()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>((Action) GivesTeamWithSlotTooLargeSlot);
        }

        [TestMethod]
        public void TestWhenTeamWithSlotBlue()
        {
            Assert.AreEqual(SlotConstants.TeamWithSlot(0), Team.Blue);
            Assert.AreEqual(SlotConstants.TeamWithSlot(5), Team.Blue);
        }

        [TestMethod]
        public void TestWhenTeamWithSlotRed()
        {
            Assert.AreEqual(SlotConstants.TeamWithSlot(6), Team.Red);
            Assert.AreEqual(SlotConstants.TeamWithSlot(11), Team.Red);
        }
    }
}
