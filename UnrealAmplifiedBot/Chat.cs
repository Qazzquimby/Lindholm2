using Deltin.CustomGameAutomation;

namespace BotLibrary
{


    class Chat : WrapperComponent
    {
        private Channel Channel;

        private string Prefix = "";

        public Chat(Lindholm wrapperInject) : base(wrapperInject) {

        }

        public void Start()
        {
        }

        public void SetPrefix(string prefix)
        {
            Prefix = prefix;
        }

        public void EnsureMatchChat() {
            SwapChannel(Channel.Match);
        }

        public void MatchChat(string text)
        {
            if (Channel != Channel.Match)
            {
                SwapChannel(Channel.Match);
            }

            if (text != null)
            {
                cg.Chat.SendChatMessage(Prefix + text);
            }
        }

        private void SwapChannel(Channel channel)
        {
            Channel = channel;
            cg.Chat.SwapChannel(channel);
        }

    }
}