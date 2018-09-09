namespace Lindholm.Bots
{
    internal interface IBotDeltinManipulator
    {
        void AddAi(AiHero hero, Difficulty difficulty, Team team);
        void RemoveBots();
        bool RemoveBot(int slot);
    }
}