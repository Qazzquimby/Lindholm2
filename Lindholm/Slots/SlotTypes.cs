namespace Lindholm.Slots
{
    public class AllSlots : BaseSlots
    {
        internal AllSlots(ISlotContentHistory history) : base(history)
        {
        }

        internal override bool SlotContentIsInCategory(SlotContent content)
        {
            return true;
        }
    }

    public class FilledSlots : BaseSlots
    {
        internal FilledSlots(ISlotContentHistory history) : base(history)
        {
        }

        internal override bool SlotContentIsInCategory(SlotContent content)
        {
            return content == SlotContent.Bot || content == SlotContent.Player;
        }
    }

    public class EmptySlots : BaseSlots
    {
        internal EmptySlots(ISlotContentHistory history) : base(history)
        {
        }

        internal override bool SlotContentIsInCategory(SlotContent content)
        {
            return content == SlotContent.Empty;
        }
    }

    public class PlayerSlots : BaseSlots
    {
        internal PlayerSlots(ISlotContentHistory history) : base(history)
        {
        }

        internal override bool SlotContentIsInCategory(SlotContent content)
        {
            return content == SlotContent.Player;
        }
    }

    public class BotSlots : BaseSlots
    {
        internal BotSlots(ISlotContentHistory history) : base(history)
        {
        }

        internal override bool SlotContentIsInCategory(SlotContent content)
        {
            return content == SlotContent.Bot;
        }
    }
}