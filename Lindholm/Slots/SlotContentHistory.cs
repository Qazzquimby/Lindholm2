using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Lindholm.Slots
{
    internal enum SlotContent
    {
        Empty,
        Player,
        Bot
    }


    internal interface ISlotContentHistory
    {
        SlotContent Current(int slot);
        List<SlotContent> History(int slot);
        void Update(List<SlotContent> slots);
        void PurgeHistory();
    }

    internal class SlotContentHistory : ISlotContentHistory
    {
        private readonly int _numTrackedSlots = 12;
        private Dictionary<int, List<SlotContent>> _history;
        
        internal SlotContentHistory()
        {
            PurgeHistory();
        }

        public SlotContent Current(int slot)
        {
            return _history[slot][_history[slot].Count-1];
        }
        
        public List<SlotContent> History(int slot)
        {
            return _history[slot];
        }

        public void Update(List<SlotContent> slots)
        {

            Debug.Assert(slots.Count == _numTrackedSlots, $"{nameof(slots)} must be of size {_numTrackedSlots}.");
            for (int i = 0; i < _numTrackedSlots; i++)
            {
                _history[i].Add(slots[i]);
            }
        }

        public void PurgeHistory()
        {
            _history = new Dictionary<int, List<SlotContent>>();
            for (int i = 0; i < _numTrackedSlots; i++)
            {
                _history[i] = new List<SlotContent>();
            }
        }
    }
}