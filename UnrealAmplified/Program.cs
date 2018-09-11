using Lindholm;
using Lindholm.Bots;
using Lindholm.Phases;

namespace UnrealAmplified
{
    public class Program
    {
        public static Game Game;

        public static Phase SetupPhase;

        public static void Main(string[] args)
        {
            Game = new Game();


            Game.Chat.Decorator.SetPrefix(">>");
            Game.AddStartFunc(PrintWelcomeMessage);
            //game.SetPreset(5, 7); //fixme colors are wrong

            SetBotRequests();

            SetupPhase = BuildSetupPhase();


            Game.Start();
        }

        private static void PrintWelcomeMessage()
        {
            Game.Chat.Print("Beep Boop. Starting up.");
        }

        private static void SetBotRequests()
        {
            Game.Bots.BotRequester.RequestBot(AiHero.McCree,
                Difficulty.Medium,
                new BotRuleSmallerTeam(),
                0, 4);
            Game.Bots.BotRequester.RequestBot(AiHero.Roadhog,
                Difficulty.Medium,
                new BotRuleSmallerTeam(),
                0, 4);

            Game.Bots.BotRequester.RequestBot(AiHero.McCree,
                Difficulty.Medium,
                new BotRuleEqualTeams(), 
                0, 3);
            Game.Bots.BotRequester.RequestBot(AiHero.Roadhog,
                Difficulty.Medium,
                new BotRuleEqualTeams(), 
                0, 3);
        }


        private static Phase BuildSetupPhase()
        {
            Phase newSetUpPhase = new Phase("SETUP");
//            newSetUpPhase.AddEntry(Game.Logger.EndMatchLog);
//            newSetUpPhase.AddEntry(Game.Slots.PurgeAllSlotsHistory);
//            newSetUpPhase.AddEntry(Game.Players.scrambler.ScrambleTeams);
//            newSetUpPhase.AddEntry(SetGameNotEnding);

//            newSetUpPhase.AddLoop(Game.BotSlots.UpdateAllSlotsHistory, 50);
//            newSetUpPhase.AddLoop(Game.Players.balancer.SwapToBalance, 50);
//            newSetUpPhase.AddLoop(Game.Bots.HandleBots, 50);

//            newSetUpPhase.AddDelay(EndSetUpPhase, 250);

            return newSetUpPhase;
        }

//        private static Phase GamePhaseConstructor()
//        {
//            Phase newGamePhase = new Phase("GAME");
//            newGamePhase.AddEntry(EnsureMatchChat);
//
//            newGamePhase.AddLoop(Game.BotSlots.UpdateAllSlotsHistory, 50);
//            newGamePhase.AddLoop(Game.Players.UpdatePlayers, TimeConstants.StandardDelay);
//            newGamePhase.AddLoop(Game.Players.HandleJoinsAndLeaves, TimeConstants.StandardDelay);
//
//            newGamePhase.AddLoop(Game.Logger.UpdatematchLog, TimeConstants.StandardDelay);
//            newGamePhase.AddLoop(Game.Players.balancer.PerformAutoBalance, TimeConstants.StandardDelay);
//            newGamePhase.AddLoop(Game.Match.PreventMapTimeout, 300);
//            newGamePhase.AddLoop(Game.Players.balancer.BeginOrEndAutobalance, 50);
//            newGamePhase.AddLoop(Game.Bots.HandleBots, 50);
//            newGamePhase.AddLoop(HandleGameOver, TimeConstants.StandardDelay);
//            newGamePhase.AddLoop(HandleMissedGameOver, TimeConstants.StandardDelay);
//
//            return newGamePhase;
//        }
//
//        static void EnsureMatchChat()
//        {
//            Game.Chat.ChannelSwapper.SwapChannel(Deltin.CustomGameAutomation.Channel.Match);
//        }
//
//        static Phase GameEndingPhaseConstructor()
//        {
//            Phase newGameEndingPhase = new Phase("GAME ENDING");
//            newGameEndingPhase.AddEntry(Game.Match.PerformGameOverFuncs);
//            newGameEndingPhase.AddEntry(SkipPostGame);
//            return newGameEndingPhase;
//        }
//
//
//        static void SkipPostGame()
//        {
//            int wait_timer = 0;
//            while (Game.CG.GetGameState() != Deltin.CustomGameAutomation.GameState.Ending_Commend && wait_timer < 20)
//            {
//                System.Threading.Thread.Sleep(1000);
//                wait_timer++;
//                System.Diagnostics.Debug.WriteLine(string.Format("{0} seconds wait", wait_timer));
//            }
//            Game.Maps.NextMap();
//        }
//
//        static void SetGameNotEnding()
//        {
//            Game.Match.gameEnded = false;
//        }
//
//        static void HandleGameOver()
//        {
//            if (Game.Match.gameEnded)
//            {
//                Dev.Log("Entering gameEndingPhase");
//                Game.Phases.gameEnterPhase(gameEndingPhase);
//            }
//        }
//
//        static void HandleMissedGameOver()
//        {
//            if (cg.GetGameState() == GameState.Ending_Commend)
//            {
//                Dev.Log("Missed game over");
//                Game.Match.HandleArgumentlessGameOver();
//            }
//        }
//
//        static void EndSetUpPhase()
//        {
//            Dev.Log("Entering game phase");
//            EnterPhase(gamePhase);
//        }


    }
}
