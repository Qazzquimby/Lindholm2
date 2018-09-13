using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Lindholm.Bots;

namespace Lindholm.Slots
{
    public class SlotContentObserver : ISlotContentObserver
    {
        private readonly IDeltinSlotObservation _observation;
        private readonly BotsModifiedFlag _modifiedFlag;

        internal SlotContentObserver(BotsModifiedFlag modifiedFlag, IDeltinSlotObservation observation)
        {
            _modifiedFlag = modifiedFlag;
            _observation = observation;
        }

        public List<SlotContent> Observe(Dictionary<int, List<SlotContent>> history)
        {
            List<int> filledSlots = _observation.FilledSlots;
            List<int> botSlots = GetBotSlots(history, filledSlots);
            List<int> playerSlots = filledSlots.Except(botSlots).ToList();

            List<SlotContent> slotContents = new List<SlotContent>();

            for (int slot = 0; slot < 12; slot++)
            {
                if (botSlots.Contains(slot))
                {
                    slotContents.Add(SlotContent.Bot);
                }
                else if (playerSlots.Contains(slot))
                {
                    slotContents.Add(SlotContent.Player);
                }
                else
                {
                    slotContents.Add(SlotContent.Empty);
                }
            }

            Debug.Assert(slotContents.Count == 12);

            return slotContents;
        }

        private List<int> GetBotSlots(Dictionary<int, List<SlotContent>> history, List<int> filledSlots)
        {
            if (_modifiedFlag.Value)
            {
                return GetNewBotSlots(filledSlots);
            }
            else
            {
                return GetBotSlotsFromHistory(history, filledSlots);
            }
        }

        private List<int> GetNewBotSlots(List<int> filledSlots)
        {
            _observation.BotObservation.CalibrateSafeIsAi();
            List<int> botSlots = new List<int>();
            foreach (int slot in filledSlots)
            {
                if (_observation.BotObservation.SafeIsAi(slot))
                {
                    botSlots.Add(slot);
                }
            }

            return botSlots;
        }

        private List<int> GetBotSlotsFromHistory(Dictionary<int, List<SlotContent>> history, List<int> filledSlots)
        {
            List<int> botSlots = new List<int>();
            foreach (int slot in filledSlots)
            {
                if (history[slot][history[slot].Count - 1] == SlotContent.Bot)
                {
                    botSlots.Add(slot);
                }
            }

            return botSlots;
        }

    }


}
