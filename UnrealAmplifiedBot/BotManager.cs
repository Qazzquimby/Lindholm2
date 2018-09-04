using Deltin.CustomGameAutomation;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Diagnostics;

//todosoon make fullfillbotrequests understand min and max rules

namespace Lindholm
{

    public enum BotRule
    {
        SmallerTeam,
        BothTeams,
        EqualTeams,
        LargerTeam
    }

    class BotRequest
    {
        public Team BotTeam;
        public AIHero Hero;
        public Difficulty Difficulty;
        public BotRule Rule;
        public int MinPlayersOnTeam;
        public int MaxPlayersOnTeam;

        internal BotRequest(Team team, AIHero hero, Difficulty difficulty, BotRule rule, int minPlayersOnTeam, int maxPlayersOnTeam)
        {
            if (minPlayersOnTeam < 0 || minPlayersOnTeam > 5)
            {
                throw (new ArgumentOutOfRangeException("minPlayers must be in range 0 - 5 inclusive."));
            }
            if (maxPlayersOnTeam < 0 || maxPlayersOnTeam > 5)
            {
                throw (new ArgumentOutOfRangeException("maxPlayers must be in range 0 - 5 inclusive."));
            }
            if (minPlayersOnTeam > maxPlayersOnTeam)
            {
                throw (new ArgumentOutOfRangeException("minPlayers must not be greater than maxPlayers"));
            }

            BotTeam = team;
            Hero = hero;
            Difficulty = difficulty;
            Rule = rule;
            MinPlayersOnTeam = minPlayersOnTeam;
            MaxPlayersOnTeam = maxPlayersOnTeam;
        }
    }

    class BotManager : WrapperComponent
    {
        public BotManager(Lindholm wrapperInject) : base(wrapperInject) {
            wrapper.slots.bots = new BotSlots(this);
        }

        public List<int> IBlueBotSlots {
            get {
                return BotSlots[Team.Blue];
            }
        }

        public List<int> IRedBotSlots {
            get {
                return BotSlots[Team.Red];
            }
        }

        private List<BotRequest> BotRequests = new List<BotRequest>();

        private Dictionary<Team, List<BotRequest>> PrevBotExpectations = new Dictionary<Team, List<BotRequest>>()
            {
                { Team.Blue, new List<BotRequest>() },
                { Team.Red, new List<BotRequest>() },
            };

        private Dictionary<Team, List<BotRequest>> BotExpectations = new Dictionary<Team, List<BotRequest>>()
            {
                { Team.Blue, new List<BotRequest>() },
                { Team.Red, new List<BotRequest>() },
            };

        private Dictionary<Team, List<int>> BotSlots = new Dictionary<Team, List<int>>()
            {
                { Team.Blue, new List<int>() },
                { Team.Red, new List<int>() },
            };

        

        public void Start()
        {
            wrapper.players.AddJoinOrLeaveFunc(HandleBots);
            RemoveBots(); //Simplifies bot management by starting with no bots.
        }

        public void HandleBots()
        {
            if (wrapper.maps.IsDMOrTDM)
            {
                RemoveBotsIfAny();
            }
            else
            {
                UpdateBotSlots(); //Fixme, Shouldn't be needed frequently. A failsafe.
                UpdateBotExpectations();
                FulfillBotExpectations();
            }
        }

        public void RequestBot(AIHero hero, Difficulty difficulty, BotRule rule)
        {
            RequestBot(Team.Blue, hero, difficulty, rule);
            RequestBot(Team.Red, hero, difficulty, rule);
        }

        public void RequestBot(Team team, AIHero hero, Difficulty difficulty, BotRule rule)
        {
            RequestBot(team, hero, difficulty, rule, 0, 5);
        }


        public void RequestBot(AIHero hero, Difficulty difficulty, BotRule rule, int minPlayersOnTeam, int maxPlayersOnTeam)
        {
            RequestBot(Team.Blue, hero, difficulty, rule, minPlayersOnTeam, maxPlayersOnTeam);
            RequestBot(Team.Red, hero, difficulty, rule, minPlayersOnTeam, maxPlayersOnTeam);
        }


        public void RequestBot(Team team, AIHero hero, Difficulty difficulty, BotRule rule, int minPlayersOnTeam, int maxPlayersOnTeam)
        {
            BotRequest newRequest = new BotRequest(team, hero, difficulty, rule, minPlayersOnTeam, maxPlayersOnTeam);
            BotRequests.Add(newRequest);
        }

        public void ClearBotRequests()
        {
            BotRequests = new List<BotRequest>();
        }

        public void UpdateBotSlots()
        {
            UpdateBotSlots(Team.Blue);
            UpdateBotSlots(Team.Red);
        }

