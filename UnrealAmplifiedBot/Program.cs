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
        static BotLibrary.Lindholm game;

        static void Main(string[] args)
        {
            game = new BotLibrary.Lindholm();

            game.chat.SetPrefix(">>");
            game.AddStartFunc(PrintWelcomeMessage);

            //game.SetPreset(5, 7); //fixme colors are wrong

            game.bots.RequestBot(WrapperBotTeam.Either,
                Deltin.CustomGameAutomation.AIHero.McCree, //todolater separate the enums so that they can be imported without importing customgameautomation
                Deltin.CustomGameAutomation.Difficulty.Medium,
                BotRule.SmallerTeam,
                0, 4);
            game.bots.RequestBot(WrapperBotTeam.Either, 
                Deltin.CustomGameAutomation.AIHero.Roadhog,
                Deltin.CustomGameAutomation.Difficulty.Medium,
                BotRule.SmallerTeam,
                0, 4);

            game.bots.RequestBot(WrapperBotTeam.Either,
                Deltin.CustomGameAutomation.AIHero.McCree,
                Deltin.CustomGameAutomation.Difficulty.Medium,
                BotRule.EqualTeams,
                0, 3);
            game.bots.RequestBot(WrapperBotTeam.Either,
                Deltin.CustomGameAutomation.AIHero.Roadhog,
                Deltin.CustomGameAutomation.Difficulty.Medium,
                BotRule.EqualTeams,
                0, 3);


            game.Start();


        }

        static void PrintWelcomeMessage()
        {
            game.chat.MatchChat("Beep Boop. Starting up.");
        }

    }
}
