using Lindholm;
using Deltin.CustomGameAutomation;
using System;
using System.Collections.Generic;

namespace Lindholm
{

    partial class Lindholm : IDisposable
    {
        public CustomGame cg;

        internal GameLoop loop;

        internal Chat chat;
        internal PhaseManager phases;
        internal MatchManager match;
        internal MapChooser maps;

        internal BotManager bots;
        internal PlayerManager players;


        internal MatchLogger logger;
        internal SlotManager slots;
        internal JoinManager joins;

        public int ServerDuration
        {
            get; internal set;
        } = 0;

        private List<Action> StartFuncs = new List<Action>();
        private bool blueNameSet;
        private bool redNameSet;
        private CustomGameBuilder builder = new CustomGameBuilder();

        public Lindholm() : this(ScreenshotMethod.BitBlt) { }

        public Lindholm(ScreenshotMethod screenshotMethod)
        {
            builder.ScreenshotMethod = screenshotMethod;
            builder.OpenChatIsDefault = true; //todolater turn on only when command listening is on.
            cg = new CustomGame(builder);
            slots = new SlotManager(this);

            bots = new BotManager(this);
            players = new PlayerManager(this);
            
            match = new MatchManager(this);
            chat = new Chat(this);
            loop = new GameLoop(this);

            logger = new MatchLogger(this);
            phases = new PhaseManager(this);
            maps = new MapChooser(this, Map.A_HorizonLunarColony); //todolater get this from the user. Also need to load preset

            
            joins = new JoinManager(this);

            loop.AddPhaselessLoop(Tick_IncrementTime, 1);
        }

        public void SetGameName(string name)
        {
            cg.Settings.SetGameName(name);
        }

        public void SetBlueName(string name)
        {
            blueNameSet = true;
            cg.Settings.SetTeamName(PlayerTeam.Blue, @"\ " + name);
        }

        public void SetRedName(string name)
        {
            redNameSet = true;
            cg.Settings.SetTeamName(PlayerTeam.Red, "* " + name);
        }

        public void SetPreset(int preset, int numPresets)
        {
            //cg.Settings.SetNumPresets(numPresets); //fixme readd if deltin doesn't mind
            SetPreset(preset);
        }

        public void SetPreset(int preset)
        {
            cg.Settings.LoadPreset(preset);
        }


        public void Start()
        {
            Dev.Log("Starting");
            FixNames();
            chat.Start();
            PerformStartFunctions();
            loop.Start();
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
            chat.MatchChat("Beep boop. Starting up.");
        }

        public void Start_EnterGame()
        {
            if (cg.GetGameState() == GameState.InLobby)
            {
                cg.StartGame();
            }
        }

        /// <summary>
        /// Disposes of custom game resources.
        /// </summary>
        public void Dispose()
        {
            cg.Dispose();
            
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

    abstract class WrapperComponent
    {
        protected Lindholm wrapper;
        protected CustomGame cg;

        public WrapperComponent(Lindholm wrapperInject)
        {
            wrapper = wrapperInject;
            cg = wrapper.cg;
        }
    }

}
