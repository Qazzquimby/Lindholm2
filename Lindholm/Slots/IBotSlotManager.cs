namespace Lindholm.Slots
{
    public interface IBotSlotManager
    {
        bool BotsModified { get; set; }
        BotSlots BotSlots { get; }
    }
}