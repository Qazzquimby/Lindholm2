using Deltin.CustomGameAutomation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BotLibrary
{



    class MapChooser : WrapperComponent
    {
        public Map currMap { get; internal set; }

        public Gamemode currMode
        {
            get
            {
                return currMap.GameMode;
            }
        }
        
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

            currMap = initialMap;

            wrapper.match.AddGameOverFunc(SetRandomMap);

            modes = new Map[][]{
                AE_maps, A_maps, C_maps, TDM_maps, E_maps
                };

        }

        public void SetInitialMap(Map map)
        {
            currMap = map;
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
                System.Diagnostics.Debug.WriteLine(String.Format("Recently went to {0}. Rerolling.", Map.MapNameFromID(map)));
                map = GetRandomMap();
            }

            System.Diagnostics.Debug.WriteLine(String.Format("New Map is {0}.", Map.MapNameFromID(map)));

            SetMap(map);
        }

        public void SetMap(Map map)
        {
            cg.ToggleMap(ToggleAction.DisableAll, map);
            if (recent_maps.Count >= 5)
            {
                recent_maps.RemoveAt(0);
            }
            recent_maps.Add(map);
            currMap = map;
        }

        public void RandomAndNextMap()
        {
            wrapper.maps.SetRandomMap();
            NextMap();
        }

        public void NextMap()
        {
            //todo set MatchManger match duration to 0 on new match
            Debug.Log("Restarting game");
            cg.RestartGame();
            Debug.Log("Entering setup phase");
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
            return mode;
        }
    }
}