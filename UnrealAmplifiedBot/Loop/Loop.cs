using System;
using System.Collections.Generic;
using System.Diagnostics;
using Lindholm.Phases;
using UnrealAmplified;

namespace Lindholm
{

    internal class GameLoop
    {
        private PhaseManager Phases;
        private Stopwatch _stopwatch;
        public GameLoop(PhaseManager phases) {
            Phases = phases;
            _stopwatch = new Stopwatch();
        }

        private Dictionary<int, List<Action>> PhaselessLoopFuncs { get; set; } = new Dictionary<int, List<Action>>() { };

        public void Start()
        {
            while (true)
            {
                _stopwatch.Start();
                Phases.CurrentPhase.PerformLoop();
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
                PhaselessLoopFuncs[delay].Add(func);
            }
            catch (KeyNotFoundException)
            {
                PhaselessLoopFuncs[delay] = new List<Action>
                {
                    func
                };
            }
        }
    }
}