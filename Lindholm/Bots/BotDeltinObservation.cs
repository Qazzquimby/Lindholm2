using System.Collections.Generic;

namespace Lindholm.Bots
{
    internal class BotDeltinObservation : IBotDeltinObservation
    {
        private readonly Deltin.CustomGameAutomation.AI _ai;

        public BotDeltinObservation(Deltin.CustomGameAutomation.AI ai)
        {
            _ai = ai;
        }       

        public bool SafeIsAi(int slot)
        {
            return _ai.AccurateIsAI(slot);
        }

        public List<int> SafeGetBotSlots()
        {
            return _ai.GetAISlots(accurate: true);
        }

        public bool FastIsAi(int slot)
        {
            return _ai.IsAI(slot);
        }

        public List<int> FastGetBotSlots()
        {
            return _ai.GetAISlots(accurate: false);
        }

        public void CalibrateSafeIsAi()
        {
            _ai.CalibrateAIChecking();
        }

//        internal Difficulty GetAiDifficulty(int slot) todo later
//        {
//            Deltin.CustomGameAutomation.Difficulty? difficulty = _ai.GetAIDifficulty(slot);
//
//        }
    }
}