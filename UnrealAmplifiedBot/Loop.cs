using System;
using System.Collections.Generic;

namespace BotLibrary
{

    class GameLoop : WrapperComponent
    {
        public int TicksPerSecond = 10;
        public int StandardDelay = 5;

        public GameLoop(Lindholm wrapperInject) : base(wrapperInject) { }

        private Dictionary<int, List<Action>> PhaselessLoopFuncs { get; set; } = new Dictionary<int, List<Action>>() { };

        public void Start()
        {
            while (true)
            {
                wrapper.phases.GetCurrPhase().PerformLoop();
                wrapper.ServerDuration++;
                wrapper.match.matchDuration++;
                System.Threading.Thread.Sleep(1000 / TicksPerSecond);
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
                PhaselessLoopFuncs[delay] = new List<Action>();
                PhaselessLoopFuncs[delay].Add(func);
            }
        }



    }
}