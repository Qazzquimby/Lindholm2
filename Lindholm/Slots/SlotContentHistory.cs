using System.Collections.Generic;
using System.Diagnostics;

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
        void Update();
        void PurgeHistory();
    }

    internal class SlotContentHistory : ISlotContentHistory
    {
        private readonly int _numTrackedSlots = 12;
        private Dictionary<int, List<SlotContent>> _history;
        private SlotContentObserver _observer;
        
        internal SlotContentHistory(SlotContentObserver observer)
        {
            _observer = observer;
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

        public void Update()
        {
            List<SlotContent> slotContentObservations = _observer.Observe();
            Debug.Assert(slotContentObservations.Count == _numTrackedSlots, $"{nameof(slotContentObservations)} must be of size {_numTrackedSlots}.");
            for (int i = 0; i < _numTrackedSlots; i++)
            {
                _history[i].Add(slotContentObservations[i]);
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