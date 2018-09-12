using System;
using System.Collections.Generic;
using Lindholm.Bots;

namespace Lindholm.Slots
{
    internal class SlotContentObserver : ISlotContentObserver
    {
        private readonly DeltinSlotObservation _observation;
        private readonly BotsModifiedFlag _modifiedFlag;

        internal SlotContentObserver(BotsModifiedFlag modifiedFlag, DeltinSlotObservation observation)
        {
            _modifiedFlag = modifiedFlag;
            _observation = observation;
        }

        public List<SlotContent> Observe()
        {
            List<int> filledSlots = _observation.FilledSlots;
            List<int> botSlots;
            List<int> playerSlots;
            if (_modifiedFlag.Value)
            {
                
            }

            throw new NotImplementedException();
        }

    }


}
