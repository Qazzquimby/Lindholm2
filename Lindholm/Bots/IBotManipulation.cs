using System.Collections.Generic;

namespace Lindholm.Bots
{
    internal interface IBotManipulation
    {
        void AddBots(List<BotRequest> requests);
        void AddBot(BotRequest request);
        void AddBot(AiHero hero, Difficulty difficulty, Team team);
        void RemoveBots();
        void RemoveBots(Team team);
    }
}