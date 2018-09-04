namespace Lindholm.Chat
{
    public interface IChatChannelSwapper
    {
        void SwapChannel(Channel? channel);
        void SwapChannelIfDifferent(Channel channel);
        void SwapBack();
    }
}