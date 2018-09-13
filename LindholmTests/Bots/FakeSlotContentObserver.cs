using System;
using System.Collections.Generic;
using System.Linq;
using Lindholm.Slots;

namespace LindholmTests.Bots
{
    public class FakeSlotContentObserver : ISlotContentObserver
    {
        private List<SlotContent> _blueSlots = new List<SlotContent>(){SlotContent.Bot, SlotContent.Empty, SlotContent.Player, SlotContent.Bot, SlotContent.Empty, SlotContent.Player, };
        private List<SlotContent> _redSlots = new List<SlotContent>() { SlotContent.Bot, SlotContent.Empty, SlotContent.Player, SlotContent.Bot, SlotContent.Empty, SlotContent.Player, };

        public List<SlotContent> Observe(Dictionary<int, List<SlotContent>> history)
        {
            List<SlotContent> observations = new List<SlotContent>();
            observations = observations.Concat(_blueSlots).ToList();
            observations = observations.Concat(_redSlots).ToList();
            return observations;
        }

        public void SetObserve(List<SlotContent> blueSlots, List<SlotContent> redSlots)
        {
            _blueSlots = blueSlots;
            _redSlots = redSlots;
        }
    }
}