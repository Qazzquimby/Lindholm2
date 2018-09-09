using System.Collections.Generic;

namespace Lindholm.Bots
{
    internal interface IBotDeltinReader
    {
        bool SafeIsAi(int slot);
        List<int> SafeGetBotSlots();
        bool FastIsAi(int slot);
        List<int> FastGetBotSlots();
        void CalibrateSafeIsAi();
    }
}