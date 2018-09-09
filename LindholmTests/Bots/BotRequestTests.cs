using System;
using Lindholm;
using Lindholm.Bots;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LindholmTests.Bots
{
    [TestClass()]
    public class BotRequestTests
    {
        [TestMethod()]
        public void TestIfDifferentHeroNotReturnEqual()
        {
            Team team = Team.Blue;
            AiHero hero1 = AiHero.Ana;
            AiHero hero2 = AiHero.Bastion;
            IBotRule rule = new BotRuleEqualTeams();
            Difficulty difficulty = Difficulty.Medium;
            int min = 0;
            int max = 3;

            BotRequest sut1 = new BotRequest(team, hero1, difficulty, rule, min, max);
            BotRequest sut2 = new BotRequest(team, hero2, difficulty, rule, min, max);

            Assert.AreNotEqual(sut1, sut2);
        }

        [TestMethod()]
        public void TestIfDifferentDifficultyNotReturnEqual()
        {
            Team team = Team.Blue;
            AiHero hero = AiHero.Ana;
            IBotRule rule = new BotRuleEqualTeams();
            Difficulty difficulty1 = Difficulty.Medium;
            Difficulty difficulty2 = Difficulty.Hard;
            int min = 3;
            int max = 3;

            BotRequest sut1 = new BotRequest(team, hero, difficulty1, rule, min, max);
            BotRequest sut2 = new BotRequest(team, hero, difficulty2, rule, min, max);

            Assert.AreNotEqual(sut1, sut2);
        }

        [TestMethod()]
        public void TestIfSameReturnEqual()
        {
            Team team = Team.Blue;
            AiHero hero = AiHero.Ana;
            IBotRule rule = new BotRuleEqualTeams();
            Difficulty difficulty = Difficulty.Medium;
            int min = 2;
            int max = 4;

            BotRequest sut1 = new BotRequest(team, hero, difficulty, rule, min, max);
            BotRequest sut2 = new BotRequest(team, hero, difficulty, rule, min, max);

            Assert.IsTrue(sut1.Equals(sut2));
        }

        public void InitTooSmallMin()
        {
            Team team = Team.Blue;
            AiHero hero = AiHero.Ana;
            Difficulty difficulty = Difficulty.Medium;
            IBotRule rule = new BotRuleEqualTeams();
            int min = -1;
            int max = 4;
            new BotRequest(team, hero, difficulty, rule, min, max);
        }

        [TestMethod()]
        public void TestTooSmallMinimumThrows()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>( (Action) InitTooSmallMin );           
        }

        public void InitTooLargeMax()
        {
            Team team = Team.Blue;
            AiHero hero = AiHero.Ana;
            Difficulty difficulty = Difficulty.Medium;
            IBotRule rule = new BotRuleEqualTeams();
            int min = 0;
            int max = 6;
            new BotRequest(team, hero, difficulty, rule, min, max);
        }

        [TestMethod()]
        public void TestTooLargeMaximumThrows()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>((Action)InitTooLargeMax);
        }


        public void InitMinGreaterThanMax()
        {
            Team team = Team.Blue;
            AiHero hero = AiHero.Ana;
            Difficulty difficulty = Difficulty.Medium;
            IBotRule rule = new BotRuleEqualTeams();
            int min = 4;
            int max = 3;
            new BotRequest(team, hero, difficulty, rule, min, max);
        }

        [TestMethod()]
        public void TestMinGreaterThanMaxThrows()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>((Action)InitMinGreaterThanMax);
        }


    }
}