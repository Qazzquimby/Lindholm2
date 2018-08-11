using Deltin.CustomGameAutomation;
using System.Collections.Generic;
using System.Linq;

namespace BotLibrary
{

    public enum BotRule
    {
        SmallerTeam,
        BothTeams,
        EqualTeams,
        LargerTeam
    }

    public enum WrapperBotTeam
    {
        Blue,
        Red
    }

    class BotRequest
    {
        public WrapperBotTeam Team;
        public AIHero Hero;
        public Difficulty Difficulty;
        public BotRule Rule;

        public BotRequest(WrapperBotTeam team, AIHero hero, Difficulty difficulty, BotRule rule)
        {
            Team = team;
            Hero = hero;
            Difficulty = difficulty;
            Rule = rule;
        }
    }

    class BotManager : WrapperComponent
    {
        public List<int> IBlueBotSlots
        {
            get
            {
                return BlueBotSlots;
            }
        }

        public List<int> IRedBotSlots
        {
            get
            {
                return RedBotSlots;
            }
        }

        public int maximumPlayersBeforeBotRemoval = 8;

        public bool dontAddBotsToLargerTeam = true;
        public bool addBotsToBothTeamsWhenEven = true;

        private List<BotRequest> BotRequests = new List<BotRequest>();

        private List<BotRequest> PrevBlueBotExpectations = new List<BotRequest>();
        private List<BotRequest> PrevRedBotExpectations = new List<BotRequest>();

        private List<BotRequest> BlueBotExpectations = new List<BotRequest>();
        private List<BotRequest> RedBotExpectations = new List<BotRequest>();

        private List<int> BlueBotSlots = new List<int>();
        private List<int> RedBotSlots = new List<int>();

        public BotManager(CustomGameWrapper wrapperInject) : base(wrapperInject)
        {
            
        }

        public void Start()
        {
            wrapper.players.AddJoinOrLeaveFunc(HandleBots);
            RemoveBots(); //Simplifies bot management by starting with no bots.
        }

        public void HandleBots()
        {
            UpdateBotSlots(); //Fixme, Shouldn't be needed frequently. A failsafe.
            if (wrapper.maps.currMode == Gamemode.TeamDeathmatch)
            {
                RemoveBotsIfAny();
            }
            else if (wrapper.slots.PlayerCount >= maximumPlayersBeforeBotRemoval)
            {
                Debug.Log(string.Format("{0} players >= {1} maximum. Removing bots.", wrapper.slots.PlayerCount, maximumPlayersBeforeBotRemoval));
                RemoveBotsIfAny();
            }
            else
            {
                UpdateBotExpectations();
                FulfillBotExpectations();
            }
        }

        public void RequestBot(WrapperBotTeam team, AIHero hero, Difficulty difficulty, BotRule rule)
        {
            BotRequest newRequest = new BotRequest(team, hero, difficulty, rule);
            BotRequests.Add(newRequest);
        }

        public void ClearBotRequests()
        {
            BotRequests = new List<BotRequest>();
        }

        public void UpdateBotSlots()
        {
            UpdateBlueBotSlots();
            UpdateRedBotSlots();
        }

