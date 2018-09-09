namespace Lindholm.Bots
{
    internal class BotDeltinManipulator : IBotDeltinManipulator
    {
        private readonly Deltin.CustomGameAutomation.AI _ai;

        public BotDeltinManipulator(Deltin.CustomGameAutomation.AI ai)
        {
            _ai = ai;
        }

        public void AddAi(AiHero hero, Difficulty difficulty, Team team)
        {
            _ai.AddAI(hero.ToDeltin(), difficulty.ToDeltin(), team.ToBotTeam());
        }

        public void RemoveBots()
        {
            _ai.RemoveAllBotsAuto();
        }

        public bool RemoveBot(int slot)
        {
            return _ai.RemoveFromGameIfAI(slot);
        }
    }
}