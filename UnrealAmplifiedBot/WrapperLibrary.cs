using Deltin.CustomGameAutomation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BotLibrary
{
    class CustomGameWrapper
    {
        public CustomGame cg;

        public GameLoop loop;

        public Chat chat;
        public PhaseManager phases;
        public MatchManager match;
        public MapChooser maps;

        public BotManager bots;
        public PlayerManager players;


        public MatchLogger logger;
        public SlotManager slots;
        public JoinManager joins;

        public int serverDuration
        {
            get; internal set;
        } = 0;

        private List<Action> StartFuncs = new List<Action>();
        private bool blueNameSet;
        private bool redNameSet;

        public CustomGameWrapper() : this(ScreenshotMethod.ScreenCopy) { }

        public CustomGameWrapper(ScreenshotMethod screenshotMethod)
        {
            cg = new CustomGame(default(IntPtr), screenshotMethod);

            bots = new BotManager(this);
            players = new PlayerManager(this);

            
            match = new MatchManager(this);
            chat = new Chat(this);
            loop = new GameLoop(this);

            logger = new MatchLogger(this);
            phases = new PhaseManager(this);
            maps = new MapChooser(this, Map.A_HorizonLunarColony);
            
            

            slots = new SlotManager(this);
            joins = new JoinManager(this);

            loop.AddPhaselessLoop(Tick_IncrementTime, 1);
        }

        public void Start()
        {
            FixNames();
            chat.Start();
            PerformStartFunctions();
            loop.Start();
        }

        public void NewMatch()
        {

        }

        public void AddStartFunc(Action func)
        {
            StartFuncs.Add(func);
        }

        public void SetGameName(string name)
        {
            cg.GameSettings.SetGameName(name);
        }

        public void SetBlueName(string name)
        {
            blueNameSet = true;
            cg.GameSettings.SetTeamName(PlayerTeam.Blue, @"\ " + name);
        }

        public void SetRedName(string name)
        {
            redNameSet = true;
            cg.GameSettings.SetTeamName(PlayerTeam.Red, "* " + name);
        }

        private void PerformStartFunctions()
        {
            foreach (Action func in StartFuncs)
            {
                func();
            }
        }

        public void Start_WelcomeMessage()
        {
            chat.MatchChat("Beep boop. Starting up.");
        }

        public void Start_EnterGame()
        {
            if (cg.GetGameState() == GameState.InLobby)
            {
                cg.StartGame();
            }
        }

        private void Tick_IncrementTime()
        {
            serverDuration++;
        }

        private void FixNames()
        {
            if (!blueNameSet)
            {
                SetBlueName("Blue Team");
            }

            if (!redNameSet)
            {
                SetRedName("Red Team");
            }

        }

    }

    abstract class WrapperComponent
    {
        protected CustomGameWrapper wrapper;
        protected CustomGame cg;

        public WrapperComponent(CustomGameWrapper wrapperInject)
        {
            wrapper = wrapperInject;
            cg = wrapper.cg;
        }
    }

    class JoinManager : WrapperComponent
    {
        private Join DesiredJoin = Join.Everyone;
        private Join CurrentJoin;
        private int LockLevel = 0;

        public JoinManager(CustomGameWrapper wrapper) : base(wrapper) { }

        public void Start()
        {
            SetJoin(DesiredJoin);
        }

        public void SetJoin(Join join)
        {
            DesiredJoin = join;
            if (CurrentJoin != DesiredJoin)
            {
                cg.GameSettings.SetJoinSetting(DesiredJoin);
                CurrentJoin = join;
            }
        }

        public void SlotOperation(Action operation)
        {
            LockSlots();
            operation();
            UnlockSlots();
        }

        public void LockSlots()
        {
            //LockLevel++;
            //if(CurrentJoin != Join.InviteOnly)
            //{
            //    cg.GameSettings.SetJoinSetting(Join.InviteOnly);
            //}  
        }

        public void UnlockSlots()
        {
            //LockLevel--;
            //if(LockLevel == 0)
            //{
            //    SetJoin(DesiredJoin);
            //}
            //if(LockLevel < 0)
            //{
            //    System.Diagnostics.Debug.Assert(false);
            //}
        }

    }

    class SlotManager : WrapperComponent
    {
        public SlotManager(CustomGameWrapper wrapper) : base(wrapper) { }

        public List<int> BlueSlots
        {
            get
            {
                return new List<int>() { 0, 1, 2, 3, 4, 5 };
            }
        }

        public int BlueCount
        {
            get
            {
                return BlueSlots.Count;
            }
        }


        public List<int> RedSlots
        {
            get
            {
                return new List<int>() { 6, 7, 8, 9, 10, 11 };
            }

        }

        public int RedCount
        {
            get
            {
                return RedSlots.Count;
            }
        }


        public List<int> Slots
        {
            get
            {
                return new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
            }
        }

        public int Count
        {
            get
            {
                return Slots.Count;
            }
        }



        public List<int> BlueEmptySlots
        {
            get
            {
                return BlueSlots.Except(cg.BlueSlots).ToList();
            }

        }

        public int BlueEmptyCount
        {
            get
            {
                return BlueEmptySlots.Count;
            }
        }


        public List<int> RedEmptySlots
        {
            get
            {
                return RedSlots.Except(cg.RedSlots).ToList();
            }

        }

        public int RedEmptyCount
        {
            get
            {
                return RedEmptySlots.Count;
            }
        }


        public List<int> EmptySlots
        {
            get
            {
                return BlueEmptySlots.Concat(RedEmptySlots).ToList();
            }
        }

        public int EmptyCount
        {
            get
            {
                return EmptySlots.Count;
            }
        }



        public List<int> BluePlayerSlots
        {
            get
            {
                try
                {
                    return wrapper.players.BluePlayerSlotHistory[wrapper.players.BluePlayerSlotHistory.Count - 1];
                }
                catch (ArgumentOutOfRangeException)
                {
                    return new List<int>();
                }
                
            }
        }

        public int BluePlayerCount
        {
            get
            {
                return BluePlayerSlots.Count;
            }
        }


        public List<int> RedPlayerSlots
        {
            get
            {
                try
                {
                    return wrapper.players.RedPlayerSlotHistory[wrapper.players.RedPlayerSlotHistory.Count - 1];
                } catch (ArgumentOutOfRangeException)
                {
                    return new List<int>();
                }
                
            }
        }

        public int RedPlayerCount
        {
            get
            {
                return RedPlayerSlots.Count;
            }
        }


        public List<int> PlayerSlots
        {
            get
            {
                return BluePlayerSlots.Concat(RedPlayerSlots).ToList();
            }
        }

        public int PlayerCount
        {
            get
            {
                return PlayerSlots.Count;
            }
        }


        public bool BlueHasMorePlayers
        {
            get
            {
                return BluePlayerCount > RedPlayerCount;
            }
        }

        public bool RedHasMorePlayers
        {
            get
            {
                return RedPlayerCount > BluePlayerCount;
            }
        }

        public bool TeamsHaveEqualPlayers
        {
            get
            {
                return BluePlayerCount == RedPlayerCount;
            }
        }


        public List<int> BlueBotSlots
        {
            get
            {
                return wrapper.bots.IBlueBotSlots;
            }
        }

        public int BlueBotCount
        {
            get
            {
                return BlueBotSlots.Count;
            }
        }


        public List<int> RedBotSlots
        {
            get
            {
                return wrapper.bots.IRedBotSlots;
            }
        }

        public int RedBotCount
        {
            get
            {
                return RedBotSlots.Count;
            }
        }


        public List<int> BotSlots
        {
            get
            {
                return BlueBotSlots.Concat(RedBotSlots).ToList();
            }

        }

        public int BotCount
        {
            get
            {
                return BotSlots.Count;
            }
        }

    }

    class MatchManager : WrapperComponent
    {
        public double matchDuration = 0;
        public bool gameEnded = false;

        private DateTime lastGameOver = DateTime.Parse("2017-02-16T00:00:00-0:00");
        private List<System.EventHandler<Deltin.CustomGameAutomation.GameOverArgs>> gameOverArgumentFuncs
            = new List<System.EventHandler<Deltin.CustomGameAutomation.GameOverArgs>>();
        private List<Action> gameOverFuncs = new List<Action>();

        public MatchManager(CustomGameWrapper wrapperInject) : base(wrapperInject)
        {
            cg.OnGameOver += HandleGameOver;
        }

        private void HandleGameOver(object sender, GameOverArgs args)
        {
            double passedSeconds = DateTime.Now.Subtract(lastGameOver).TotalSeconds;
            if (passedSeconds > 60)
            {
                lastGameOver = DateTime.Now;
                PerformGameOverArgumentFuncs(sender, args);
                PerformGameOverFuncs();
                gameEnded = true;
            }
            else
            {
                //todolater maybe log possible false alarm
            }
        }

        public void AddGameOverArgumentFunc(System.EventHandler<Deltin.CustomGameAutomation.GameOverArgs> func)
        {
            gameOverArgumentFuncs.Add(func);
        }

        public void AddGameOverFunc(Action func)
        {
            gameOverFuncs.Add(func);
        }

        private void PerformGameOverArgumentFuncs(object sender, GameOverArgs args)
        {
            foreach (System.EventHandler<Deltin.CustomGameAutomation.GameOverArgs> func in gameOverArgumentFuncs)
            {
                func(sender, args);
            }
        }

        public void PerformGameOverFuncs()
        {
            wrapper.match.gameEnded = false;
            foreach (Action func in gameOverFuncs)
            {
                func();
            }
        }

        private void PreventMapTimeout()
        {
            int timeServerMustBeEmptyToTimeout = 100;
            int minutesBeforeEarliestTimeout = 5;


            bool consistentlyEmpty = true;
            if (wrapper.players.RedPlayerCountHistory.Count > timeServerMustBeEmptyToTimeout)
            {
                for (int i = 0; i < timeServerMustBeEmptyToTimeout; i++)
                {
                    try
                    {
                        if (wrapper.players.RedPlayerCountHistory[wrapper.players.RedPlayerCountHistory.Count - i - 1] + wrapper.players.BluePlayerCountHistory[wrapper.players.BluePlayerCountHistory.Count - i - 1] > 0)
                        {
                            consistentlyEmpty = false;
                            break;
                        }
                    }
                    catch
                    {
                        break;
                    }
                }
            }

            if (consistentlyEmpty && matchDuration > (minutesBeforeEarliestTimeout * 60) * wrapper.loop.TicksPerSecond)
            {
                cg.Chat.Chat(">>Sever is timing out soon. Moving on.");
                wrapper.maps.RandomAndNextMap();
            }
        }
    }


    class Chat : WrapperComponent
    {
        public string prefix = "";

        private Channel channel;

        public Chat(CustomGameWrapper wrapperInject) : base(wrapperInject) { }

        public void Start()
        {
            EnsureMatchChat();
        }

        public void EnsureMatchChat() //todo call at beginning of match
        {
            channel = Channel.Match;
            cg.Chat.SwapChannel(Channel.Match);
        }

        public void MatchChat(string text)
        {
            if (channel != Channel.Match)
            {
                channel = Channel.Match;
                cg.Chat.SwapChannel(Channel.Match);
            }

            if (text != null)
            {
                cg.Chat.Chat(prefix + text);
            }
        }


    }

    class GameLoop : WrapperComponent
    {
        public int TicksPerSecond = 10;
        public int StandardDelay = 5;

        public GameLoop(CustomGameWrapper wrapperInject) : base(wrapperInject) { }

        private Dictionary<int, List<Action>> PhaselessLoopFuncs { get; set; } = new Dictionary<int, List<Action>>() { };

        public void Start()
        {
            while (true)
            {
                wrapper.phases.GetCurrPhase().PerformLoop();
                wrapper.serverDuration++;
                wrapper.match.matchDuration++;
                System.Threading.Thread.Sleep(1000 / TicksPerSecond);
            }
        }

        public void AddPhaselessLoop(Action func, int delay)
        {
            try
            {
                PhaselessLoopFuncs[delay].Add(func);
            }
            catch (KeyNotFoundException)
            {
                PhaselessLoopFuncs[delay] = new List<Action>();
                PhaselessLoopFuncs[delay].Add(func);
            }
        }



    }

    class PhaseManager : WrapperComponent
    {
        public Phase gamePhase;
        public Phase setUpPhase;
        public Phase gameEndingPhase;

        private Phase currPhase;

        public PhaseManager(CustomGameWrapper wrapperInject) : base(wrapperInject)
        {

            gamePhase = GamePhaseConstructor();
            currPhase = gamePhase;

            setUpPhase = SetUpPhaseConstructor();
            gameEndingPhase = GameEndingPhaseConstructor();

        }

        public void EnterPhase(Phase newPhase)
        {
            if (currPhase != null)
            {
                currPhase.PerformExit();
            }
            currPhase = newPhase;
            currPhase.PerformEntry();
        }

        public Phase GetCurrPhase()
        {
            return currPhase;
        }

        public Phase SetUpPhaseConstructor()
        {
            Phase setUpPhase = new Phase();
            setUpPhase.AddEntry(wrapper.logger.EndMatchLog);
            setUpPhase.AddEntry(wrapper.players.scrambler.ScrambleTeams);

            setUpPhase.AddLoop(wrapper.players.balancer.SwapToBalance, 25);

            setUpPhase.AddDelay(EndSetUpPhase, 250);

            return new Phase();
        }

        public Phase GamePhaseConstructor()
        {
            Phase gamePhase = new Phase();
            gamePhase.AddLoop(wrapper.players.UpdatePlayers, wrapper.loop.StandardDelay);
            gamePhase.AddLoop(wrapper.logger.UpdatematchLog, wrapper.loop.StandardDelay);
            gamePhase.AddLoop(wrapper.players.balancer.PerformAutoBalance, wrapper.loop.StandardDelay);
            gamePhase.AddLoop(wrapper.players.balancer.BeginOrEndAutobalance, 100);
            gamePhase.AddLoop(wrapper.bots.HandleBots, 100);

            //todo look at what elements need to be added.
            //This begingamephase is executed when bot is started. End setup phase is not
            return gamePhase;
        }

        private void HandleGameOver()
        {
            if (wrapper.match.gameEnded)
            {
                EnterPhase(gameEndingPhase);
            }
        }

        private void HandleMissedGameOver()
        {
            if (cg.GetGameState() == GameState.Ending_Commend)
            {

                HandleGameOver();

            }
        }

        private void EndSetUpPhase()
        {
            EnterPhase(gamePhase);
        }

        public Phase GameEndingPhaseConstructor()
        {
            Phase gameEndingPhase = new Phase();
            gameEndingPhase.AddEntry(wrapper.match.PerformGameOverFuncs);
            gameEndingPhase.AddEntry(SkipPostGame);
            return gameEndingPhase;
        }

        public void SkipPostGame()
        {
            int wait_timer = 0;
            while (cg.GetGameState() != GameState.Ending_Commend && wait_timer < 20)
            {
                System.Threading.Thread.Sleep(1000);
                wait_timer++;
                System.Diagnostics.Debug.WriteLine(string.Format("{0} seconds wait", wait_timer));
            }
            wrapper.maps.NextMap();
        }
    }

    class Phase
    {
        private int timer = 0;

        private List<Action> EntryFuncs = new List<Action>();
        private Dictionary<int, List<Action>> DelayFuncs { get; set; } = new Dictionary<int, List<Action>>() { };
        private Dictionary<int, List<Action>> LoopFuncs { get; set; } = new Dictionary<int, List<Action>>() { };
        private List<Action> ExitFuncs = new List<Action>();

        public Phase()
        {
        }

        public void AddEntry(Action func)
        {
            EntryFuncs.Add(func);
        }

        public void AddLoop(Action func, int delay)
        {
            try
            {
                LoopFuncs[delay].Add(func);
            }
            catch (KeyNotFoundException)
            {
                LoopFuncs[delay] = new List<Action>();
                LoopFuncs[delay].Add(func);
            }
        }

        public void AddExit(Action func)
        {
            ExitFuncs.Add(func);
        }

        public void AddDelay(Action func, int delay)
        {
            try
            {
                DelayFuncs[delay].Add(func);
            }
            catch (KeyNotFoundException)
            {
                DelayFuncs[delay] = new List<Action>();
                DelayFuncs[delay].Add(func);
            }
        }

        public void PerformEntry()
        {
            PerformAllFuncs(EntryFuncs);
        }

        public void PerformDelay()
        {
            foreach (int delay in DelayFuncs.Keys)
            {
                if (timer == delay)
                {
                    List<Action> funcs = DelayFuncs[delay];
                    PerformAllFuncs(funcs);
                    DelayFuncs.Remove(delay);
                }
            }
        }

        public void PerformLoop()
        {
            foreach (int delay in LoopFuncs.Keys)
            {
                if (timer % delay == 0)
                {
                    List<Action> funcs = LoopFuncs[delay];
                    PerformAllFuncs(funcs);
                }
            }
            PerformDelay();
            timer++;
        }

        public void PerformExit()
        {
            PerformAllFuncs(ExitFuncs);
        }

        public void PerformAllFuncs(List<Action> funcs)
        {
            foreach (Action func in funcs)
            {
                func();
            }
        }

    }

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
            List<int> RedNewSlots = GetNewRedPlayerSlots();
            List<int> BlueNewSlots = GetNewBluePlayerSlots();

            List<int> RedOldSlots = wrapper.slots.RedPlayerSlots;
            List<int> BlueOldSlots = wrapper.slots.BluePlayerSlots;

            RedJoinSlots = RedNewSlots.Except(RedOldSlots).ToList();
            BlueJoinSlots = BlueNewSlots.Except(BlueOldSlots).ToList();

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
            if(RedPlayerCountHistory.Count > 5) //Buffer to avoid swaps when starting up
            {
                if (wrapper.slots.BlueHasMorePlayers)
                {
                    foreach (int joinSlot in BlueJoinSlots)
                    {
                        if (wrapper.slots.BluePlayerCount >= wrapper.slots.RedPlayerCount + 2)
                        {
                            ForcePlayerSwap(joinSlot);
                        }
                        else if (wrapper.slots.BluePlayerCount == wrapper.slots.RedPlayerCount + 1
                          && BlueSizeOverTime > RedSizeOverTime)
                        {
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
                            ForcePlayerSwap(joinSlot);
                        }
                        else if (wrapper.slots.RedPlayerCount == wrapper.slots.BluePlayerCount + 1
                          && RedSizeOverTime > BlueSizeOverTime)
                        {
                            ForcePlayerSwap(joinSlot);
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
                System.Diagnostics.Debug.WriteLine("Tried to move when no empty slot. Retrying.");
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
                //todonow readd with slots taken into account
                //if (players.wrapper.slots.PlayerCount > 0)
                //{
                //    players.wrapper.chat.MatchChat("Scrambling teams.");
                //    SettleBlueTeam();
                //    SettleRedTeam();

                //    ShuffleBlueTeam();

                //    int total_rows = Math.Max(players.wrapper.slots.RedPlayerCount, players.wrapper.slots.BluePlayerCount);
                //    int num_swaps = (total_rows + 1) / 2;

                //    List<int> swappable_rows = new List<int>();
                //    for (int i = 0; i < total_rows; i++)
                //    {
                //        swappable_rows.Add(i);
                //    }

                //    List<int> rows_to_swap = new List<int>();
                //    for (int i = 0; i < num_swaps; i++)
                //    {
                //        int swappable_row_index = rnd.Next(swappable_rows.Count);
                //        int row = swappable_rows[swappable_row_index];
                //        rows_to_swap.Add(row);
                //        swappable_rows.RemoveAt(swappable_row_index);
                //    }

                //    for (int i = 0; i < rows_to_swap.Count; i++)
                //    {
                //        int row = rows_to_swap[i];
                //        players.cg.Interact.Move(row, row + 6);
                //    }
                //}
            }

            private void SettleBlueTeam()
            {
                List<int> filled_slots = players.cg.BlueSlots;
                int swaps = 0;
                for (int i = 0; i < filled_slots.Count; i++)
                {
                    if (!filled_slots.Contains(i))
                    {
                        players.cg.Interact.Move(filled_slots[filled_slots.Count - swaps - 1], i);
                        swaps++;
                    }
                }
            }

            private void SettleRedTeam()
            {
                System.Diagnostics.Debug.WriteLine("Settling red team");
                List<int> filled_slots = players.cg.RedSlots;
                System.Diagnostics.Debug.WriteLine(String.Format("red filled slots {0}.", filled_slots));
                for (int i = 0; i < filled_slots.Count; i++)
                {
                    if (filled_slots[i] != i + 6)
                    {
                        players.cg.Interact.Move(filled_slots[i], i + 6);
                    }
                }
            }

            private void ShuffleBlueTeam()
            {
                List<int> slots = players.cg.BlueSlots;
                if (slots.Count > 1)
                {
                    System.Diagnostics.Debug.WriteLine(String.Format("Shuffling team starting at {0}", slots[0]));
                    for (int i = 0; i < slots.Count - 1; i++)
                    {
                        int random_index = rnd.Next(i + 1, slots.Count);
                        players.cg.Interact.Move(i, random_index);
                    }
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
                    //players.wrapper.bots.RemoveBotsIfAny(); //should be made unecessary by tracking player slots
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
                int blueSizeAdvantage = players.GetBlueSizeAdvantage();
                if (Math.Abs(blueSizeAdvantage) >= 2)
                {
                    List<int> playerSlots;
                    if (blueSizeAdvantage > 0)
                    {
                        playerSlots = this.players.wrapper.slots.BluePlayerSlots;
                    }
                    else
                    {
                        playerSlots = this.players.wrapper.slots.RedPlayerSlots;
                    }

                    int randomPlayer = playerSlots[rnd.Next(playerSlots.Count)];
                    players.ForcePlayerSwap(randomPlayer);

                }
            }
        }

    }

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
            if (wrapper.maps.mode_i == 3)
            {
                RemoveBotsIfAny();
            }
            else if (wrapper.slots.PlayerCount >= maximumPlayersBeforeBotRemoval)
            {
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
            List<BotRequest> alreadyFulfilled = new List<BotRequest>();
            List<BotRequest> TempBlueBotExpectations = new List<BotRequest>(BlueBotExpectations);

            //is state corrupt?
            if(PrevBlueBotExpectations.Count != wrapper.slots.BlueBotCount)
            {
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

        //private void BlueAddBot(AIHero hero, Difficulty difficulty, int numToAdd)
        //{
        //    wrapper.joins.LockSlots();
        //    for (int i = 0; i < numToAdd; i++)
        //    {
        //        BlueAddBot(hero, difficulty);
        //    }
        //    wrapper.joins.UnlockSlots();
        //}

        //private void RedAddBot(AIHero hero, Difficulty difficulty, int numToAdd)
        //{
        //    wrapper.joins.LockSlots();
        //    for (int i = 0; i < numToAdd; i++)
        //    {
        //        RedAddBot(hero, difficulty);
        //    }
        //    wrapper.joins.UnlockSlots();
        //}

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
            System.Threading.Thread.Sleep(500);
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

    class MapChooser : WrapperComponent
    {
        public Map curr { get; internal set; }
        public int mode_i { get; internal set; }

        private Random rnd = new Random();
        private List<Map> recent_maps = new List<Map>();

        private Map[][] modes;

        //todolater use probabilities

        public MapChooser(CustomGameWrapper wrapperInject, Map initialMap) : base(wrapperInject)
        {
            cg.ModesEnabled = new ModesEnabled();
            cg.ModesEnabled.Assault = true;
            cg.ModesEnabled.AssaultEscort = true;
            cg.ModesEnabled.Control = true;
            cg.ModesEnabled.Escort = true;
            cg.ModesEnabled.TeamDeathmatch = true;

            cg.CurrentOverwatchEvent = cg.GetCurrentOverwatchEvent();

            curr = initialMap;
            mode_i = 0; //wrong but not needed??

            //todolater fixme
            //wrapper.match.AddGameOverFunc(SetRandomMap);

        }

        public void SetProbability(Map map)
        {

        }

        private Dictionary<Map, float> MapProbabilities = new Dictionary<Map, float>();

        private void InitMapProbabilities()
        {
            List<Map> allMaps = GetAllMaps();
            foreach (Map map in allMaps)
            {

            }
            //todolater request different team sizes
            //Map[] mapsInGamemode = typeof(Map).GetFields(BindingFlags.Public | BindingFlags.Static)
            //    .Select(v => (Map)v.GetValue(null))
            //    .Where(v => v.GameMode = gamemode && (v.Event == Event.None || v.Event == event))
            //    .ToArray();

            //for(Map map in Map.A_VolskayaIndustries.)
        }

        private List<Map> GetAllMaps()
        {
            return typeof(Map).GetFields().Select(v => (Map)v.GetValue(null)).ToList(); ;
        }

        private Map[] AE_maps = {
            Map.AE_Eichenwalde,
            Map.AE_Hollywood,
            Map.AE_KingsRow,
            Map.AE_Numbani
        };

        private Map[] A_maps = {
            Map.A_Hanamura,
            Map.A_HorizonLunarColony,
            Map.A_TempleOfAnubis,
            Map.A_VolskayaIndustries
        };

        private Map[] C_maps = {
            Map.C_Ilios,
            Map.C_Lijiang,
            Map.C_Nepal,
            Map.C_Oasis
        };

        private Map[] TDM_maps = {
            Map.TDM_Antarctica,
            Map.TDM_BlackForest,
            Map.TDM_Castillo,
            Map.TDM_ChateauGuillard,
            //Map.TDM_Dorado,
            //Map.TDM_Eichenwalde,
            //Map.TDM_Hanamura,
            //Map.TDM_Hollywood,
            //Map.TDM_HorizonLunarColony,
            //Map.TDM_Ilios_Lighthouse,
            //Map.TDM_Ilios_Ruins,
            //Map.TDM_Ilios_Well,
            //Map.TDM_KingsRow,
            //Map.TDM_Lijiang_ControlCenter,
            //Map.TDM_Lijiang_Garden,
            //Map.TDM_Lijiang_NightMarket,
            Map.TDM_Necropolis,
            //Map.TDM_Nepal_Sanctum,
            //Map.TDM_Nepal_Shrine,
            //Map.TDM_Nepal_Village,
            //Map.TDM_Oasis_CityCenter,
            //Map.TDM_Oasis_Gardens,
            //Map.TDM_Oasis_University,
            Map.TDM_Petra,
            //Map.TDM_TempleOfAnubis,
            //Map.TDM_VolskayaIndustries
        };

        private Map[] E_maps = {
            Map.E_Dorado,
            Map.E_Junkertown,
            Map.E_Route66,
            Map.E_Gibraltar
        };

        public void SetRandomMap()
        {
            System.Diagnostics.Debug.WriteLine("Setting map");
            Map map = GetRandomMap();
            foreach (Map recent_map in recent_maps)
            {
                System.Diagnostics.Debug.WriteLine(String.Format("Recent maps contains {0}.", recent_map.MapName));
            }

            while (recent_maps.Contains(map))
            {
                System.Diagnostics.Debug.WriteLine(String.Format("Recently went to {0}. Rerolling.", CustomGame.CG_Maps.MapNameFromID(map)));
                map = GetRandomMap();
            }

            System.Diagnostics.Debug.WriteLine(String.Format("New Map is {0}.", CustomGame.CG_Maps.MapNameFromID(map)));

            SetMap(map);
        }

        public void SetMap(Map map)
        {
            cg.Maps.ToggleMap(ToggleAction.DisableAll, map);
            if (recent_maps.Count >= 5)
            {
                recent_maps.RemoveAt(0);
            }
            recent_maps.Add(map);
            curr = map;
        }

        public void RandomAndNextMap()
        {
            wrapper.maps.SetRandomMap();
            NextMap();
        }

        public void NextMap()
        {
            //todo set MatchManger match duration to 0 on new match
            cg.RestartGame();
            wrapper.phases.EnterPhase(wrapper.phases.SetUpPhaseConstructor());
        }

        private Map GetRandomMap()
        {
            Map[] mode = RandomMode();

            Map map;
            map = mode[rnd.Next(mode.Length)];
            return map;
        }

        private Map[] RandomMode()
        {
            int index;
            index = rnd.Next(modes.Length);
            //todolater allow limiting based on player count
            Map[] mode = modes[index];
            mode_i = index;
            return mode;
        }
    }

    class MatchLogger : WrapperComponent
    {
        private MatchLog currMatchLog;

        public MatchLogger(CustomGameWrapper wrapperInject) : base(wrapperInject) { }

        public void BeginMatchLog()
        {
            System.Diagnostics.Debug.WriteLine("Beginning log");
            currMatchLog = new MatchLog();
            currMatchLog.map = wrapper.maps.curr;
            currMatchLog.modeIndex = wrapper.maps.mode_i;
        }

        private void LogAtGameOver(object sender, GameOverArgs args)
        {
            if (currMatchLog != null)
            {
                currMatchLog.winning_team = args.GetWinningTeam();
            }
        }

        public void UpdatematchLog()
        {
            if (currMatchLog != null)
            {
                if (wrapper.slots.PlayerCount > 0)
                {
                    currMatchLog.duration++;

                    currMatchLog.player_count[Team.Blue] += wrapper.slots.BluePlayerCount;
                    currMatchLog.player_count[Team.Red] += wrapper.slots.RedPlayerCount;

                    int BlueJoins = wrapper.players.BlueJoinCount;
                    int BlueLeaves = wrapper.players.BlueLeaveCount;

                    int RedJoins = wrapper.players.BlueJoinCount;
                    int RedLeaves = wrapper.players.BlueLeaveCount;

                    currMatchLog.joins += BlueJoins + RedJoins;
                    currMatchLog.leaves += BlueLeaves + RedLeaves;


                    foreach (int slot in wrapper.slots.BluePlayerSlots)
                    {
                        Hero? hero_or_null = cg.PlayerInfo.GetHero(slot);
                        if (hero_or_null != null)
                        {
                            Hero hero = (Hero)hero_or_null;
                            currMatchLog.hero_play_time[Team.Blue][hero]++;
                        }
                    }
                    foreach (int slot in wrapper.slots.RedPlayerSlots)
                    {
                        Hero? hero_or_null = cg.PlayerInfo.GetHero(slot);
                        if (hero_or_null != null)
                        {
                            Hero hero = (Hero)hero_or_null;
                            currMatchLog.hero_play_time[Team.Red][hero]++;
                        }
                    }
                }
            }
        }

        public void EndMatchLog()
        {
            System.Diagnostics.Debug.WriteLine("Saving log");
            if (currMatchLog != null)
            {
                currMatchLog.endTime = DateTime.Now;

                //Todo choose your own path to log
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"D:\life itself\Unreal300%\Logs\GameLogs.txt", true))
                {
                    file.WriteLine("=============================\n");
                    file.WriteLine(String.Format("Start: {0}\n", currMatchLog.startTime));
                    file.WriteLine(String.Format("End: {0}\n", currMatchLog.endTime));
                    file.WriteLine(String.Format("Winning team: {0}\n", currMatchLog.winning_team));
                    file.WriteLine(String.Format("Map: {0}\n", CustomGame.CG_Maps.MapNameFromID(currMatchLog.map)));
                    file.WriteLine(String.Format("Mode index: {0}\n", currMatchLog.modeIndex));

                    file.WriteLine(String.Format("Joins: {0}\n", currMatchLog.joins));
                    file.WriteLine(String.Format("Leaves: {0}\n", currMatchLog.leaves));

                    file.WriteLine(String.Format("Blue player count: {0}\n", (double)currMatchLog.player_count[Team.Blue] / (double)currMatchLog.duration));
                    file.WriteLine(String.Format("Red player count: {0}\n", (double)currMatchLog.player_count[Team.Red] / (double)currMatchLog.duration));

                    Team[] teams = { Team.Blue, Team.Red };
                    foreach (Team team in teams)
                    {
                        string[] names = Enum.GetNames(typeof(Hero));
                        Hero[] heroes = Enum.GetValues(typeof(Hero)).Cast<Hero>().ToArray();

                        for (int i = 0; i < heroes.Length; i++)
                        {
                            file.WriteLine(String.Format(
                                "{0} {1} Average count: {2}\n",
                                team,
                                names[i],
                                (double)currMatchLog.hero_play_time[team][heroes[i]] / (double)currMatchLog.duration));
                        }

                    }

                }
            }

        }
    }

    class MatchLog
    {
        public DateTime startTime;
        public DateTime endTime;
        public int duration = 0;

        public int joins = 0;
        public int leaves = 0;

        public Map map;
        public int modeIndex;

        public Dictionary<Team, Dictionary<Hero, int>> hero_play_time;
        public Dictionary<Team, int> player_count;
        public PlayerTeam winning_team;

        public MatchLog()
        {
            startTime = DateTime.Now;

            hero_play_time = new Dictionary<Team, Dictionary<Hero, int>>()
            {
                {
                    Team.Blue, new Dictionary<Hero, int>()
                    {
                        { Hero.Doomfist, 0 },
                        { Hero.Genji, 0 },
                        { Hero.McCree, 0 },
                        { Hero.Pharah, 0 },
                        { Hero.Reaper, 0 },
                        { Hero.Soldier76, 0 },
                        { Hero.Sombra, 0 },
                        { Hero.Tracer, 0 },
                        { Hero.Bastion, 0 },
                        { Hero.Hanzo, 0 },
                        { Hero.Junkrat, 0 },
                        { Hero.Mei, 0 },
                        { Hero.Torbjorn, 0 },
                        { Hero.Widowmaker, 0 },
                        { Hero.DVA, 0 },
                        { Hero.Orisa, 0 },
                        { Hero.Reinhardt, 0 },
                        { Hero.Roadhog, 0 },
                        { Hero.Winston, 0 },
                        { Hero.Zarya, 0 },
                        { Hero.Ana, 0 },
                        { Hero.Brigitte, 0 },
                        { Hero.Lucio, 0 },
                        { Hero.Mercy, 0 },
                        { Hero.Moira, 0 },
                        { Hero.Symmetra, 0 },
                        { Hero.Zenyatta, 0 }
                    }
                },
                {
                    Team.Red, new Dictionary<Hero, int>()
                    {
                        { Hero.Doomfist, 0 },
                        { Hero.Genji, 0 },
                        { Hero.McCree, 0 },
                        { Hero.Pharah, 0 },
                        { Hero.Reaper, 0 },
                        { Hero.Soldier76, 0 },
                        { Hero.Sombra, 0 },
                        { Hero.Tracer, 0 },
                        { Hero.Bastion, 0 },
                        { Hero.Hanzo, 0 },
                        { Hero.Junkrat, 0 },
                        { Hero.Mei, 0 },
                        { Hero.Torbjorn, 0 },
                        { Hero.Widowmaker, 0 },
                        { Hero.DVA, 0 },
                        { Hero.Orisa, 0 },
                        { Hero.Reinhardt, 0 },
                        { Hero.Roadhog, 0 },
                        { Hero.Winston, 0 },
                        { Hero.Zarya, 0 },
                        { Hero.Ana, 0 },
                        { Hero.Brigitte, 0 },
                        { Hero.Lucio, 0 },
                        { Hero.Mercy, 0 },
                        { Hero.Moira, 0 },
                        { Hero.Symmetra, 0 },
                        { Hero.Zenyatta, 0 }
                    }
                }
            };
            player_count = new Dictionary<Team, int>() {
                {Team.Blue, 0 },
                {Team.Red, 0 }
            };


        }

    }
}