        private void UpdateBotExpectations()
        {
            PrevBlueBotExpectations = new List<BotRequest>(BlueBotExpectations);
            PrevRedBotExpectations = new List<BotRequest>(RedBotExpectations);

            BlueBotExpectations = new List<BotRequest>();
            RedBotExpectations = new List<BotRequest>();

            foreach (BotRequest request in BotRequests)
            {
                if (request.Rule == BotRule.SmallerTeam)
                {
                    if (request.Team == WrapperBotTeam.Blue && wrapper.slots.RedHasMorePlayers)
                    {
                        BlueBotExpectations.Add(request);
                    }
                    else if (request.Team == WrapperBotTeam.Red && wrapper.slots.BlueHasMorePlayers)
                    {
                        RedBotExpectations.Add(request);
                    }
                }
                else if (request.Rule == BotRule.BothTeams)
                {
                    if (request.Team == WrapperBotTeam.Blue)
                    {
                        BlueBotExpectations.Add(request);
                    }
                    else if (request.Team == WrapperBotTeam.Red)
                    {
                        RedBotExpectations.Add(request);
                    }
                }
                else if (request.Rule == BotRule.EqualTeams)
                {
                    if (request.Team == WrapperBotTeam.Blue && wrapper.slots.TeamsHaveEqualPlayers)
                    {
                        BlueBotExpectations.Add(request);
                    }
                    else if (request.Team == WrapperBotTeam.Red && wrapper.slots.TeamsHaveEqualPlayers)
                    {
                        RedBotExpectations.Add(request);
                    }
                }
                else if (request.Rule == BotRule.LargerTeam)
                {
                    if (request.Team == WrapperBotTeam.Blue && wrapper.slots.BlueHasMorePlayers)
                    {
                        BlueBotExpectations.Add(request);
                    }
                    else if (request.Team == WrapperBotTeam.Red && wrapper.slots.RedHasMorePlayers)
                    {
                        RedBotExpectations.Add(request);
                    }
                }
            }
        }

        private void FulfillBotExpectations()
        {
            if (BlueBotExpectations.Count != PrevBlueBotExpectations.Count)
            {
                Debug.Log(string.Format("Blue bots previously {0}, now {1}.", PrevBlueBotExpectations.Count, BlueBotExpectations.Count));
            }
            if (RedBotExpectations.Count != PrevRedBotExpectations.Count)
            {
                Debug.Log(string.Format("Red bots previously {0}, now {1}.", PrevRedBotExpectations.Count, RedBotExpectations.Count));
            }

            List<BotRequest> alreadyFulfilled = new List<BotRequest>();
            List<BotRequest> TempBlueBotExpectations = new List<BotRequest>(BlueBotExpectations);

            //is state corrupt?
            if(PrevBlueBotExpectations.Count != wrapper.slots.BlueBotCount)
            {
                Debug.Log(string.Format("Blue bots corrupted. Count is {0}, should be {1}", wrapper.slots.BlueBotCount, PrevBlueBotExpectations.Count));
                RemoveBlueBots();
                foreach (BotRequest request in TempBlueBotExpectations)
                {
                    BlueAddBot(request);
                }
            }
            else
            {
                foreach (BotRequest oldRequest in PrevBlueBotExpectations.ToList())
                {
                    bool fulfilled = false;
                    foreach (BotRequest newRequest in TempBlueBotExpectations)
                    {
                        if (oldRequest.Hero == newRequest.Hero && oldRequest.Difficulty == newRequest.Difficulty)
                        {
                            fulfilled = true;
                            alreadyFulfilled.Add(newRequest);
                            PrevBlueBotExpectations.Remove(oldRequest);
                            TempBlueBotExpectations.Remove(newRequest);
                            break;
                        }
                    }
                    if (!fulfilled)
                    {
                        RemoveBlueBots();
                        foreach (BotRequest request in TempBlueBotExpectations)
                        {
                            BlueAddBot(request);
                        }
                        foreach (BotRequest request in alreadyFulfilled)
                        {
                            BlueAddBot(request);
                        }
                        break;
                    }
                }
                foreach (BotRequest request in TempBlueBotExpectations)
                {
                    BlueAddBot(request);
                }
            }
            UpdateBlueBotSlots();

            alreadyFulfilled = new List<BotRequest>();
            List<BotRequest> TempRedBotExpectations = new List<BotRequest>(RedBotExpectations);

            //is state corrupt?
            if (PrevRedBotExpectations.Count != wrapper.slots.RedBotCount)
            {
                Debug.Log(string.Format("Red bots corrupted. Count is {0}, should be {1}", wrapper.slots.RedBotCount, PrevRedBotExpectations.Count));
                RemoveRedBots();
                foreach (BotRequest request in TempRedBotExpectations)
                {
                    RedAddBot(request);
                }
            }
            else
            {
                foreach (BotRequest oldRequest in PrevRedBotExpectations.ToList())
                {
                    bool fulfilled = false;
                    foreach (BotRequest newRequest in TempRedBotExpectations)
                    {
                        if (oldRequest.Hero == newRequest.Hero && oldRequest.Difficulty == newRequest.Difficulty)
                        {
                            fulfilled = true;
                            alreadyFulfilled.Add(newRequest);
                            PrevRedBotExpectations.Remove(oldRequest);
                            TempRedBotExpectations.Remove(newRequest);
                            break;
                        }
                    }
                    if (!fulfilled)
                    {
                        RemoveRedBots();
                        foreach (BotRequest request in TempRedBotExpectations)
                        {
                            RedAddBot(request);
                        }
                        foreach (BotRequest request in alreadyFulfilled)
                        {
                            RedAddBot(request);
                        }
                        break;
                    }
                }
                foreach (BotRequest request in TempRedBotExpectations)
                {
                    RedAddBot(request);
                }
            }
            UpdateRedBotSlots();
        }

