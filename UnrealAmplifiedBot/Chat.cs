using Deltin.CustomGameAutomation;

namespace BotLibrary
{


    class Chat : WrapperComponent
    {
        private Channel channel;

        public Chat(CustomGameWrapper wrapperInject) : base(wrapperInject) {

        }

        public void Start()
        {
        }

        public void SetPrefix(string prefix)
        {
            cg.Chat.ChatPrefix = prefix;
        }

        public void EnsureMatchChat() { 
            channel = Channel.Match;
            cg.Chat.SwapChannel(Channel.Match);
        }

        public void MatchChat(string text)
        {
            if (channel != Channel.Match)
            {
                channel = Channel.Match;
                cg.Chat.SwapChannel(Channel.Match);
            }

            if (text != null)
            {
                cg.Chat.Chat(text);
            }
        }


    }
}