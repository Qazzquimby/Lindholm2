using System.Collections.Generic;
using Lindholm;
using Lindholm.Bots;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LindholmTests.Bots
{
    [TestClass()]
    public class BotRequesterTests
    {
        [TestMethod()]
        public void TestWhenNoTeamSpecifiedThenRequestAddedToBothTeams()
        {
            BotRequester sut = new BotRequester();
            AiHero hero = AiHero.Reaper;
            Difficulty difficulty = Difficulty.Hard;
            IBotRule rule = new BotRuleBothTeams();

            sut.RequestBot(hero, difficulty, rule);


            Assert.AreEqual(sut.BotRequests.Count, 2);
        }

        [TestMethod()]
        public void TestWhenNoTeamSpecifiedThenRequestAddedForDifferentTeams()
        {
            BotRequester sut = new BotRequester();
            AiHero hero = AiHero.Reaper;
            Difficulty difficulty = Difficulty.Hard;
            IBotRule rule = new BotRuleBothTeams();

            sut.RequestBot(hero, difficulty, rule);

            Assert.AreNotEqual(sut.BotRequests[0].BotTeam, sut.BotRequests[1].BotTeam);
        }

        [TestMethod()]
        public void TestCorrectHeroInRequest()
        {
            BotRequester sut = new BotRequester();
            AiHero hero = AiHero.Reaper;
            Difficulty difficulty = Difficulty.Hard;
            IBotRule rule = new BotRuleBothTeams();

            sut.RequestBot(hero, difficulty, rule);

            Assert.AreEqual(sut.BotRequests[0].Hero, hero);
            Assert.AreEqual(sut.BotRequests[1].Hero, hero);
        }

        [TestMethod()]
        public void TestCorrectDifficultyInRequest()
        {
            BotRequester sut = new BotRequester();
            AiHero hero = AiHero.Reaper;
            Difficulty difficulty = Difficulty.Hard;
            IBotRule rule = new BotRuleBothTeams();

            sut.RequestBot(hero, difficulty, rule);

            Assert.AreEqual(sut.BotRequests[0].Difficulty, difficulty);
            Assert.AreEqual(sut.BotRequests[1].Difficulty, difficulty);
        }

        [TestMethod()]
        public void TestCorrectRuleInRequest()
        {
            BotRequester sut = new BotRequester();
            AiHero hero = AiHero.Reaper;
            Difficulty difficulty = Difficulty.Hard;
            IBotRule rule = new BotRuleBothTeams();

            sut.RequestBot(hero, difficulty, rule);

            Assert.AreEqual(sut.BotRequests[0].Rule, rule);
            Assert.AreEqual(sut.BotRequests[1].Rule, rule);
        }

        [TestMethod()]
        public void TestClearBotRequestsLeavesNoBotRequests()
        {
            BotRequester sut = new BotRequester();


            AiHero hero = AiHero.Sombra;
            Difficulty difficulty = Difficulty.Hard;
            IBotRule rule = new BotRuleSmallerTeam();
            sut.RequestBot(hero, difficulty, rule);

            AiHero hero2 = AiHero.Bastion;
            Difficulty difficulty2 = Difficulty.Easy;
            IBotRule rule2 = new BotRuleLargerTeam();
            sut.RequestBot(hero2, difficulty2, rule2);

            sut.ClearBotRequests();

            Assert.AreEqual(sut.BotRequests.Count, 0);
        }

        [TestMethod()]
        public void TestRequestHasCorrectTeam()
        {
            BotRequester sut = new BotRequester();


            AiHero hero = AiHero.Sombra;
            Difficulty difficulty = Difficulty.Hard;
            IBotRule rule = new BotRuleSmallerTeam();
            Team team = Team.Blue;
            sut.RequestBot(team, hero, difficulty, rule);

            Assert.AreEqual(sut.BotRequests[0].BotTeam, Team.Blue);
        }

        [TestMethod()]
        public void TestMinAndMaxPlayersForRequest()
        {
            BotRequester sut = new BotRequester();

            AiHero hero = AiHero.Sombra;
            Difficulty difficulty = Difficulty.Hard;
            IBotRule rule = new BotRuleSmallerTeam();
            int minPlayers = 1;
            int maxPlayers = 4;
            sut.RequestBot(hero, difficulty, rule, minPlayers, maxPlayers);

            
            Assert.AreEqual(sut.BotRequests[0].MinPlayersOnTeam, minPlayers);
            Assert.AreEqual(sut.BotRequests[0].MaxPlayersOnTeam, maxPlayers);
        }
    }
}