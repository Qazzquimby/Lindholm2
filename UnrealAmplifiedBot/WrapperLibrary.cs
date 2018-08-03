using BotLibrary;
using Deltin.CustomGameAutomation;
using System;
using System.Collections.Generic;

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
            maps = new MapChooser(this, Map.A_HorizonLunarColony); //todolater get this from the user. Also need to load preset

            slots = new SlotManager(this);
            joins = new JoinManager(this);

            loop.AddPhaselessLoop(Tick_IncrementTime, 1);
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

        public void SetPreset(int preset, int numPresets)
        {
            cg.GameSettings.SetNumPresets(numPresets);
            SetPreset(preset);
        }

        public void SetPreset(int preset)
        {
            cg.GameSettings.LoadPreset(preset);
        }


        public void Start()
        {
            Debug.Log("Starting");
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

}
