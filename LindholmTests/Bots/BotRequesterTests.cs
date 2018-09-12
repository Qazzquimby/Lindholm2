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
            BotRequests requests = new BotRequests();
            BotRequester sut = new BotRequester(requests);
            AiHero hero = AiHero.Reaper;
            Difficulty difficulty = Difficulty.Hard;
            IBotRule rule = new BotRuleBothTeams();

            sut.RequestBot(hero, difficulty, rule);


            Assert.AreEqual(requests.Requests.Count, 2);
        }

        [TestMethod()]
        public void TestWhenNoTeamSpecifiedThenRequestAddedForDifferentTeams()
        {
            BotRequests requests = new BotRequests();
            BotRequester sut = new BotRequester(requests);
            AiHero hero = AiHero.Reaper;
            Difficulty difficulty = Difficulty.Hard;
            IBotRule rule = new BotRuleBothTeams();

            sut.RequestBot(hero, difficulty, rule);

            Assert.AreNotEqual(requests.Requests[0].BotTeam, requests.Requests[1].BotTeam);
        }

        [TestMethod()]
        public void TestCorrectHeroInRequest()
        {
            BotRequests requests = new BotRequests();
            BotRequester sut = new BotRequester(requests);
            AiHero hero = AiHero.Reaper;
            Difficulty difficulty = Difficulty.Hard;
            IBotRule rule = new BotRuleBothTeams();

            sut.RequestBot(hero, difficulty, rule);

            Assert.AreEqual(requests.Requests[0].Hero, hero);
            Assert.AreEqual(requests.Requests[1].Hero, hero);
        }

        [TestMethod()]
        public void TestCorrectDifficultyInRequest()
        {
            BotRequests requests = new BotRequests();
            BotRequester sut = new BotRequester(requests);
            AiHero hero = AiHero.Reaper;
            Difficulty difficulty = Difficulty.Hard;
            IBotRule rule = new BotRuleBothTeams();

            sut.RequestBot(hero, difficulty, rule);

            Assert.AreEqual(requests.Requests[0].Difficulty, difficulty);
            Assert.AreEqual(requests.Requests[1].Difficulty, difficulty);
        }

        [TestMethod()]
        public void TestCorrectRuleInRequest()
        {
            BotRequests requests = new BotRequests();
            BotRequester sut = new BotRequester(requests);
            AiHero hero = AiHero.Reaper;
            Difficulty difficulty = Difficulty.Hard;
            IBotRule rule = new BotRuleBothTeams();

            sut.RequestBot(hero, difficulty, rule);

            Assert.AreEqual(requests.Requests[0].Rule, rule);
            Assert.AreEqual(requests.Requests[1].Rule, rule);
        }

        [TestMethod()]
        public void TestClearBotRequestsLeavesNoBotRequests()
        {
            BotRequests requests = new BotRequests();
            BotRequester sut = new BotRequester(requests);


            AiHero hero = AiHero.Sombra;
            Difficulty difficulty = Difficulty.Hard;
            IBotRule rule = new BotRuleSmallerTeam();
            sut.RequestBot(hero, difficulty, rule);

            AiHero hero2 = AiHero.Bastion;
            Difficulty difficulty2 = Difficulty.Easy;
            IBotRule rule2 = new BotRuleLargerTeam();
            sut.RequestBot(hero2, difficulty2, rule2);

            sut.ClearBotRequests();

            Assert.AreEqual(requests.Requests.Count, 0);
        }

        [TestMethod()]
        public void TestRequestHasCorrectTeam()
        {
            BotRequests requests = new BotRequests();
            BotRequester sut = new BotRequester(requests);


            AiHero hero = AiHero.Sombra;
            Difficulty difficulty = Difficulty.Hard;
            IBotRule rule = new BotRuleSmallerTeam();
            Team team = Team.Blue;
            sut.RequestBot(team, hero, difficulty, rule);

            Assert.AreEqual(requests.Requests[0].BotTeam, Team.Blue);
        }

        [TestMethod()]
        public void TestMinAndMaxPlayersForRequest()
        {
            BotRequests requests = new BotRequests();
            BotRequester sut = new BotRequester(requests);

            AiHero hero = AiHero.Sombra;
            Difficulty difficulty = Difficulty.Hard;
            IBotRule rule = new BotRuleSmallerTeam();
            int minPlayers = 1;
            int maxPlayers = 4;
            sut.RequestBot(hero, difficulty, rule, minPlayers, maxPlayers);

            
            Assert.AreEqual(requests.Requests[0].MinPlayersOnTeam, minPlayers);
            Assert.AreEqual(requests.Requests[0].MaxPlayersOnTeam, maxPlayers);
        }
    }
}