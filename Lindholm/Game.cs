using Deltin.CustomGameAutomation;
using System;
using System.Collections.Generic;
using Lindholm.Bots;
using Lindholm.Chat;
using Lindholm.Loop;
using Lindholm.Phases;
using Lindholm.Slots;

namespace Lindholm
{

    public partial class Game : IDisposable
    {
        private readonly GameLoop _loop;

        public IChatManager Chat;
        internal PhaseManager Phases;
//        internal MatchManager Match;
//        internal MapChooser Maps;

        public IBotManager Bots;
        //internal PlayerManager Players;


//        internal MatchLogger Logger;
        internal ISlotManager Slots;
//        internal JoinManager Joins;

        public int ServerDuration {
            get; internal set;
        } = 0;

        public CustomGame Cg { get; set; }

        private readonly List<Action> _startFunctions = new List<Action>();
        private bool _blueNameSet;
        private bool _redNameSet;

        public Game()
        {
            IGameBuilder builder = new GameBuilder();
            Cg = builder.CustomGameBuilder();

            Phases = builder.PhaseManagerBuilder();
            _loop = builder.LoopBuilder(Phases);

            Chat = builder.ChatBuilder(Cg.Chat);

            //ISlotContentHistory slotContentHistory = builder.SlotContentHistoryBuilder();           
            Slots = builder.SlotBuilder();

            Bots = builder.BotBuilder(Cg.AI, Slots);
            //Players = new PlayerManager(this);

            //Match = new MatchManager(this);


            //Logger = new MatchLogger(this);

            //Maps = new MapChooser(this, Map.A_HorizonLunarColony); //todolater get this from the user. Also need to load preset


            //Joins = new JoinManager(this);

            //Loop.AddPhaselessLoop(IncrementTime, 1);
        }

        public void SetGameName(string name)
        {
            Cg.Settings.SetGameName(name);
        }

        public void SetBlueName(string name)
        {
            _blueNameSet = true;
            Cg.Settings.SetTeamName(PlayerTeam.Blue, @"\ " + name);
        }

        public void SetRedName(string name)
        {
            _redNameSet = true;
            Cg.Settings.SetTeamName(PlayerTeam.Red, "* " + name);
        }

        public void SetPreset(int preset, int numPresets)
        {
            //cg.Settings.SetNumPresets(numPresets); //fixme re-add if deltin doesn't mind
            SetPreset(preset);
        }

        public void SetPreset(int preset)
        {
            Cg.Settings.LoadPreset(preset);
        }


        public void Start()
        {
            Dev.Log("Starting");
            //FixNames();
            PerformStartFunctions();
            _loop.Start();
        }

        public void AddStartFunc(Action func)
        {
            _startFunctions.Add(func);
        }

        private void PerformStartFunctions()
        {
            foreach (Action func in _startFunctions)
            {
                func();
            }
        }

        public void Start_WelcomeMessage()
        {
            Chat.Print("Beep boop. Starting up.");
        }

        public void Start_EnterGame()
        {
            if (Cg.GetGameState() == GameState.InLobby)
            {
                Cg.StartGame();
            }
        }

        /// <summary>
        /// Disposes of custom game resources.
        /// </summary>
        /// 

        public void Dispose() { }

        public void Dispose(bool all)
        {
            GC.SuppressFinalize(this); //todolater param may be wrong
            if (all)
            {
                Cg.Dispose();
            }
        }

        private void IncrementTime()
        {
            ServerDuration++;
        }

//        private void FixNames()
//        {
//            if (!blueNameSet)
//            {
//                SetBlueName("Blue Team");
//            }
//
//            if (!redNameSet)
//            {
//                SetRedName("Red Team");
//            }
//
//        }



    }
}
