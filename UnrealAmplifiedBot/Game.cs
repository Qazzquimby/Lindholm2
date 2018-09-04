using Deltin.CustomGameAutomation;
using System;
using System.Collections.Generic;
using Lindholm.Bots;
using Lindholm.Phases;
using Lindholm.Slots;

namespace Lindholm
{

    public partial class Game : IDisposable
    {
        public LindholmRuntime Runtime;

        private Deltin.CustomGameAutomation.CustomGame _cG;

        private GameLoop Loop;

        public Chat.IChatManager Chat;
        internal PhaseManager Phases;
//        internal MatchManager Match;
//        internal MapChooser Maps;

        public BotManager Bots;
        //internal PlayerManager Players;


//        internal MatchLogger Logger;
        internal SlotsManager Slots;
        internal JoinManager Joins;

        public int ServerDuration {
            get; internal set;
        } = 0;

        public CustomGame CG { get => _cG; set => _cG = value; }

        private List<Action> StartFuncs = new List<Action>();
        private bool blueNameSet;
        private bool redNameSet;

        public Game()
        {
            IGameBuilder builder = new GameBuilder();
            CG = builder.CustomGameBuilder();

            Phases = builder.PhaseManagerBuilder();
            Loop = builder.LoopBuilder(Phases);

            Chat = builder.ChatBuilder(CG.Chat);

            Slots = builder.SlotBuilder();

            Bots = builder.BotBuilder(Slots);
            //Players = new PlayerManager(this);

            //Match = new MatchManager(this);


            //Logger = new MatchLogger(this);

            //Maps = new MapChooser(this, Map.A_HorizonLunarColony); //todolater get this from the user. Also need to load preset


            //Joins = new JoinManager(this);

            //Loop.AddPhaselessLoop(Tick_IncrementTime, 1);
        }

        public void SetGameName(string name)
        {
            CG.Settings.SetGameName(name);
        }

        public void SetBlueName(string name)
        {
            blueNameSet = true;
            CG.Settings.SetTeamName(PlayerTeam.Blue, @"\ " + name);
        }

        public void SetRedName(string name)
        {
            redNameSet = true;
            CG.Settings.SetTeamName(PlayerTeam.Red, "* " + name);
        }

        public void SetPreset(int preset, int numPresets)
        {
            //cg.Settings.SetNumPresets(numPresets); //fixme readd if deltin doesn't mind
            SetPreset(preset);
        }

        public void SetPreset(int preset)
        {
            CG.Settings.LoadPreset(preset);
        }


        public void Start()
        {
            Dev.Log("Starting");
            FixNames();
            PerformStartFunctions();
            Loop.Start();
            Joins.Start();
        }

        public void AddStartFunc(Action func)
        {
            StartFuncs.Add(func);
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
            Chat.Print("Beep boop. Starting up.");
        }

        public void Start_EnterGame()
        {
            if (CG.GetGameState() == GameState.InLobby)
            {
                CG.StartGame();
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
                CG.Dispose();
            }
        }

        private void Tick_IncrementTime()
        {
            ServerDuration++;
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

    interface ILindholmComponent
    {
        void Start();
    }

    //todosoon destroy this. It's bad.
    public abstract class WrapperComponent
    {
        protected Game wrapper;
        protected CustomGame cg;

        public WrapperComponent(Game wrapperInject)
        {
            wrapper = wrapperInject;
            cg = wrapper.CG;
        }
    }

}
