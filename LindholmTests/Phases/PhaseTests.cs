using System;
using Lindholm.Phases;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LindholmTests.Phases
{
    [TestClass()]
    public class PhaseTests
    {
        [TestMethod()]
        public void TestPhaseHasGivenName()
        {
            string name = "some test name 123 .!";
            Phase sut = new Phase(name);

            Assert.AreEqual(sut.Name, name);
        }

        private int _testValue;

        private void Reset()
        {
            _testValue = 5;
        }

        private void AddOneToTestValue()
        {
            _testValue += 1;
        }

        private void MultiplyTwoToTestValue()
        {
            _testValue *= 2;
        }

        private void CrashAction()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void TestWhenAddEntryActionOnceThenRunsAction()
        {
            Reset();
            Phase sut = new Phase("sut");
            sut.AddEntry(AddOneToTestValue);
            sut.PerformEntry();
            Assert.AreEqual(_testValue, 6);
        }

        [TestMethod()]
        public void TestWhenAddEntryTwiceHasTwoTests()
        {
            Reset();
            Phase sut = new Phase("sut");
            sut.AddEntry(MultiplyTwoToTestValue);
            sut.AddEntry(AddOneToTestValue);
            sut.PerformEntry();
            Assert.AreEqual(_testValue, 11);
        }

        [TestMethod()]
        public void TestWhenAddExitActionOnceThenRunsAction()
        {
            Reset();
            Phase sut = new Phase("sut");
            sut.AddExit(AddOneToTestValue);
            sut.PerformExit();
            Assert.AreEqual(_testValue, 6);
        }

        [TestMethod()]
        public void TestWhenAddExitTwiceHasTwoTests()
        {
            Reset();
            Phase sut = new Phase("sut");
            sut.AddExit(MultiplyTwoToTestValue);
            sut.AddExit(AddOneToTestValue);
            sut.PerformExit();
            Assert.AreEqual(_testValue, 11);
        }

        [TestMethod()]
        public void TestWhenPerformLoopRun3TimesThenDelay3FuncRun1()
        {
            Reset();
            Phase sut = new Phase("sut");
            sut.AddLoop(AddOneToTestValue, 3);
            sut.PerformLoop();
            sut.PerformLoop();
            sut.PerformLoop();
            Assert.AreEqual(_testValue, 6);
        }

        [TestMethod()]
        public void TestWhenPerformLoopRun4TimesThenDelay3FuncRun2()
        {
            Reset();
            Phase sut = new Phase("sut");
            sut.AddLoop(AddOneToTestValue, 3);
            sut.PerformLoop();
            sut.PerformLoop();
            sut.PerformLoop();
            sut.PerformLoop();
            Assert.AreEqual(_testValue, 7);
        }

        [TestMethod()]
        public void TestWhenPerformDelayRun2TimesThenDelay3FuncNotRun()
        {
            Reset();
            Phase sut = new Phase("sut");
            sut.AddDelay(AddOneToTestValue, 3);
            sut.PerformLoop();
            sut.PerformLoop();
            Assert.AreEqual(_testValue, 5);
        }

        [TestMethod()]
        public void TestWhenPerformDelayRun100TimesThenDelay3FuncRun1()
        {
            Reset();
            Phase sut = new Phase("sut");
            sut.AddDelay(AddOneToTestValue, 3);
            for (int i = 0; i < 100; i++)
            {
                sut.PerformLoop();
            }
           
            Assert.AreEqual(_testValue, 6);
        }


        [TestMethod()]
        public void AddLoopTest()
        {

        }

        [TestMethod()]
        public void AddExitTest()
        {

        }

        [TestMethod()]
        public void AddDelayTest()
        {

        }

        [TestMethod()]
        public void PerformEntryTest()
        {

        }

        [TestMethod()]
        public void PerformDelayTest()
        {

        }

        [TestMethod()]
        public void PerformLoopTest()
        {

        }

        [TestMethod()]
        public void PerformExitTest()
        {

        }

        [TestMethod()]
        public void PerformAllFuncsTest()
        {

        }
    }
}