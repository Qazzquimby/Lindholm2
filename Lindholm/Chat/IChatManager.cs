namespace Lindholm.Chat
{
    public interface IChatManager
    {
        IChatChannelSwapper ChannelSwapper { get; }
        IChatDecorator Decorator { get; }
        void Print(string text);
        void Print(string text, Channel channel);
    }
}