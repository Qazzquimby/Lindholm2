//using Deltin.CustomGameAutomation;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Lindholm.BotSlots;
//using UnrealAmplified;
//
//namespace Lindholm
//{
//
//    //todolater set desired player counts per team (for assymetrical games)
//
//    class PlayerManager : WrapperComponent
//    {
//        public JoinSlots joins;
//        public LeaveSlots leaves;
//
//        public Dictionary<Team, List<int>> PlayerCountHistory = new Dictionary<Team, List<int>>()
//            {
//                { Team.Blue, new List<int>() },
//                { Team.Red, new List<int>() },
//            };
//
//        public Dictionary<Team, List<List<int>>> PlayerSlotHistory = new Dictionary<Team, List<List<int>>>()
//            {
//                { Team.Blue, new List<List<int>>() },
//                { Team.Red, new List<List<int>>() },
//            };
//
//        private List<Action> joinFuncs = new List<Action>();
//        private List<Action> leaveFuncs = new List<Action>();
//        private List<Action> joinOrLeaveFuncs = new List<Action>();
//
//        public Dictionary<Team, int> SizeOverTime = new Dictionary<Team, int>()
//            {
//                { Team.Blue, 0 },
//                { Team.Red, 0 },
//            };
//
//        public Scrambler scrambler;
//        public AutoBalancer balancer;
//
//        public void NewMatch()
//        {
//            SizeOverTime[Team.Blue] = 0;
//            SizeOverTime[Team.Red] = 0;
//        }
//
//        public PlayerManager(Game wrapperInject) : base(wrapperInject)
//        {
//            joins = new JoinSlots(wrapper.BotSlots, _joinSlots);
//            leaves = new LeaveSlots(wrapper.BotSlots, _leaveSlots);
//            InitPlayerCount();
//            scrambler = new Scrambler(this);
//            balancer = new AutoBalancer(this);
//            wrapper.BotSlots.Players = new PlayerSlots(this);
//        }
//
//
//
//        #region JoinSlots and LeaveSlots
//
//        public class JoinSlots : BaseSlots
//        {
//            private Dictionary<Team, List<int>> _joinSlots;
//            public JoinSlots(BotSlots.SlotManager slotManager, Dictionary<Team, List<int>> leaveSlots) : base()
//            {
//                _joinSlots = leaveSlots;
//            }
//
//            protected override List<int> BlueSlots {
//                get {
//                    return _joinSlots[Team.Blue];
//                }
//            }
//
//            protected override List<int> RedSlots {
//                get {
//                    return _joinSlots[Team.Red];
//                }
//            }
//        }
//
//        private Dictionary<Team, List<int>> _joinSlots = new Dictionary<Team, List<int>>()
//            {
//                { Team.Blue, new List<int>() },
//                { Team.Red, new List<int>() },
//            };
//
//
//
//        public class LeaveSlots : BaseSlots
//        {
//            private Dictionary<Team, List<int>> _leaveSlots;
//            public LeaveSlots(BotSlots.SlotManager slotManager, Dictionary<Team, List<int>> leaveSlots) : base()
//            {
//                _leaveSlots = leaveSlots;
//            }
//
//            protected override List<int> BlueSlots {
//                get {
//                    return _leaveSlots[Team.Blue];
//                }
//            }
//
//            protected override List<int> RedSlots {
//                get {
//                    return _leaveSlots[Team.Red];
//                }
//            }
//        }
//
//        private Dictionary<Team, List<int>> _leaveSlots = new Dictionary<Team, List<int>>()
//            {
//                { Team.Blue, new List<int>() },
//                { Team.Red, new List<int>() },
//            };
//        #endregion
//
//        public void AddJoinFunc(Action func)
//        {
//            joinFuncs.Add(func);
//        }
//
//        public void AddLeaveFunc(Action func)
//        {
//            leaveFuncs.Add(func);
//        }
//
//        public void AddJoinOrLeaveFunc(Action func)
//        {
//            joinOrLeaveFuncs.Add(func);
//        }
//
//        private void PerformJoinFuncs()
//        {
//            foreach (Action func in joinFuncs)
//            {
//                func();
//            }
//        }
//
//        private void PerformLeaveFuncs()
//        {
//            foreach (Action func in leaveFuncs)
//            {
//                func();
//            }
//        }
//
//        private void PerformJoinOrLeaveFuncs()
//        {
//            foreach (Action func in joinOrLeaveFuncs)
//            {
//                func();
//            }
//        }
//
//        public int GetTeamSizeAdvantage(Team team)
//        {
//            return wrapper.BotSlots.Players.Count(team) - wrapper.BotSlots.Players.Count(team.Other());
//        }
//
//        private void InitPlayerCount()
//        {
//            //wrapper.bots.RemoveBots(); //todosoon ensure this is safe to remove.
//            InitPlayerCount(Team.Blue);
//            InitPlayerCount(Team.Red);
//        }
//
//        private void InitPlayerCount(Team team)
//        {
//            List<int> BotSlots = wrapper.BotSlots.Filled.BotSlots(team);
//            int Count = 0;
//            for (int i = 0; i < BotSlots.Count; i++)
//            {
//                if (!cg.AI.IsAI(BotSlots[i]))
//                {
//                    Count++;
//                }
//            }
//            PlayerCountHistory[Team.Red].Add(Count);
//        }
//
//        public void UpdatePlayers()
//        {
//            UpdatePlayers(Team.Blue);
//            UpdatePlayers(Team.Red);
//        }
//
//        private void UpdatePlayers(Team team)
//        {
//            List<int> OldSlots;
//            List<int> NewSlots = GetNewPlayerSlots(team);
//
//            try
//            {
//                NewSlots = NewSlots.Intersect(wrapper.Players.PlayerSlotHistory[team][wrapper.Players.PlayerSlotHistory[team].Count - 1]).ToList();
//                OldSlots = wrapper.Players.PlayerSlotHistory[team][wrapper.Players.PlayerSlotHistory[team].Count - 2];
//            }
//            catch (ArgumentOutOfRangeException)
//            {
//                OldSlots = new List<int>();
//            }
//
//            _joinSlots[team] = NewSlots.Except(OldSlots).ToList();
//            foreach (int slot in joins.BotSlots(team))
//            {
//                Dev.Log(string.Format("{0} join {1}", team.ToString(), slot));
//            }
//
//            _leaveSlots[team] = OldSlots.Except(NewSlots).ToList();
//
//            SizeOverTime[team] += NewSlots.Count;
//
//            PlayerCountHistory[team].Add(NewSlots.Count);
//            PlayerSlotHistory[team].Add(NewSlots);
//        }
//
//
//        private List<int> GetNewPlayerSlots(Team team)
//        {
//            List<int> BotSlots = wrapper.BotSlots.Filled.BotSlots(team);
//            return GetNewPlayerSlots(BotSlots);
//        }
//
//        private List<int> GetNewPlayerSlots(List<int> slots)
//        {
//            List<int> BotSlots = wrapper.BotSlots.Bots.BotSlots();
//
//            List<int> PlayerSlots = slots.Except(BotSlots).ToList();
//            return PlayerSlots;
//        }
//
//        public void HandleJoinsAndLeaves()
//        {
//            if (!wrapper.Match.gameEnded)
//            {
//                HandleSwaps();
//                if (joins.Count() > 0)
//                {
//                    cg.AI.CalibrateAIChecking();
//                    PerformJoinFuncs();
//                }
//                if (leaves.Count() > 0)
//                {
//                    PerformLeaveFuncs();
//                }
//                if (joins.Count() > 0 || leaves.Count() > 0)
//                {
//                    PerformJoinOrLeaveFuncs();
//                }
//            }
//        }
//
//        private void HandleSwaps()
//        {
//            if (PlayerCountHistory[Team.Blue].Count > 10) //Buffer to avoid swaps when starting up
//            {
//                Team largerTeam;
//                if (wrapper.BotSlots.TeamHasMorePlayers(Team.Blue))
//                {
//                    largerTeam = Team.Blue;
//                }
//                else
//                {
//                    largerTeam = Team.Red;
//                }
//
//                foreach (int joinSlot in joins.BotSlots(largerTeam))
//                {
//                    if (wrapper.BotSlots.Players.Count(largerTeam) >= wrapper.BotSlots.Players.Count(largerTeam.Other()) + 2)
//                    {
//                        Dev.Log(string.Format("{0} much larger. Swapping new join from slot {1}", largerTeam.ToString(), joinSlot));
//                        ForcePlayerSwap(joinSlot);
//                    }
//                    else if (wrapper.BotSlots.Players.Count(largerTeam) == wrapper.BotSlots.Players.Count(largerTeam.Other()) + 1
//                        && SizeOverTime[Team.Blue] > SizeOverTime[Team.Red])
//                    {
//                        Dev.Log(string.Format("{0} larger longer. Swapping new join from slot {1}", largerTeam.ToString(), joinSlot));
//                        ForcePlayerSwap(joinSlot);
//                    }
//                }
//            }
//        }
//
//        private void ForcePlayerSwap(int slot)
//        {
//            Team slotTeam = wrapper.BotSlots.TeamWithSlot(slot);
//            Team newTeam = slotTeam.Other();
//
//            List<int> empties = wrapper.BotSlots.Empty.BotSlots(newTeam);
//
//            if (empties.Count == 0)
//            {
//                if (wrapper.BotSlots.Players.Count(newTeam) == 6)
//                {
//                    Dev.Log(string.Format("Trying to swap from {0} to full team, cancelling.", slot));
//                    return;
//                }
//                else
//                {
//                    Dev.Log(string.Format("Must remove bots before able to swap {0}", slot));
//                    wrapper.Bots.RemoveBots(newTeam);
//                    ForcePlayerSwap(slot);
//                    return;
//                }
//            }
//            try
//            {
//                cg.Interact.Move(slot, empties[0]);
//            }
//            catch (ArgumentOutOfRangeException)
//            {
//                Dev.Log(string.Format("Tried to swap {0} when no empty slots. Retrying.", slot));
//                ForcePlayerSwap(slot);
//                return;
//            }
//        }
//
//        public class Scrambler
//        {
//            private PlayerManager players;
//            private Random rnd = new Random();
//            public Scrambler(PlayerManager playersInject)
//            {
//                players = playersInject;
//            }
//
//            public void ScrambleTeams()
//            {
//                if (players.wrapper.BotSlots.Players.Count() > 0)
//                {
//                    //players.wrapper.Chat.Print("Scrambling teams.");
//
//                    Dictionary<Team, List<int>> SlotsToSwap = new Dictionary<Team, List<int>>() {
//                        {Team.Blue, GetSlotsToSwap(Team.Blue) },
//                        {Team.Red, GetSlotsToSwap(Team.Red) },
//                    };
//
//                    while (SlotsToSwap[Team.Blue].Count > 0 && SlotsToSwap[Team.Red].Count > 0)
//                    {
//                        int BlueSlotToSwap = SlotsToSwap[Team.Blue][0];
//                        int RedSlotToSwap = SlotsToSwap[Team.Red][0];
//
//                        players.wrapper.CG.Interact.Move(BlueSlotToSwap, RedSlotToSwap);
//                        SlotsToSwap[Team.Blue].RemoveAt(0);
//                        SlotsToSwap[Team.Red].RemoveAt(0);
//                    }
//
//                    foreach (Team team in players.wrapper.Teams)
//                    {
//                        while (SlotsToSwap[team].Count > 0)
//                        {
//                            int SlotToSwap = SlotsToSwap[team][0];
//                            players.ForcePlayerSwap(SlotToSwap);
//                            SlotsToSwap[team].RemoveAt(0);
//                        }
//                    }
//
//                    Dev.Log("Done scrambling");
//                }
//            }
//
//            private List<int> GetSlotsToSwap(Team team)
//            {
//                List<int> PlayerSlotsCopy = new List<int>(players.wrapper.BotSlots.Players.BotSlots(team));
//                PlayerSlotsCopy.OrderBy(x => rnd.Next()).ToList();
//                int NumToSwap = (PlayerSlotsCopy.Count + 1) / 2;
//                List<int> SlotsToSwap = new List<int>(PlayerSlotsCopy.GetRange(0, NumToSwap));
//                return SlotsToSwap;
//            }
//
//        }
//
//        public class AutoBalancer
//        {
//            public int SecondsBeforeSoftAutoBalance = 5;
//            public int SecondsBeforeHardAutoBalance = 10;
//
//            int TicksBeforeSoftAutobalance {
//                get {
//                    return SecondsBeforeSoftAutoBalance * TimeConstants.TicksPerSecond;
//                }
//            }
//            private bool autoBalance;
//            private DateTime autoBalanceStartTime;
//            private PlayerManager players;
//            private Random rnd = new Random();
//
//            public AutoBalancer(PlayerManager playersInject)
//            {
//                players = playersInject;
//                players.AddJoinOrLeaveFunc(BeginOrEndAutobalance);
//            }
//
//            public void BeginOrEndAutobalance()
//            {
//                int imbalanceAmount = Math.Abs(players.GetTeamSizeAdvantage(Team.Blue));
//
//                if (imbalanceAmount >= 3)
//                {
//                    Dev.Log(string.Format("Imbalance of {0}. Beginning autobalance.", imbalanceAmount));
//                    BeginAutoBalance();
//                }
//                else if (imbalanceAmount <= 1)
//                {
//                    if (autoBalance)
//                    {
//                        Dev.Log(string.Format("Imbalance of {0}. Ending autobalance.", imbalanceAmount));
//                    }
//                    EndAutoBalance();
//                }
//                else
//                {
//                    Dev.Log(string.Format("Imbalance of {0}. Waiting.", imbalanceAmount));
//                    List<int> blueTeamSizeAdvantageHistory = new List<int>();
//                    if (players.PlayerCountHistory[Team.Blue].Count > TicksBeforeSoftAutobalance)
//                    {
//                        if (GetConsistentImbalance())
//                        {
//                            BeginAutoBalance();
//                        }
//                    }
//                }
//            }
//
//            private bool GetConsistentImbalance()
//            {
//                bool consistentImbalance = true;
//                for (int i = 0; i < TicksBeforeSoftAutobalance; i++)
//                {
//                    int blueCount = players.PlayerCountHistory[Team.Blue][players.PlayerCountHistory[Team.Blue].Count - i - 1];
//                    int redCount = players.PlayerCountHistory[Team.Red][players.PlayerCountHistory[Team.Red].Count - i - 1];
//                    int blueSizeAdvantage = blueCount - redCount;
//                    if (Math.Abs(blueSizeAdvantage) < 2)
//                    {
//                        consistentImbalance = false;
//                        break;
//                    }
//                }
//                return consistentImbalance;
//            }
//
//            public void PerformAutoBalance()
//            {
//                if (autoBalance)
//                {
//                    if (Math.Abs(players.GetTeamSizeAdvantage(Team.Blue)) <= 1)
//                    {
//                        EndAutoBalance();
//                    }
//                    else
//                    {
//                        //Wait for death
//                        if (DateTime.Now.Subtract(autoBalanceStartTime).TotalSeconds < SecondsBeforeHardAutoBalance)
//                        {
//                            SwapDeadPlayer();
//                        }
//                        else
//                        {//Swap anyone
//                            SwapToBalance();
//                        }
//                    }
//                }
//            }
//
//            private void SwapDeadPlayer()
//            {
//                Team largerTeam = players.wrapper.BotSlots.TeamWithMoreOrEqualPlayers();
//
//                List<int> slots = players.wrapper.BotSlots.Players.BotSlots(largerTeam);
//                List<int> empties = players.wrapper.BotSlots.Empty.BotSlots(largerTeam.Other());
//
//                List<int> dead = players.wrapper.CG.PlayerInfo.PlayersDead();
//                foreach (int dead_slot in dead)
//                {
//                    if (slots.Contains(dead_slot))
//                    {
//                        if (empties.Count > 0)
//                        {
//                            int last_empty = empties[empties.Count - 1];
//                            players.wrapper.CG.Interact.Move(dead_slot, last_empty);
//                        }
//                    }
//                }
//            }
//
//            public void SwapToBalance()
//            {
//                players.UpdatePlayers();
//                int blueSizeAdvantage = players.GetTeamSizeAdvantage(Team.Blue);
//                if (Math.Abs(blueSizeAdvantage) >= 2)
//                {
//                    List<int> playerSlots;
//                    Team largerTeam = players.wrapper.BotSlots.TeamWithMoreOrEqualPlayers();
//                    Dev.Log(string.Format("Swapping player from {0} to balance", largerTeam.ToString()));
//                    playerSlots = players.wrapper.BotSlots.Players.BotSlots(largerTeam);
//                    int randomPlayer = playerSlots[rnd.Next(playerSlots.Count)];
//                    players.ForcePlayerSwap(randomPlayer);
//                }
//            }
//
//            private void BeginAutoBalance()
//            {
//                if (!autoBalance)
//                {
//                    autoBalanceStartTime = DateTime.Now;
//                    //players.wrapper.Chat.Print("Beginning autobalance");
//                    autoBalance = true;
//                }
//            }
//
//            private void EndAutoBalance()
//            {
//                if (autoBalance)
//                {
//                    //players.wrapper.Chat.Print("Team sizes balanced.");
//                    autoBalance = false;
//                }
//            }
//
//        }
//
//    }
//
//
//
//}