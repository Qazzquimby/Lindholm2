namespace Lindholm.Chat
{
    internal class ChatDecorator : IChatDecorator
    {
        private string _prefix = "";
        private string _suffix = "";

        public string GetDecoratedText(string text)
        {
            return $"{_prefix}{text}{_suffix}";
        }

        public void SetPrefix(string prefix)
        {
            _prefix = prefix;
        }

        public void SetSuffix(string suffix)
        {
            _suffix = suffix;
        }
    }
}