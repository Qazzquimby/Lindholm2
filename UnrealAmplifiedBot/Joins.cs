using Deltin.CustomGameAutomation;
using System;

namespace BotLibrary
{

    class JoinManager : WrapperComponent
    {
        private Join DesiredJoin = Join.Everyone;
        private Join CurrentJoin;
        //private int LockLevel = 0;

        public JoinManager(Lindholm wrapper) : base(wrapper) { }

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

        public void SlotOperation(Action operation)
        {
            LockSlots();
            operation();
            UnlockSlots();
        }

        public void LockSlots()
        {
            //LockLevel++;
            //if(CurrentJoin != Join.InviteOnly)
            //{
            //    cg.GameSettings.SetJoinSetting(Join.InviteOnly);
            //}  
        }

        public void UnlockSlots()
        {
            //LockLevel--;
            //if(LockLevel == 0)
            //{
            //    SetJoin(DesiredJoin);
            //}
            //if(LockLevel < 0)
            //{
            //    System.Diagnostics.Debug.Assert(false);
            //}
        }

    }
}