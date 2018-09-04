using Deltin.CustomGameAutomation;
using System;
using System.Collections.Generic;

namespace Lindholm
{

    class MatchManager : WrapperComponent
    {
        public double matchDuration = 0;
        public bool gameEnded = false;

        private DateTime lastGameOver = DateTime.Parse("2017-02-16T00:00:00-0:00");
        private List<System.EventHandler<Deltin.CustomGameAutomation.GameOverArgs>> gameOverArgumentFuncs
            = new List<System.EventHandler<Deltin.CustomGameAutomation.GameOverArgs>>();
        private List<Action> gameOverFuncs = new List<Action>();

        public MatchManager(Lindholm wrapperInject) : base(wrapperInject)
        {
            cg.OnGameOver += HandleGameOver;
        }

        private void HandleGameOver(object sender, GameOverArgs args)
        {
            Dev.Log("Running HandleGameOver");
            double passedSeconds = DateTime.Now.Subtract(lastGameOver).TotalSeconds;
            if (passedSeconds > 30)
            {
                HandleArgumentlessGameOver();
                PerformGameOverArgumentFuncs(sender, args);
            }
            else
            {
                Dev.Log("IGNORING GAME OVER, BECAUSE TOO SOON SINCE PREVIOUS GAME OVER");
            }
        }

        public void HandleArgumentlessGameOver()
        {
            lastGameOver = DateTime.Now;
            PerformGameOverFuncs();
            gameEnded = true;
        }

        public void AddGameOverArgumentFunc(System.EventHandler<Deltin.CustomGameAutomation.GameOverArgs> func)
        {   
            gameOverArgumentFuncs.Add(func);
        }

        public void AddGameOverFunc(Action func)
        {
            gameOverFuncs.Add(func);
        }

        private void PerformGameOverArgumentFuncs(object sender, GameOverArgs args)
        {
            Dev.Log("Running game over arg funcs");
            foreach (System.EventHandler<Deltin.CustomGameAutomation.GameOverArgs> func in gameOverArgumentFuncs)
            {
                func(sender, args);
            }
        }

        public void PerformGameOverFuncs()
        {
            Dev.Log("Running game over funcs");
            foreach (Action func in gameOverFuncs)
            {
                func();
            }
        }

        public void PreventMapTimeout()
        {
            int timeServerMustBeEmptyToTimeout = 100;
            int minutesBeforeEarliestTimeout = 3;


            bool consistentlyEmpty = true;
            if (wrapper.players.PlayerCountHistory[Team.Red].Count > timeServerMustBeEmptyToTimeout)
            {
                for (int i = 0; i < timeServerMustBeEmptyToTimeout; i++)
                {
                    try
                    {
                        if (wrapper.players.PlayerCountHistory[Team.Red][wrapper.players.PlayerCountHistory[Team.Red].Count - i - 1] + wrapper.players.PlayerCountHistory[Team.Blue][wrapper.players.PlayerCountHistory[Team.Blue].Count - i - 1] > 0)
                        {
                            consistentlyEmpty = false;
                            break;
                        }
                    }
                    catch
                    {
                        break;
                    }
                }
            }

            if (consistentlyEmpty && matchDuration > (minutesBeforeEarliestTimeout * 60) * wrapper.loop.TicksPerSecond)
            {
                wrapper.chat.MatchChat("Sever is timing out soon. Moving on.");
                wrapper.maps.RandomAndNextMap();
            }
        }
    }
}