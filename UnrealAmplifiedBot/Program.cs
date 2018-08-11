using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BotLibrary;

namespace UnrealAmplifiedBot
{
    class Program
    {
        static CustomGameWrapper game;

        static void Main(string[] args)
        {
            game = new CustomGameWrapper();

            game.chat.SetPrefix(">>");
            game.AddStartFunc(PrintWelcomeMessage);

            //game.SetPreset(5, 7); //fixme colors are wrong

            game.bots.RequestBot(WrapperBotTeam.Red, 
                Deltin.CustomGameAutomation.AIHero.Roadhog,
                Deltin.CustomGameAutomation.Difficulty.Medium,
                BotRule.SmallerTeam);
            game.bots.RequestBot(WrapperBotTeam.Red,
                Deltin.CustomGameAutomation.AIHero.Roadhog,
                Deltin.CustomGameAutomation.Difficulty.Medium,
                BotRule.EqualTeams);

            game.bots.RequestBot(WrapperBotTeam.Red,
                Deltin.CustomGameAutomation.AIHero.McCree,
                Deltin.CustomGameAutomation.Difficulty.Medium,
                BotRule.SmallerTeam);
            game.bots.RequestBot(WrapperBotTeam.Red,
                Deltin.CustomGameAutomation.AIHero.McCree,
                Deltin.CustomGameAutomation.Difficulty.Medium,
                BotRule.EqualTeams);

            game.bots.RequestBot(WrapperBotTeam.Blue,
                Deltin.CustomGameAutomation.AIHero.Roadhog,
                Deltin.CustomGameAutomation.Difficulty.Medium,
                BotRule.SmallerTeam);
            game.bots.RequestBot(WrapperBotTeam.Blue,
                Deltin.CustomGameAutomation.AIHero.Roadhog,
                Deltin.CustomGameAutomation.Difficulty.Medium,
                BotRule.EqualTeams);

            game.bots.RequestBot(WrapperBotTeam.Blue,
                Deltin.CustomGameAutomation.AIHero.McCree,
                Deltin.CustomGameAutomation.Difficulty.Medium,
                BotRule.SmallerTeam);
            game.bots.RequestBot(WrapperBotTeam.Blue,
                Deltin.CustomGameAutomation.AIHero.McCree,
                Deltin.CustomGameAutomation.Difficulty.Medium,
                BotRule.EqualTeams);


            game.Start();


        }

        static void PrintWelcomeMessage()
        {
            game.chat.MatchChat("Beep Boop. Starting up.");
        }

    }
}
