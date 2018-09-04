namespace Lindholm.Bots
{
    internal class BotDeltinManipulator
    {
        private readonly Deltin.CustomGameAutomation.AI _ai;

        private BotDeltinManipulator(Deltin.CustomGameAutomation.AI ai)
        {
            _ai = ai;
        }

        internal void AddAi(AiHero hero, Difficulty difficulty, Team team)
        {
            _ai.AddAI(hero.ToDeltin(), difficulty.ToDeltin(), team.ToBotTeam());
        }

        internal void RemoveBots()
        {
            _ai.RemoveAllBotsAuto();
        }

        internal bool RemoveBot(int slot)
        {
            return _ai.RemoveFromGameIfAI(slot);
        }
    }
}