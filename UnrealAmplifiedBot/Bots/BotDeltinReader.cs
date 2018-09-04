using System.Collections.Generic;

namespace Lindholm.Bots
{
    internal class BotDeltinReader
    {
        private readonly Deltin.CustomGameAutomation.AI _ai;

        private BotDeltinReader(Deltin.CustomGameAutomation.AI ai)
        {
            _ai = ai;
        }

        internal bool SafeIsAi(int slot)
        {
            return _ai.AccurateIsAI(slot);
        }

        internal List<int> SafeGetBotSlots()
        {
            return _ai.GetAISlots(accurate: true);
        }

        internal bool FastIsAi(int slot)
        {
            return _ai.IsAI(slot);
        }

        internal List<int> FastGetBotSlots()
        {
            return _ai.GetAISlots(accurate: false);
        }

        internal void CalibrateSafeIsAi()
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