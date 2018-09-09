using Lindholm.Chat;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LindholmTests.Chat
{
    [TestClass()]
    public class ChatChannelSwapperTests
    {
        class FakeChatDeltinChannelSwapperCounter : IChatDeltinChannelSwapper
        {
            public int Counter = 0;

            public void SwapChannel(Channel channel)
            {
                Counter++;
            }
        }

        [TestMethod()]
        public void TestWhenSwapChannelToNullThenNoChange()
        {
            FakeChatDeltinChannelSwapperCounter counter = new FakeChatDeltinChannelSwapperCounter(); ;
            ChatChannelSwapper sut = new ChatChannelSwapper(counter);

            Assert.AreEqual(counter.Counter, 0);

            sut.SwapChannel(null);
            Assert.AreEqual(counter.Counter, 0);
        }

        [TestMethod()]
        public void TestWhenSwapChannelIfDifferentToSameChannelThenNoChange()
        {
            FakeChatDeltinChannelSwapperCounter counter = new FakeChatDeltinChannelSwapperCounter(); ;
            ChatChannelSwapper sut = new ChatChannelSwapper(counter);

            Assert.AreEqual(counter.Counter, 0);

            sut.SwapChannelIfDifferent(Channel.Group);
            Assert.AreEqual(counter.Counter, 1);

            sut.SwapChannelIfDifferent(Channel.Group);
            Assert.AreEqual(counter.Counter, 1);
        }

        [TestMethod()]
        public void TestSwapBackReturnsToPreviousChannel()
        {
            FakeChatDeltinChannelSwapperCounter counter = new FakeChatDeltinChannelSwapperCounter(); ;
            ChatChannelSwapper sut = new ChatChannelSwapper(counter);

            Assert.AreEqual(counter.Counter, 0);

            sut.SwapChannel(Channel.Group);
            Assert.AreEqual(counter.Counter, 1);

            sut.SwapChannel(Channel.Match);
            Assert.AreEqual(counter.Counter, 2);

            sut.SwapBack();
            Assert.AreEqual(counter.Counter, 3);

            sut.SwapChannelIfDifferent(Channel.Group);
            Assert.AreEqual(counter.Counter, 3);
        }

        [TestMethod()]
        public void TestSwapBackDoesNothingIfPreviousIsNull()
        {
            FakeChatDeltinChannelSwapperCounter counter = new FakeChatDeltinChannelSwapperCounter(); ;
            ChatChannelSwapper sut = new ChatChannelSwapper(counter);

            Assert.AreEqual(counter.Counter, 0);

            sut.SwapChannel(Channel.Group);
            Assert.AreEqual(counter.Counter, 1);

            sut.SwapBack();
            Assert.AreEqual(counter.Counter, 1);
        }
    }
}