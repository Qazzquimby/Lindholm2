using System;
using System.Collections.Generic;
using System.Diagnostics;
using Lindholm.Phases;

namespace Lindholm.Loop
{

    internal class GameLoop
    {
        private readonly PhaseManager _phases;
        private readonly Stopwatch _stopwatch;
        public bool Running = true;
        public GameLoop(PhaseManager phases) {
            _phases = phases;
            _stopwatch = new Stopwatch();
        }

        private Dictionary<int, List<Action>> PhaselessLoopFunctions
        {
            get; 
        } = new Dictionary<int, List<Action>>();

        public void Start()
        {
            while (Running)
            {
                _stopwatch.Start();
                _phases.CurrentPhase.PerformLoop();
                //wrapper.ServerDuration++; todosoon readd from server component
                //wrapper.Match.matchDuration++; todosoon readd from match component
                _stopwatch.Stop();
                int msPerTick = 1000 / TimeConstants.TicksPerSecond;
                System.Threading.Thread.Sleep(msPerTick - (int)_stopwatch.ElapsedMilliseconds);
            }
        }

        public void AddPhaselessLoop(Action func, int delay)
        {
            try
            {
                PhaselessLoopFunctions[delay].Add(func);
            }
            catch (KeyNotFoundException)
            {
                PhaselessLoopFunctions[delay] = new List<Action>
                {
                    func
                };
            }
        }
    }
}