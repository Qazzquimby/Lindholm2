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
        private readonly string Name = "UNNAMED PHASE";

        public Phase()
        {
        }

        public Phase(string name)
        {
            Name = name;
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
                LoopFuncs[delay] = new List<Action>
                {
                    func
                };
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
                DelayFuncs[delay] = new List<Action>
                {
                    func
                };
            }
        }

        public void PerformEntry()
        {
            PerformAllFuncs(EntryFuncs);
        }

        public void PerformDelay()
        {
            try
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
            catch (InvalidOperationException)  { } //may be the wrong exception.
        }

        public void PerformLoop()
        {
            try
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
            catch (InvalidOperationException) { }
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

        //private int tempi=0; //fixme kill this

        public PhaseManager(Lindholm wrapperInject) : base(wrapperInject)
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
            Phase setUpPhase = new Phase("SETUP");
            setUpPhase.AddEntry(wrapper.logger.EndMatchLog);
            setUpPhase.AddEntry(wrapper.players.scrambler.ScrambleTeams);
            setUpPhase.AddEntry(SetGameNotEnding);

            setUpPhase.AddLoop(wrapper.players.balancer.SwapToBalance, 25);
            gamePhase.AddLoop(wrapper.bots.HandleBots, 50);

            setUpPhase.AddDelay(EndSetUpPhase, 250);

            return setUpPhase;
        }

        public Phase GamePhaseConstructor()
        {
            Phase gamePhase = new Phase("GAME");
            gamePhase.AddEntry(wrapper.chat.EnsureMatchChat);

            gamePhase.AddLoop(wrapper.players.UpdatePlayers, wrapper.loop.StandardDelay);
            gamePhase.AddLoop(wrapper.logger.UpdatematchLog, wrapper.loop.StandardDelay);
            gamePhase.AddLoop(wrapper.players.balancer.PerformAutoBalance, wrapper.loop.StandardDelay);
            gamePhase.AddLoop(wrapper.match.PreventMapTimeout, 300);
            gamePhase.AddLoop(wrapper.players.balancer.BeginOrEndAutobalance, 50);
            gamePhase.AddLoop(wrapper.bots.HandleBots, 50);
            gamePhase.AddLoop(HandleGameOver, wrapper.loop.StandardDelay);
            gamePhase.AddLoop(HandleMissedGameOver, wrapper.loop.StandardDelay);

            return gamePhase;
        }

        public Phase GameEndingPhaseConstructor()
        {
            Phase gameEndingPhase = new Phase("GAME ENDING");
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

        private void SetGameNotEnding()
        {
            wrapper.match.gameEnded = false;
        }

        private void HandleGameOver()
        {
            //cg.SaveScreenshot(string.Format(@"C:\Users\User\Documents\Visual Studio 2015\Projects\UnrealAmplifiedBot\screenshots\shot{0}.jpg",tempi));
            //tempi++;
            if (wrapper.match.gameEnded)
            {
                Debug.Log("Entering gameEndingPhase");
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
            Debug.Log("Entering game phase");
            EnterPhase(gamePhase);
        }


    }
}