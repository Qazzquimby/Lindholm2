namespace Lindholm.Bots
{
    public interface IBotManager
    {
        IBotRequester BotRequester { get; }
        IBotRequestFulfillmentManager BotRequestFulfillmentManager { get; }
    }
}