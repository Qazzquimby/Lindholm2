namespace Lindholm.Chat
{
    internal class ChatDeltinPrinter : IChatDeltinPrinter
    {
        private readonly Deltin.CustomGameAutomation.Chat _chat;

        public ChatDeltinPrinter(Deltin.CustomGameAutomation.Chat chat)
        {
            _chat = chat;
        }

        public void SendChatMessage(string text)
        {
            _chat.SendChatMessage(text);
        }
    }
}