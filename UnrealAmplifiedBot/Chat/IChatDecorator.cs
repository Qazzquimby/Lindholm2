namespace Lindholm.Chat
{
    public interface IChatDecorator
    {
        string GetDecoratedText(string text);
        void SetPrefix(string prefix);
        void SetSuffix(string suffix);
    }
}