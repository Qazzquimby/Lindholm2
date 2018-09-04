using System;

namespace Lindholm.Phases
{
    internal class PhaseManager
    {
        public Phase CurrentPhase { get; private set; }
        private bool _initialPhaseSet;

        public PhaseManager()
        {
            CurrentPhase = new Phase("EMPTY PHASE");
        }

        public void SetInitialPhase(Phase phase)
        {
            if (_initialPhaseSet)
            {
                CurrentPhase = phase;
                _initialPhaseSet = true;
            }
            else
            {
                throw (new MethodAccessException("SetInitialPhase can only be run once."));
            }
        }

        public void EnterPhase(Phase newPhase)
        {
            CurrentPhase?.PerformExit();
            CurrentPhase = newPhase;
            CurrentPhase.PerformEntry();
        }

    }
}