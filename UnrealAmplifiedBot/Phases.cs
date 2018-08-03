using Deltin.CustomGameAutomation;
using System;
using System.Collections.Generic;

namespace BotLibrary
{
    class Phase
    {
        private int timer = 0;

        private List<Action> EntryFuncs = new List<Action>();
        private Dictionary<int, List<Action>> DelayFuncs { get; set; } = new Dictionary<int, List<Action>>() { };
        private Dictionary<int, List<Action>> LoopFuncs { get; set; } = new Dictionary<int, List<Action>>() { };
        private List<Action> ExitFuncs = new List<Action>();

        public Phase()
        {
        }

        public void AddEntry(Action func)
        {
            EntryFuncs.Add(func);
        }

        public void AddLoop(Action func, int delay)
        {
            try
            {
                LoopFuncs[delay].Add(func);
            }
            catch (KeyNotFoundException)
            {
                LoopFuncs[delay] = new List<Action>();
                LoopFuncs[delay].Add(func);
            }
        }

        public void AddExit(Action func)
        {
            ExitFuncs.Add(func);
        }

        public void AddDelay(Action func, int delay)
        {
            try
            {
                DelayFuncs[delay].Add(func);
            }
            catch (KeyNotFoundException)
            {
                DelayFuncs[delay] = new List<Action>();
                DelayFuncs[delay].Add(func);
            }
        }

        public void PerformEntry()
        {
            PerformAllFuncs(EntryFuncs);
        }

        public void PerformDelay()
        {
            foreach (int delay in DelayFuncs.Keys)
            {
                if (timer == delay)
                {
                    List<Action> funcs = DelayFuncs[delay];
                    PerformAllFuncs(funcs);
                    DelayFuncs.Remove(delay);
                }
            }
        }

        public void PerformLoop()
        {
            foreach (int delay in LoopFuncs.Keys)
            {
                if (timer % delay == 0)
                {
                    List<Action> funcs = LoopFuncs[delay];
                    PerformAllFuncs(funcs);
                }
            }
            PerformDelay();
            timer++;
        }

        public void PerformExit()
        {
            PerformAllFuncs(ExitFuncs);
        }

        public void PerformAllFuncs(List<Action> funcs)
        {
            foreach (Action func in funcs)
            {
                func();
            }
        }

    }


    class PhaseManager : WrapperComponent
    {
        public Phase gamePhase;
        public Phase setUpPhase;
        public Phase gameEndingPhase;

        private Phase currPhase;

        public PhaseManager(CustomGameWrapper wrapperInject) : base(wrapperInject)
        {

            gamePhase = GamePhaseConstructor();
            currPhase = gamePhase;

            setUpPhase = SetUpPhaseConstructor();
            gameEndingPhase = GameEndingPhaseConstructor();

        }

        public void EnterPhase(Phase newPhase)
        {
            if (currPhase != null)
            {
                currPhase.PerformExit();
            }
            currPhase = newPhase;
            currPhase.PerformEntry();
        }

        public Phase GetCurrPhase()
        {
            return currPhase;
        }

        public Phase SetUpPhaseConstructor()
        {
            Phase setUpPhase = new Phase();
            setUpPhase.AddEntry(wrapper.logger.EndMatchLog);
            setUpPhase.AddEntry(wrapper.players.scrambler.ScrambleTeams);

            setUpPhase.AddLoop(wrapper.players.balancer.SwapToBalance, 25);

            setUpPhase.AddDelay(EndSetUpPhase, 250);

            return new Phase();
        }

        public Phase GamePhaseConstructor()
        {
            Phase gamePhase = new Phase();
            gamePhase.AddEntry(wrapper.chat.EnsureMatchChat);

            gamePhase.AddLoop(wrapper.players.UpdatePlayers, wrapper.loop.StandardDelay);
            gamePhase.AddLoop(wrapper.logger.UpdatematchLog, wrapper.loop.StandardDelay);
            gamePhase.AddLoop(wrapper.players.balancer.PerformAutoBalance, wrapper.loop.StandardDelay);
            gamePhase.AddLoop(wrapper.players.balancer.BeginOrEndAutobalance, 100);
            gamePhase.AddLoop(wrapper.bots.HandleBots, 100);

            return gamePhase;
        }

        private void HandleGameOver()
        {
            if (wrapper.match.gameEnded)
            {
                EnterPhase(gameEndingPhase);
            }
        }

        private void HandleMissedGameOver()
        {
            if (cg.GetGameState() == GameState.Ending_Commend)
            {
                Debug.Log("Missed game over");
                wrapper.match.HandleArgumentlessGameOver();
            }
        }

        private void EndSetUpPhase()
        {
            EnterPhase(gamePhase);
        }

        public Phase GameEndingPhaseConstructor()
        {
            Phase gameEndingPhase = new Phase();
            gameEndingPhase.AddEntry(wrapper.match.PerformGameOverFuncs);
            gameEndingPhase.AddEntry(SkipPostGame);
            return gameEndingPhase;
        }

        public void SkipPostGame()
        {
            int wait_timer = 0;
            while (cg.GetGameState() != GameState.Ending_Commend && wait_timer < 20)
            {
                System.Threading.Thread.Sleep(1000);
                wait_timer++;
                System.Diagnostics.Debug.WriteLine(string.Format("{0} seconds wait", wait_timer));
            }
            wrapper.maps.NextMap();
        }
    }
}