using Deltin.CustomGameAutomation;
using System;

namespace Lindholm
{

    class JoinManager : WrapperComponent
    {
        private Join DesiredJoin = Join.Everyone;
        private Join CurrentJoin;

        public JoinManager(Game wrapper) : base(wrapper) { }

        public void Start()
        {
            SetJoin(DesiredJoin);
        }

        public void SetJoin(Join join)
        {
            DesiredJoin = join;
            if (CurrentJoin != DesiredJoin)
            {
                cg.Settings.SetJoinSetting(DesiredJoin);
                CurrentJoin = join;
            }
        }
    }
}