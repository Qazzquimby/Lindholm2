using System;
using System.Collections.Generic;
using Lindholm;
using Lindholm.Bots;
using Lindholm.Slots;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LindholmTests.Bots
{
    [TestClass()]
    public class BotRequestFulfillmentManagerTests
    {
        class FakeSlotContentHistory : ISlotContentHistory
        {
            public int BlueCount;
            public int RedCount;
            public void SetUpFake(Dictionary<Team, List<BotRequest>> expectations)
            {
                BlueCount = expectations[Team.Blue].Count;
                RedCount = expectations[Team.Red].Count;
            }

            public SlotContent Current(int slot)
            {
                if (slot < 6)
                {
                    if (slot < BlueCount)
                    {
                        return SlotContent.Bot;
                    }
                    else
                    {
                        return SlotContent.Empty;
                    }
                }
                else
                {
                    if (slot < 6 + RedCount)
                    {
                        return SlotContent.Bot;
                    }
                    else
                    {
                        return SlotContent.Empty;
                    }
                }
            }

            public List<SlotContent> History(int slot)
            {
                throw new System.NotImplementedException();
            }

            public void Update(List<SlotContent> slots)
            {
                throw new System.NotImplementedException();
            }

            public void PurgeHistory()
            {
                throw new System.NotImplementedException();
            }
        }

        class FakeExpectations : IBotExpectations
        {
            public Dictionary<Team, List<BotRequest>> Expectations { get; set; }
            public Dictionary<Team, List<BotRequest>> PreviousExpectations { get; set; }

            public void UpdateBotExpectations()
            {
                throw new System.NotImplementedException();
            }
        }

        class FakeBotManipulation : IBotManipulation
        {
            public int AddedBotsCount = 0;
            public int RemoveBotsCount = 0;
            

            public void AddBots(List<BotRequest> requests)
            {
                foreach (BotRequest request in requests)
                {
                    AddBot(request);
                }
            }

            public void AddBot(BotRequest request)
            {
                AddedBotsCount++;
            }

            public void AddBot(AiHero hero, Difficulty difficulty, Team team)
            {
                AddedBotsCount++;
            }

            public void RemoveBots()
            {
                RemoveBotsCount++;
                AddedBotsCount = 0;
            }

            public void RemoveBots(Team team)
            {
                RemoveBotsCount++;
            }
        }

        class FakeBotSlotManager : IBotSlotManager
        {
            public bool BotsModified { get; set; }
            public BotSlots BotSlots { get; set; }
        }

        [TestMethod()]
        public void TestWhenNoChangeInRequestsThenNoAddsOrRemovals()
        {

            Dictionary<Team, List<BotRequest>> previousExpectations = new Dictionary<Team, List<BotRequest>>()
            {
                {
                    Team.Blue, new List<BotRequest>()
                    {
                        new BotRequest(Team.Blue, AiHero.Ana, Difficulty.Easy, new BotRuleBothTeams(), 0, 4),
                        new BotRequest(Team.Blue, AiHero.Bastion, Difficulty.Easy, new BotRuleBothTeams(), 0, 4),
                        new BotRequest(Team.Blue, AiHero.Ana, Difficulty.Hard, new BotRuleBothTeams(), 0, 4),
                    }
                },
                {
                    Team.Red, new List<BotRequest>()
                {
                    new BotRequest(Team.Red, AiHero.Reaper, Difficulty.Easy, new BotRuleBothTeams(), 0, 4),
                    new BotRequest(Team.Red, AiHero.Reaper, Difficulty.Easy, new BotRuleBothTeams(), 0, 4),
                }},
            };

            Dictionary<Team, List<BotRequest>> Expectations = new Dictionary<Team, List<BotRequest>>()
            {
                {
                    Team.Blue, new List<BotRequest>()
                    {
                        new BotRequest(Team.Blue, AiHero.Ana, Difficulty.Easy, new BotRuleBothTeams(), 0, 4),
                        new BotRequest(Team.Blue, AiHero.Bastion, Difficulty.Easy, new BotRuleBothTeams(), 0, 4),
                        new BotRequest(Team.Blue, AiHero.Ana, Difficulty.Hard, new BotRuleBothTeams(), 0, 4),
                        new BotRequest(Team.Blue, AiHero.Lucio, Difficulty.Hard, new BotRuleBothTeams(), 0, 4),
                    }
                },
                {
                    Team.Red, new List<BotRequest>()
                    {
                        new BotRequest(Team.Red, AiHero.Reaper, Difficulty.Easy, new BotRuleBothTeams(), 0, 4),
                        new BotRequest(Team.Red, AiHero.Reaper, Difficulty.Easy, new BotRuleBothTeams(), 0, 4),
                        new BotRequest(Team.Red, AiHero.Ana, Difficulty.Hard, new BotRuleBothTeams(), 0, 4),
                    }
                },
            };


            FakeExpectations fakeExpectations = new FakeExpectations();
            fakeExpectations.Expectations = Expectations;
            fakeExpectations.PreviousExpectations = Expectations;


            FakeBotSlotManager fakeBotSlotManager = new FakeBotSlotManager();

            FakeSlotContentHistory fakeSlotContentHistory = new FakeSlotContentHistory();
            fakeSlotContentHistory.SetUpFake(Expectations);
            BotSlots fakedBotSlots = new BotSlots(fakeSlotContentHistory);
            fakeBotSlotManager.BotSlots = fakedBotSlots;

            FakeBotManipulation fakeManipulation = new FakeBotManipulation();


            BotRequestFulfillmentManager sut = new BotRequestFulfillmentManager(fakeExpectations, fakeManipulation, fakeBotSlotManager);

            sut.FulfillBotExpectations();

            Assert.AreEqual(0, fakeManipulation.AddedBotsCount);
            Assert.AreEqual(0, fakeManipulation.RemoveBotsCount);

        }

        [TestMethod()]
        public void TestWhenCorruptRemovesAndAddsNewRequests()
        {

            Dictionary<Team, List<BotRequest>> previousExpectations = new Dictionary<Team, List<BotRequest>>()
            {
                {
                    Team.Blue, new List<BotRequest>()
                    {
                        new BotRequest(Team.Blue, AiHero.Ana, Difficulty.Easy, new BotRuleBothTeams(), 0, 4),
                        new BotRequest(Team.Blue, AiHero.Bastion, Difficulty.Easy, new BotRuleBothTeams(), 0, 4),
                        new BotRequest(Team.Blue, AiHero.Ana, Difficulty.Hard, new BotRuleBothTeams(), 0, 4),
                    }
                },
                {
                    Team.Red, new List<BotRequest>()
                {
                    new BotRequest(Team.Red, AiHero.Reaper, Difficulty.Easy, new BotRuleBothTeams(), 0, 4),
                    new BotRequest(Team.Red, AiHero.Reaper, Difficulty.Easy, new BotRuleBothTeams(), 0, 4),
                }},
            };

            Dictionary<Team, List<BotRequest>> expectations = new Dictionary<Team, List<BotRequest>>()
            {
                {
                    Team.Blue, new List<BotRequest>()
                    {
                        new BotRequest(Team.Blue, AiHero.Ana, Difficulty.Easy, new BotRuleBothTeams(), 0, 4),
                        new BotRequest(Team.Blue, AiHero.Bastion, Difficulty.Easy, new BotRuleBothTeams(), 0, 4),
                        new BotRequest(Team.Blue, AiHero.Ana, Difficulty.Hard, new BotRuleBothTeams(), 0, 4),
                        new BotRequest(Team.Blue, AiHero.Lucio, Difficulty.Hard, new BotRuleBothTeams(), 0, 4),
                    }
                },
                {
                    Team.Red, new List<BotRequest>()
                    {
                        new BotRequest(Team.Red, AiHero.Reaper, Difficulty.Easy, new BotRuleBothTeams(), 0, 4),
                        new BotRequest(Team.Red, AiHero.Reaper, Difficulty.Easy, new BotRuleBothTeams(), 0, 4),
                        new BotRequest(Team.Red, AiHero.Ana, Difficulty.Hard, new BotRuleBothTeams(), 0, 4),
                    }
                },
            };


            FakeExpectations fakeExpectations = new FakeExpectations();
            fakeExpectations.Expectations = expectations;
            fakeExpectations.PreviousExpectations = previousExpectations;


            FakeBotSlotManager fakeBotSlotManager = new FakeBotSlotManager();

            FakeSlotContentHistory fakeSlotContentHistory = new FakeSlotContentHistory();
            fakeSlotContentHistory.SetUpFake(expectations);
            BotSlots fakedBotSlots = new BotSlots(fakeSlotContentHistory);
            fakeBotSlotManager.BotSlots = fakedBotSlots;

            FakeBotManipulation fakeManipulation = new FakeBotManipulation();


            BotRequestFulfillmentManager sut = new BotRequestFulfillmentManager(fakeExpectations, fakeManipulation, fakeBotSlotManager);

            sut.FulfillBotExpectations();

            Assert.AreEqual(7, fakeManipulation.AddedBotsCount);
            Assert.AreEqual(2, fakeManipulation.RemoveBotsCount);

        }


        [TestMethod()]
        public void TestAddsMissingBots()
        {

            Dictionary<Team, List<BotRequest>> previousExpectations = new Dictionary<Team, List<BotRequest>>()
            {
                {
                    Team.Blue, new List<BotRequest>()
                    {
                        new BotRequest(Team.Blue, AiHero.Ana, Difficulty.Easy, new BotRuleBothTeams(), 0, 4),
                        new BotRequest(Team.Blue, AiHero.Bastion, Difficulty.Easy, new BotRuleBothTeams(), 0, 4),
                        new BotRequest(Team.Blue, AiHero.Ana, Difficulty.Hard, new BotRuleBothTeams(), 0, 4),
                    }
                },
                {
                    Team.Red, new List<BotRequest>()
                {
                    new BotRequest(Team.Red, AiHero.Reaper, Difficulty.Easy, new BotRuleBothTeams(), 0, 4),
                    new BotRequest(Team.Red, AiHero.Reaper, Difficulty.Easy, new BotRuleBothTeams(), 0, 4),
                }},
            };

            Dictionary<Team, List<BotRequest>> expectations = new Dictionary<Team, List<BotRequest>>()
            {
                {
                    Team.Blue, new List<BotRequest>()
                    {
                        new BotRequest(Team.Blue, AiHero.Ana, Difficulty.Easy, new BotRuleBothTeams(), 0, 4),
                        new BotRequest(Team.Blue, AiHero.Bastion, Difficulty.Easy, new BotRuleBothTeams(), 0, 4),
                        new BotRequest(Team.Blue, AiHero.Ana, Difficulty.Hard, new BotRuleBothTeams(), 0, 4),
                        new BotRequest(Team.Blue, AiHero.Lucio, Difficulty.Hard, new BotRuleBothTeams(), 0, 4),
                    }
                },
                {
                    Team.Red, new List<BotRequest>()
                    {
                        new BotRequest(Team.Red, AiHero.Reaper, Difficulty.Easy, new BotRuleBothTeams(), 0, 4),
                        new BotRequest(Team.Red, AiHero.Reaper, Difficulty.Easy, new BotRuleBothTeams(), 0, 4),
                        new BotRequest(Team.Red, AiHero.Ana, Difficulty.Hard, new BotRuleBothTeams(), 0, 4),
                    }
                },
            };


            FakeExpectations fakeExpectations = new FakeExpectations();
            fakeExpectations.Expectations = expectations;
            fakeExpectations.PreviousExpectations = previousExpectations;
            
            FakeBotSlotManager fakeBotSlotManager = new FakeBotSlotManager();

            FakeSlotContentHistory fakeSlotContentHistory = new FakeSlotContentHistory();
            fakeSlotContentHistory.SetUpFake(previousExpectations);

            BotSlots fakedBotSlots = new BotSlots(fakeSlotContentHistory);
            fakeBotSlotManager.BotSlots = fakedBotSlots;

            FakeBotManipulation fakeManipulation = new FakeBotManipulation();


            BotRequestFulfillmentManager sut = new BotRequestFulfillmentManager(fakeExpectations, fakeManipulation, fakeBotSlotManager);

            sut.FulfillBotExpectations();

            Assert.AreEqual(2, fakeManipulation.AddedBotsCount);
            Assert.AreEqual(0, fakeManipulation.RemoveBotsCount);

        }

        [TestMethod()]
        public void TestWhenSomeBotsAreNoLongerNeededThenRemovesAllAndAddsNew()
        {

            Dictionary<Team, List<BotRequest>> previousExpectations = new Dictionary<Team, List<BotRequest>>()
            {
                {
                    Team.Blue, new List<BotRequest>()
                    {
                        new BotRequest(Team.Blue, AiHero.Ana, Difficulty.Easy, new BotRuleBothTeams(), 0, 4),
                        new BotRequest(Team.Blue, AiHero.Bastion, Difficulty.Easy, new BotRuleBothTeams(), 0, 4),
                        new BotRequest(Team.Blue, AiHero.Ana, Difficulty.Hard, new BotRuleBothTeams(), 0, 4),
                        new BotRequest(Team.Blue, AiHero.Ana, Difficulty.Hard, new BotRuleBothTeams(), 0, 4),
                    }
                },
                {
                    Team.Red, new List<BotRequest>()
                {
                    new BotRequest(Team.Red, AiHero.Reaper, Difficulty.Easy, new BotRuleBothTeams(), 0, 4),
                    new BotRequest(Team.Red, AiHero.Reaper, Difficulty.Easy, new BotRuleBothTeams(), 0, 4),
                }},
            };

            Dictionary<Team, List<BotRequest>> expectations = new Dictionary<Team, List<BotRequest>>()
            {
                {
                    Team.Blue, new List<BotRequest>()
                    {
                        new BotRequest(Team.Blue, AiHero.Ana, Difficulty.Easy, new BotRuleBothTeams(), 0, 4),
                        new BotRequest(Team.Blue, AiHero.Bastion, Difficulty.Easy, new BotRuleBothTeams(), 0, 4),
                        new BotRequest(Team.Blue, AiHero.Ana, Difficulty.Hard, new BotRuleBothTeams(), 0, 4),
                        new BotRequest(Team.Blue, AiHero.Lucio, Difficulty.Hard, new BotRuleBothTeams(), 0, 4),
                    }
                },
                {
                    Team.Red, new List<BotRequest>()
                    {
                        new BotRequest(Team.Red, AiHero.Reaper, Difficulty.Easy, new BotRuleBothTeams(), 0, 4),
                        new BotRequest(Team.Red, AiHero.Reaper, Difficulty.Easy, new BotRuleBothTeams(), 0, 4),
                        new BotRequest(Team.Red, AiHero.Ana, Difficulty.Hard, new BotRuleBothTeams(), 0, 4),
                    }
                },
            };


            FakeExpectations fakeExpectations = new FakeExpectations();
            fakeExpectations.Expectations = expectations;
            fakeExpectations.PreviousExpectations = previousExpectations;

            FakeBotSlotManager fakeBotSlotManager = new FakeBotSlotManager();

            FakeSlotContentHistory fakeSlotContentHistory = new FakeSlotContentHistory();
            fakeSlotContentHistory.SetUpFake(previousExpectations);

            BotSlots fakedBotSlots = new BotSlots(fakeSlotContentHistory);
            fakeBotSlotManager.BotSlots = fakedBotSlots;

            FakeBotManipulation fakeManipulation = new FakeBotManipulation();


            BotRequestFulfillmentManager sut = new BotRequestFulfillmentManager(fakeExpectations, fakeManipulation, fakeBotSlotManager);

            sut.FulfillBotExpectations();

            Assert.AreEqual(5, fakeManipulation.AddedBotsCount);
            Assert.AreEqual(1, fakeManipulation.RemoveBotsCount);

        }


    }
}