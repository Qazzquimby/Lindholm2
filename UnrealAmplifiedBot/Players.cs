using Deltin.CustomGameAutomation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BotLibrary
{

    class PlayerManager : WrapperComponent
    {
        public List<int> RedPlayerCountHistory = new List<int>();
        public List<int> BluePlayerCountHistory = new List<int>();

        public List<List<int>> RedPlayerSlotHistory = new List<List<int>>();
        public List<List<int>> BluePlayerSlotHistory = new List<List<int>>();

        private List<Action> joinFuncs = new List<Action>();
        private List<Action> leaveFuncs = new List<Action>();
        private List<Action> joinOrLeaveFuncs = new List<Action>();

        private int BlueSizeOverTime = 0;
        private int RedSizeOverTime = 0;

        public Scrambler scrambler;
        public AutoBalancer balancer;

        public void NewMatch()
        {
            BlueSizeOverTime = 0;
            RedSizeOverTime = 0;
        }

        public PlayerManager(CustomGameWrapper wrapperInject) : base(wrapperInject)
        {
            {
                InitPlayerCounts();
                scrambler = new Scrambler(this);
                balancer = new AutoBalancer(this);
            }
        }

        #region JoinSlots and LeaveSlots
        public List<int> BlueJoinSlots { get; internal set; }

        public int BlueJoinCount
        {
            get
            {
                return BlueJoinSlots.Count;
            }
        }

        public List<int> RedJoinSlots { get; internal set; }

        public int RedJoinCount
        {
            get
            {
                return RedJoinSlots.Count;
            }
        }


        public List<int> BlueLeaveSlots { get; internal set; }

        public int BlueLeaveCount
        {
            get
            {
                return BlueLeaveSlots.Count;
            }
        }

        public List<int> RedLeaveSlots { get; internal set; }

        public int RedLeaveCount
        {
            get
            {
                return RedLeaveSlots.Count;
            }
        }

        #endregion

        public void AddJoinFunc(Action func)
        {
            joinFuncs.Add(func);
        }

        public void AddLeaveFunc(Action func)
        {
            leaveFuncs.Add(func);
        }

        public void AddJoinOrLeaveFunc(Action func)
        {
            joinOrLeaveFuncs.Add(func);
        }

        private void PerformJoinFuncs()
        {
            foreach (Action func in joinFuncs)
            {
                func();
            }
        }

        private void PerformLeaveFuncs()
        {
            foreach (Action func in leaveFuncs)
            {
                func();
            }
        }

        private void PerformJoinOrLeaveFuncs()
        {
            foreach (Action func in joinOrLeaveFuncs)
            {
                func();
            }
        }

        public int GetBlueSizeAdvantage()
        {
            return wrapper.slots.BluePlayerCount - wrapper.slots.RedPlayerCount;
        }

        private void InitPlayerCounts()
        {
            wrapper.bots.RemoveBots();
            InitRedCount();
            InitBlueCount();
        }

        private void InitRedCount()
        {
            List<int> Slots = cg.RedSlots;
            int Count = 0;
            for (int i = 0; i < Slots.Count; i++)
            {
                if (!cg.AI.IsAI(Slots[i]))
                {
                    Count++;
                }
            }
            RedPlayerCountHistory.Add(Count);
        }

        private void InitBlueCount()
        {
            List<int> slots = cg.BlueSlots;
            int count = 0;
            for (int i = 0; i < slots.Count; i++)
            {
                if (!cg.AI.IsAI(slots[i]))
                {
                    count++;
                }
            }
            BluePlayerCountHistory.Add(count);
        }

        public void UpdatePlayers()
        {
            List<int> RedOldSlots;
            List<int> BlueOldSlots;

            List<int> RedNewSlots = GetNewRedPlayerSlots();
            List<int> BlueNewSlots = GetNewBluePlayerSlots();

            try
            {
                RedNewSlots = RedNewSlots.Intersect(wrapper.players.RedPlayerSlotHistory[wrapper.players.RedPlayerSlotHistory.Count - 1]).ToList();
                BlueNewSlots = BlueNewSlots.Intersect(wrapper.players.BluePlayerSlotHistory[wrapper.players.BluePlayerSlotHistory.Count - 1]).ToList();
                
                RedOldSlots = wrapper.players.RedPlayerSlotHistory[wrapper.players.RedPlayerSlotHistory.Count - 2];
                BlueOldSlots = wrapper.players.BluePlayerSlotHistory[wrapper.players.BluePlayerSlotHistory.Count - 2];
            }
            catch (ArgumentOutOfRangeException)
            {
                RedOldSlots = new List<int>();
                BlueOldSlots = new List<int>();
            }
            
            
            RedJoinSlots = RedNewSlots.Except(RedOldSlots).ToList();
            foreach (int slot in RedJoinSlots)
            {
                Debug.Log(string.Format("Red join {0}", slot));
            }

            BlueJoinSlots = BlueNewSlots.Except(BlueOldSlots).ToList();
            foreach (int slot in BlueJoinSlots)
            {
                Debug.Log(string.Format("Blue join {0}", slot));
            }

            RedLeaveSlots = RedOldSlots.Except(RedNewSlots).ToList();
            BlueLeaveSlots = BlueOldSlots.Except(BlueNewSlots).ToList();

            RedSizeOverTime += RedNewSlots.Count;
            BlueSizeOverTime += BlueNewSlots.Count;

            RedPlayerCountHistory.Add(RedNewSlots.Count);
            BluePlayerCountHistory.Add(BlueNewSlots.Count);
            RedPlayerSlotHistory.Add(RedNewSlots);
            BluePlayerSlotHistory.Add(BlueNewSlots);

            HandleJoinsAndLeaves();
            if (RedJoinCount + BlueJoinCount > 0)
            {
                cg.AI.CalibrateAIChecking(); 
                PerformJoinFuncs();
            }
            if (RedLeaveCount + BlueLeaveCount > 0)
            {
                PerformLeaveFuncs();
            }
            if (RedJoinCount + BlueJoinCount > 0 && RedLeaveCount + BlueLeaveCount > 0)
            {
                PerformJoinOrLeaveFuncs();
            }
        }

        //todo set desired player counts per team

        private List<int> GetNewRedPlayerSlots()
        {
            List<int> Slots = cg.RedSlots;
            return GetNewPlayerSlots(Slots);
        }

        private List<int> GetNewBluePlayerSlots()
        {
            List<int> Slots = cg.BlueSlots;
            return GetNewPlayerSlots(Slots);
        }

        private List<int> GetNewPlayerSlots(List<int> slots)
        {
            List<int> BotSlots = wrapper.slots.BotSlots;
            List<int> PlayerSlots = slots.Except(BotSlots).ToList();
            return PlayerSlots;
        }

        private void HandleJoinsAndLeaves()
        {
            if (!wrapper.match.gameEnded)
            {
                if (RedPlayerCountHistory.Count > 10) //Buffer to avoid swaps when starting up
                {
                    if (wrapper.slots.BlueHasMorePlayers)
                    {
                        foreach (int joinSlot in BlueJoinSlots)
                        {
                            if (wrapper.slots.BluePlayerCount >= wrapper.slots.RedPlayerCount + 2)
                            {
                                Debug.Log(string.Format("Blue much larger. Swapping new join from slot {0}", joinSlot));
                                ForcePlayerSwap(joinSlot);
                            }
                            else if (wrapper.slots.BluePlayerCount == wrapper.slots.RedPlayerCount + 1
                              && BlueSizeOverTime > RedSizeOverTime)
                            {
                                Debug.Log(string.Format("Blue larger longer. Swapping new join from slot {0}", joinSlot));
                                ForcePlayerSwap(joinSlot);
                            }
                        }
                    }
                    else if (wrapper.slots.RedHasMorePlayers)
                    {
                        foreach (int joinSlot in RedJoinSlots)
                        {
                            if (wrapper.slots.RedPlayerCount >= wrapper.slots.BluePlayerCount + 2)
                            {
                                Debug.Log(string.Format("Red much larger. Swapping new join from slot {0}", joinSlot));
                                ForcePlayerSwap(joinSlot);
                            }
                            else if (wrapper.slots.RedPlayerCount == wrapper.slots.BluePlayerCount + 1
                              && RedSizeOverTime > BlueSizeOverTime)
                            {
                                Debug.Log(string.Format("Red larger longer. Swapping new join from slot {0}", joinSlot));
                                ForcePlayerSwap(joinSlot);
                            }
                        }
                    }

                }

            }
        }

        private void ForcePlayerSwap(int slot)
        {
            List<int> empties;
            Team newTeam;
            if (slot < 6)
            {
                newTeam = Team.Red;
                empties = wrapper.slots.RedEmptySlots;
            }
            else
            {
                newTeam = Team.Blue;
                empties = wrapper.slots.BlueEmptySlots;
            }

            if (empties.Count == 0)
            {
                Debug.Log(string.Format("Must remove bots before able to swap {0}", slot));
                if (newTeam == Team.Blue)
                {
                    wrapper.bots.RemoveBlueBots();
                }
                else
                {
                    wrapper.bots.RemoveRedBots();
                }
                ForcePlayerSwap(slot);
            }
            try
            {
                cg.Interact.Move(slot, empties[0]);
            }
            catch (ArgumentOutOfRangeException)
            {
                Debug.Log(string.Format("Tried to swap {0} when no empty slots. Retrying.", slot));
                ForcePlayerSwap(slot);
            }
            

        }

        public class Scrambler
        {
            private PlayerManager players;
            private Random rnd = new Random();
            public Scrambler(PlayerManager playersInject)
            {
                players = playersInject;
            }

            public void ScrambleTeams()
            {
                if (players.wrapper.slots.PlayerCount > 0)
                {
                    players.wrapper.chat.MatchChat("Scrambling teams.");

                    List<int> BluePlayerSlotsCopy = new List<int>(players.wrapper.slots.BluePlayerSlots);
                    BluePlayerSlotsCopy.OrderBy(x => rnd.Next()).ToList();
                    int BlueNumToSwap = (BluePlayerSlotsCopy.Count + 1) / 2;
                    List<int> BlueSlotsToSwap = new List<int>(BluePlayerSlotsCopy.GetRange(0, BlueNumToSwap));

                    List<int> RedPlayerSlotsCopy = new List<int>(players.wrapper.slots.RedPlayerSlots);
                    RedPlayerSlotsCopy.OrderBy(x => rnd.Next()).ToList();
                    int RedNumToSwap = (RedPlayerSlotsCopy.Count + 1) / 2;
                    List<int> RedSlotsToSwap = new List<int>(RedPlayerSlotsCopy.GetRange(0, RedNumToSwap));

                    while (BlueSlotsToSwap.Count > 0 && RedSlotsToSwap.Count > 0)
                    {
                        int BlueSlotToSwap = BlueSlotsToSwap[0];
                        int RedSlotToSwap = RedSlotsToSwap[0];

                        players.wrapper.cg.Interact.Move(BlueSlotToSwap, RedSlotToSwap);
                        BlueSlotsToSwap.RemoveAt(0);
                        RedSlotsToSwap.RemoveAt(0);
                    }

                    while (BlueSlotsToSwap.Count > 0)
                    {
                        int SlotToSwap = BlueSlotsToSwap[0];
                        players.ForcePlayerSwap(SlotToSwap);
                        BlueSlotsToSwap.RemoveAt(0);
                    }

                    while (RedSlotsToSwap.Count > 0)
                    {
                        int SlotToSwap = RedSlotsToSwap[0];
                        players.ForcePlayerSwap(SlotToSwap);
                        RedSlotsToSwap.RemoveAt(0);
                    }
                    Debug.Log("Done autobalance");
                }
            }
        }

        public class AutoBalancer
        {
            public int SecondsBeforeSoftAutoBalance = 5;
            public int SecondsBeforeHardAutoBalance = 10;

            private bool autoBalance;
            private DateTime autoBalanceStartTime;
            private PlayerManager players;
            private Random rnd = new Random();

            public AutoBalancer(PlayerManager playersInject)
            {
                players = playersInject;
                players.AddJoinOrLeaveFunc(BeginOrEndAutobalance);
            }

            public void BeginOrEndAutobalance()
            {
                int TicksBeforeSoftAutobalance = SecondsBeforeSoftAutoBalance * players.wrapper.loop.TicksPerSecond;

                int imbalanceAmount = Math.Abs(players.GetBlueSizeAdvantage());

                if (imbalanceAmount >= 3)
                {
                    BeginAutoBalance();
                }
                else if (imbalanceAmount <= 1)
                {
                    EndAutoBalance();
                }
                else
                {
                    bool consistentImbalance = true;
                    List<int> blueTeamSizeAdvantageHistory = new List<int>();
                    if(players.BluePlayerCountHistory.Count > TicksBeforeSoftAutobalance)
                    {
                        for (int i = 0; i < TicksBeforeSoftAutobalance; i++)
                        {
                            int blueCount = players.BluePlayerCountHistory[players.BluePlayerCountHistory.Count - i - 1];
                            int redCount = players.RedPlayerCountHistory[players.RedPlayerCountHistory.Count - i - 1];
                            int blueSizeAdvantage = blueCount - redCount;
                            if (Math.Abs(blueSizeAdvantage) < 2)
                            {
                                consistentImbalance = false;
                                break;
                            }
                        }
                        if (consistentImbalance)
                        {
                            BeginAutoBalance();
                        }
                    }
                }
            }

            public void BeginAutoBalance()
            {
                if (!autoBalance)
                {
                    autoBalanceStartTime = DateTime.Now;
                    players.wrapper.chat.MatchChat("Beginning autobalance");
                    autoBalance = true;
                }
            }

            public void EndAutoBalance()
            {
                if (autoBalance)
                {
                    players.wrapper.chat.MatchChat("Team sizes balanced.");
                    autoBalance = false;
                }
            }

            public void PerformAutoBalance()
            {
                if (autoBalance)
                {
                    int blueSizeAdvantage = players.GetBlueSizeAdvantage();
                    if (autoBalance)
                    {
                        if (Math.Abs(blueSizeAdvantage) <= 1)
                        {
                            EndAutoBalance();
                        }
                        else
                        {
                            //Wait for death
                            if (DateTime.Now.Subtract(autoBalanceStartTime).TotalSeconds < SecondsBeforeHardAutoBalance)
                            {
                                List<int> slots;
                                List<int> empties;
                                if (blueSizeAdvantage > 0)
                                {
                                    slots = players.wrapper.slots.BluePlayerSlots;
                                    empties = players.wrapper.slots.RedEmptySlots;
                                }
                                else
                                {
                                    slots = players.wrapper.slots.RedPlayerSlots;
                                    empties = players.wrapper.slots.RedEmptySlots;
                                }

                                List<int> dead = players.wrapper.cg.PlayerInfo.PlayersDead();
                                foreach (int dead_slot in dead)
                                {
                                    if (slots.Contains(dead_slot))
                                    {
                                        if (empties.Count > 0)
                                        {
                                            int last_empty = empties[empties.Count - 1];
                                            players.wrapper.cg.Interact.Move(dead_slot, last_empty);
                                        }
                                    }
                                }
                            }
                            else
                            {//Swap anyone
                                SwapToBalance();
                            }
                        }
                    }

                }
            }

            public void SwapToBalance()
            {
                players.UpdatePlayers();
                int blueSizeAdvantage = players.GetBlueSizeAdvantage();
                if (Math.Abs(blueSizeAdvantage) >= 2)
                {
                    List<int> playerSlots;
                    if (blueSizeAdvantage > 0)
                    {
                        Debug.Log("Swapping player from blue to balance");
                        playerSlots = this.players.wrapper.slots.BluePlayerSlots;
                    }
                    else
                    {
                        Debug.Log("Swapping player from red to balance");
                        playerSlots = this.players.wrapper.slots.RedPlayerSlots;
                    }

                    int randomPlayer = playerSlots[rnd.Next(playerSlots.Count)];
                    players.ForcePlayerSwap(randomPlayer);

                }
            }
        }

    }
}