﻿using System;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace Deltin.CustomGameAutomation
{
    partial class CustomGame
    {
        void SetupGameOverCheck()
        {
            if (GameOverCheckTask != null)
                throw new Exception("GameOverCheckTask has already been created.");

            GameOverCheckTask = new Task(() => 
            {
                GameOverCheck();
            });
            GameOverCheckTask.Start();
        }

        void DisposeGameOverCheck()
        {
            KeepGameOverCheckScanning = false;
            GameOverCheckTask.Wait();
            GameOverCheckTask.Dispose();
        }

        Task GameOverCheckTask = null;
        bool KeepGameOverCheckScanning = true;

        // The blue team must have "\" on the start of their name.
        // The red team must have "*" on the start of their name.

        void GameOverCheck()
        {
            PlayerTeam? currentWinningTeamCheck = null;
            Stopwatch checkTime = new Stopwatch();
            int checkLength = (int)(1.5 * 1000); // 1.5 seconds in milliseconds
            bool executed = false;

            while (KeepGameOverCheckScanning)
            {
                if (OnGameOver != null)
                {
                    updateScreen(); // Start

                    PlayerTeam? thisCheck = null;

                    for (int x = 110; x < 450; x++)
                        // Test for a straight line '|'
                        if (CompareColor(x, 295, new int[] { 132, 117, 87 }, 7) && CompareColor(x, 267, new int[] { 132, 117, 87 }, 7))
                        {
                            thisCheck = PlayerTeam.Blue;
                            break;
                        }
                        // Test for just the top '*'
                        else if (CompareColor(x, 267, new int[] { 132, 117, 87 }, 7))
                        {
                            thisCheck = PlayerTeam.Red;
                            break;
                        }

                    if (thisCheck == null)
                    {
                        executed = false;
                        currentWinningTeamCheck = null;
                        checkTime.Reset();
                    }

                    else if (currentWinningTeamCheck != thisCheck)
                    {
                        executed = false;
                        currentWinningTeamCheck = thisCheck;
                        checkTime.Restart();
                    }

                    else if (currentWinningTeamCheck == thisCheck)
                    {
                        if (!executed && checkTime.ElapsedMilliseconds >= checkLength)
                        {
                            OnGameOver(this, new GameOverArgs((PlayerTeam)thisCheck));
                            executed = true;
                        }
                    }
                }
                else
                {
                    executed = false;
                    currentWinningTeamCheck = null;
                    checkTime.Reset();
                }

                Thread.Sleep(10); // End
            }
        }

        /// <summary>
        /// Events that are executed when the game is over.
        /// To get the winning team, blue team must have "\" on the start of their name, and red needs "*" on the start of their name.
        /// </summary>
        /// <example>
        /// The example below will send a message to chat when the game is over.
        /// <code>
        /// using Deltin.CustomGameAutomation;
        /// 
        /// public class OnGameOverExample
        /// {
        ///     public static void SendMessageToChatWhenGameIsOver(CustomGame cg)
        ///     {
        ///         cg.GameSettings.SetTeamName(PlayerTeam.Blue, "\ Blue Team");
        ///         cg.GameSettings.SetTeamName(PlayerTeam.Red, "* Red Team");
        ///         cg.OnGameOver += Cg_OnGameOver;
        ///     }
        ///     
        ///     private static void Cg_OnGameOver(object sender, GameOverArgs e)
        ///     {
        ///         PlayerTeam winningTeam = e.GetWinningTeam();
        ///         (sender as CustomGame).Chat.Chat(string.Format("The game is over, Team {0} has won!", winningTeam.ToString()));
        ///     }
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="GameOverArgs.GetWinningTeam"/>
        public event EventHandler<GameOverArgs> OnGameOver;
    }

    /// <summary>
    /// Arguments for the OnGameOver event that is executed when the game ends in Overwatch.
    /// </summary>
    /// <seealso cref="CustomGame.OnGameOver"/>
    public class GameOverArgs : EventArgs
    {
        private PlayerTeam WinningTeam;

        /// <summary>
        /// Arguments for the OnGameOver event that is executed when the game ends in Overwatch.
        /// </summary>
        internal GameOverArgs(PlayerTeam winningteam)
        {
            WinningTeam = winningteam;
        }

        /// <summary>
        /// Gets the team that won the Overwatch game.
        /// </summary>
        /// <returns>Returns the team that won the game.</returns>
        /// <seealso cref="CustomGame.OnGameOver"/>
        public PlayerTeam GetWinningTeam()
        {
            return WinningTeam;
        }
    }
}
