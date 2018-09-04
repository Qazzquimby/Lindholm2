namespace Lindholm.Slots
{
    public class AllSlots : BaseSlots
    {
        internal AllSlots(SlotContentHistory history) : base(history) { }

        internal override bool SlotContentIsInCategory(SlotContent content)
        {
            return true;
        }
    }

    public class FilledSlots : BaseSlots
    {
        internal FilledSlots(SlotContentHistory history) : base(history) { }

        internal override bool SlotContentIsInCategory(SlotContent content)
        {
            return content == SlotContent.Bot || content == SlotContent.Player;
        }
    }

    public class EmptySlots : BaseSlots
    {
        internal EmptySlots(SlotContentHistory history) : base(history) { }

        internal override bool SlotContentIsInCategory(SlotContent content)
        {
            return content == SlotContent.Empty;
        }
    }

    public class PlayerSlots : BaseSlots
    {
        internal PlayerSlots(SlotContentHistory history) : base(history) { }

        internal override bool SlotContentIsInCategory(SlotContent content)
        {
            return content == SlotContent.Player;
        }
    }

    public class BotSlots : BaseSlots
    {
        internal BotSlots(SlotContentHistory history) : base(history) { }

        internal override bool SlotContentIsInCategory(SlotContent content)
        {
            return content == SlotContent.Bot;
        }
    }
}