        private void BlueAddBot(BotRequest request)
        {
            BlueAddBot(request.Hero, request.Difficulty);
        }

        private void RedAddBot(BotRequest request)
        {
            RedAddBot(request.Hero, request.Difficulty);
        }

        private void BlueAddBot(AIHero hero, Difficulty difficulty)
        {
            if (wrapper.slots.BlueEmptyCount > 0)
            {
                BotTeam team = BotTeam.Blue;
                AddBot(hero, difficulty, team);
            }
        }

        private void RedAddBot(AIHero hero, Difficulty difficulty)
        {
            if (wrapper.slots.RedEmptyCount > 0)
            {
                BotTeam team = BotTeam.Red;
                AddBot(hero, difficulty, team);
            }
        }

        private void UpdateBlueBotSlots()
        {
            System.Threading.Thread.Sleep(500);
            List<int> FilledSlots = cg.BlueSlots;
            List<int> BotSlots = new List<int>();
            foreach (int slot in FilledSlots)
            {
                bool isAi = cg.AI.IsAI(slot);
                if (isAi)
                {
                    BotSlots.Add(slot);
                }
            }
            BlueBotSlots = BotSlots;
        }

        private void UpdateRedBotSlots()
        {
            System.Threading.Thread.Sleep(500); //use cg wait for 
            List<int> FilledSlots = cg.RedSlots;
            List<int> BotSlots = new List<int>();
            foreach (int slot in FilledSlots)
            {
                bool isAi = cg.AI.IsAI(slot);
                if (isAi)
                {
                    BotSlots.Add(slot);
                }
            }
            RedBotSlots = BotSlots;
        }

        private void AddBot(AIHero hero, Difficulty difficulty, BotTeam team)
        {
            wrapper.joins.LockSlots();
            cg.AI.AddAI(hero, difficulty, team, 1);
            wrapper.joins.UnlockSlots();
        }

        public void RemoveBotsIfAny()
        {
            if (wrapper.slots.BotCount > 0)
            {
                RemoveBots();
            }
        }

        public void RemoveBots()
        {
            cg.AI.RemoveAllBotsAuto();
            BlueBotSlots = new List<int>();
            RedBotSlots = new List<int>();
        }

        public void RemoveBlueBots()
        {
            List<int> slots = wrapper.slots.BlueBotSlots;
            foreach (int slot in slots)
            {
                //todolater if returns false repair state
                cg.AI.RemoveFromGameIfAI(slot);
            }
        }

        public void RemoveRedBots()
        {
            List<int> slots = wrapper.slots.RedBotSlots;
            foreach(int slot in slots) {
                //todolater if returns false repair state
                cg.AI.RemoveFromGameIfAI(slot);
            }
        }

        //private int GetExpectedBotSlot(BotTeam team)
        //{
        //    List<int> EmptySlots;
        //    if (team == BotTeam.Red)
        //    {
        //        EmptySlots = wrapper.slots.RedEmptySlots;
        //    }
        //    else
        //    {
        //        EmptySlots = wrapper.slots.BlueEmptySlots;
        //    }
        //    int ExpectedBotSlot = EmptySlots[0];
        //    return ExpectedBotSlot;
        //}

    }
}