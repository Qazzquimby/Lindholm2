using System.Collections.Generic;

namespace Lindholm.Slots
{
    interface ISlotContentObserver
    {
        List<SlotContent> Observe();
    }
}