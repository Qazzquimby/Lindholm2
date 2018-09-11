using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lindholm.Bots
{
    class BotsModifiedFlag
    {
        public bool Value { get; private set;  } = false;

        public void Flag()
        {
            Value = true;
        }

        public void Unflag()
        {
            Value = false;
        }

    }
}
