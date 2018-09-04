using System;
using System.Collections.Generic;

namespace Lindholm.Phases
{
    public class Phase
    {
        public string Name { get; private set; }

        private int _timer = 0;

        private readonly List<Action> _entryFunctions = new List<Action>();
        private readonly List<Action> _exitFunctions = new List<Action>();
        private Dictionary<int, List<Action>> DelayFunctions { get; set; } = new Dictionary<int, List<Action>>() { };
        private Dictionary<int, List<Action>> LoopFunctions { get; set; } = new Dictionary<int, List<Action>>() { };

        public Phase(string name)
        {
            Name = name;
        }

        public void AddEntry(Action func)
        {
            _entryFunctions.Add(func);
        }

        public void AddLoop(Action func, int delay)
        {
            try
            {
                LoopFunctions[delay].Add(func);
            }
            catch (KeyNotFoundException)
            {
                LoopFunctions[delay] = new List<Action>
                {
                    func
                };
            }
        }

        public void AddExit(Action func)
        {
            _exitFunctions.Add(func);
        }

        public void AddDelay(Action func, int delay)
        {
            try
            {
                DelayFunctions[delay].Add(func);
            }
            catch (KeyNotFoundException)
            {
                DelayFunctions[delay] = new List<Action>
                {
                    func
                };
            }
        }

        public void PerformEntry()
        {
            PerformAllFuncs(_entryFunctions);
        }

        public void PerformDelay()
        {
            try
            {
                foreach (int delay in DelayFunctions.Keys)
                {
                    if (_timer == delay)
                    {
                        List<Action> funcs = DelayFunctions[delay];
                        PerformAllFuncs(funcs);
                        DelayFunctions.Remove(delay);
                    }
                }
            }
            catch (InvalidOperationException) { } //may be the wrong exception.
        }

        public void PerformLoop()
        {
            try
            {


                foreach (int delay in LoopFunctions.Keys)
                {
                    if (_timer % delay == 0)
                    {
                        List<Action> funcs = LoopFunctions[delay];
                        PerformAllFuncs(funcs);
                    }
                }
                PerformDelay();
                _timer++;
            }
            catch (InvalidOperationException) { }
        }

        public void PerformExit()
        {
            PerformAllFuncs(_exitFunctions);
        }

        public void PerformAllFuncs(List<Action> funcs)
        {
            foreach (Action func in funcs)
            {
                func();
            }
        }

    }
}