        private void UpdateBotSlots(Team team)
        {
            System.Threading.Thread.Sleep(500); //use cg wait for 
            List<int> FilledSlots = wrapper.slots.all.Slots(team);
            List<int> NewBotSlots = new List<int>();
            foreach (int slot in FilledSlots)
            {
                bool isAi = cg.AI.IsAI(slot);
                if (isAi)
                {
                    NewBotSlots.Add(slot);
                }
            }
            BotSlots[team] = NewBotSlots;
        }

        private void UpdateBotExpectations()
        {
            UpdateBotExpectations(Team.Blue);
            UpdateBotExpectations(Team.Red);
        }

        private void UpdateBotExpectations(Team team)
        {
            PrevBotExpectations[team] = new List<BotRequest>(BotExpectations[team]);

            BotExpectations[team] = new List<BotRequest>();

            foreach (BotRequest request in BotRequests)
            {
                if (request.BotTeam == team)
                {

                    if (request.Rule == BotRule.SmallerTeam && !wrapper.slots.TeamHasMorePlayers(team))
                    {
                        BotExpectations[team].Add(request);
                    }
                    else if (request.Rule == BotRule.EqualTeams && wrapper.slots.TeamsHaveEqualPlayers)
                    {
                        BotExpectations[team].Add(request);
                    }
                    else if (request.Rule == BotRule.LargerTeam && wrapper.slots.TeamHasMorePlayers(team))
                    {
                        BotExpectations[team].Add(request);
                    }
                    else if (request.Rule == BotRule.BothTeams)
                    {
                        BotExpectations[team].Add(request);
                    }
                }
            }
        }

        private void FulfillBotExpectations()
        {
            FulfillBotExpectations(Team.Blue);
            FulfillBotExpectations(Team.Red);
        }
        private bool IsBotStateCorrupt(Team team)
        {
            return PrevBotExpectations[team].Count != wrapper.slots.bots.Count(team);
        }

        private void FulfillBotExpectations(Team team)
        {
            if (BotExpectations[team].Count != PrevBotExpectations[team].Count)
            {
                Dev.Log(string.Format("{0} bots previously {1}, now {2}.", team.ToString(), PrevBotExpectations[team].Count, BotExpectations[team].Count));
            }

            List<BotRequest> alreadyFulfilled = new List<BotRequest>();
            List<BotRequest> TempBotExpectations = new List<BotRequest>(BotExpectations[team]);

            //is state corrupt?
            if (IsBotStateCorrupt(team))
            {
                Dev.Log(string.Format("{0} bots corrupted. Count is {1}, should be {2}", team.ToString(), wrapper.slots.bots.Count(team), PrevBotExpectations[team].Count));
                RemoveBots(team);
                foreach (BotRequest request in TempBotExpectations)
                {
                    AddBot(request);
                }
            }
            else
            {
                foreach (BotRequest oldRequest in PrevBotExpectations[team].ToList())
                {
                    bool fulfilled = false;
                    foreach (BotRequest newRequest in TempBotExpectations)
                    {
                        if (oldRequest.Hero == newRequest.Hero && oldRequest.Difficulty == newRequest.Difficulty)
                        {
                            fulfilled = true;
                            alreadyFulfilled.Add(newRequest);
                            PrevBotExpectations[team].Remove(oldRequest);
                            TempBotExpectations.Remove(newRequest);
                            break;
                        }
                    }
                    if (!fulfilled)
                    {
                        RemoveBots(team);
                        foreach (BotRequest request in TempBotExpectations)
                        {
                            AddBot(request);
                        }
                        foreach (BotRequest request in alreadyFulfilled)
                        {
                            AddBot(request);
                        }
                        break;
                    }
                }
                foreach (BotRequest request in TempBotExpectations)
                {
                    AddBot(request);
                }
            }
            UpdateBotSlots(team);
        }

        private void AddBot(BotRequest request)
        {
            AddBot(request.Hero, request.Difficulty, request.BotTeam);
        }

        private void AddBot(AIHero hero, Difficulty difficulty, Team team)
        {
            if (wrapper.slots.empty.Count(team) > 0)
            {
                cg.AI.AddAI(hero, difficulty, wrapper.TeamToBotTeam(team), 1);
            }
        }

        public void RemoveBotsIfAny()
        {
            if (wrapper.slots.bots.Count() > 0)
            {
                RemoveBots();
            }
        }

        public void RemoveBots()
        {
            cg.AI.RemoveAllBotsAuto();
            BotSlots[Team.Blue] = new List<int>();
            BotSlots[Team.Red] = new List<int>();
        }

        public void RemoveBots(Team team)
        {
            List<int> slots = wrapper.slots.bots.Slots(team);
            foreach (int slot in slots)
            {
                //todolater if returns false repair state
                cg.AI.RemoveFromGameIfAI(slot);
            }
        }
    }
}