namespace Lindholm.Chat
{
    internal class ChatDeltinChannelSwapper : IChatDeltinChannelSwapper
    {
        private readonly Deltin.CustomGameAutomation.Chat _chat;

        public ChatDeltinChannelSwapper(Deltin.CustomGameAutomation.Chat chat)
        {
            _chat = chat;
        }

        public void SwapChannel(Channel channel)
        {
            _chat.SwapChannel(channel.ToDeltin());
        }
    }
}