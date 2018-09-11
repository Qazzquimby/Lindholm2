using System.CodeDom.Compiler;
using System.Collections.Generic;
using Lindholm.Bots;

namespace Lindholm.Slots
{
    interface ISlotContentObserver
    {
        List<SlotContent> Observe();
    }

    internal class SlotContentObserver
    {
        private Deltin.CustomGameAutomation.CustomGame _cg;
        private BotsModifiedFlag _modifiedFlag;

        SlotContentObserver(Deltin.CustomGameAutomation.CustomGame cg, BotsModifiedFlag modifiedFlag)
        {
            _cg = cg;
            _modifiedFlag = modifiedFlag;
        }

        public List<SlotContent> Observe()
        {
            List<int> filledSlots = _cg.PlayerSlots;
            if (_modifiedFlag.Value)
            {

            }

        }

    }


}
