namespace Lindholm.Chat
{
    internal class ChatManagerManager : IChatManager
    {
        private readonly IChatDeltinPrinter _deltinPrinter;

        public IChatChannelSwapper ChannelSwapper { get; }

        public IChatDecorator Decorator { get; }

        public ChatManagerManager(IChatChannelSwapper channelSwapper, IChatDecorator decorator, IChatDeltinPrinter deltinPrinter)
        {
            ChannelSwapper = channelSwapper;
            Decorator = decorator;
            _deltinPrinter = deltinPrinter;
        }

        public void Print(string text)
        {
            Print(text, Channel.Match);
        }

        public void Print(string text, Channel channel)
        {
            ChannelSwapper.SwapChannelIfDifferent(channel);

            string decoratedText = Decorator.GetDecoratedText(text);
            _deltinPrinter.SendChatMessage(decoratedText);

            ChannelSwapper.SwapBack();
        }
    }
}