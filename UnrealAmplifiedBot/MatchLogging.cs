//using Deltin.CustomGameAutomation;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//
//namespace Lindholm
//{
//
//    //todo track the length a hero is played before swapping off. Account for the player changing slots.
//
//    class MatchLogger : WrapperComponent
//    {
//        private MatchLog currMatchLog;
//
//        public MatchLogger(Game wrapperInject) : base(wrapperInject) { }
//
//        public void BeginMatchLog()
//        {
//            System.Diagnostics.Debug.WriteLine("Beginning log");
//            currMatchLog = new MatchLog
//            {
//                map = wrapper.Maps.currMap
//            };
//            //currMatchLog.modeIndex = wrapper.maps.mode_i; //fixme add mode
//        }
//
//        private void LogAtGameOver(object sender, GameOverArgs args)
//        {
//            if (currMatchLog != null)
//            {
//                currMatchLog.winning_team = args.GetWinningTeam();
//            }
//        }
//
//        public void UpdatematchLog()
//        {
//            if (currMatchLog != null)
//            {
//                if (wrapper.Slots.Players.Count() > 0)
//                {
//                    currMatchLog.duration++;
//
//                    currMatchLog.player_count[Team.Blue] += wrapper.Slots.Players.Count(Team.Blue);
//                    currMatchLog.player_count[Team.Red] += wrapper.Slots.Players.Count(Team.Red);
//
//                    int BlueJoins = wrapper.Players.joins.Count(Team.Blue);
//                    int BlueLeaves = wrapper.Players.leaves.Count(Team.Blue);
//
//                    int RedJoins = wrapper.Players.joins.Count(Team.Red);
//                    int RedLeaves = wrapper.Players.joins.Count(Team.Red);
//
//                    currMatchLog.joins += BlueJoins + RedJoins;
//                    currMatchLog.leaves += BlueLeaves + RedLeaves;
//
//
//                    foreach (int slot in wrapper.Slots.Players.Slots(Team.Blue))
//                    {
//                        Hero? hero_or_null = cg.PlayerInfo.GetHero(slot);
//                        if (hero_or_null != null)
//                        {
//                            Hero hero = (Hero)hero_or_null;
//                            currMatchLog.hero_play_time[Team.Blue][hero]++;
//                        }
//                    }
//                    foreach (int slot in wrapper.Slots.Players.Slots(Team.Red))
//                    {
//                        Hero? hero_or_null = cg.PlayerInfo.GetHero(slot);
//                        if (hero_or_null != null)
//                        {
//                            Hero hero = (Hero)hero_or_null;
//                            currMatchLog.hero_play_time[Team.Red][hero]++;
//                        }
//                    }
//                }
//            }
//        }
//
//        public void EndMatchLog()
//        {
//            System.Diagnostics.Debug.WriteLine("Saving log");
//            if (currMatchLog != null)
//            {
//                currMatchLog.endTime = DateTime.Now;
//
//                //Todo choose your own path to log
//                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"D:\life itself\Unreal300%\Logs\GameLogs.txt", true))
//                {
//                    file.WriteLine("=============================\n");
//                    file.WriteLine(String.Format("Start: {0}\n", currMatchLog.startTime));
//                    file.WriteLine(String.Format("End: {0}\n", currMatchLog.endTime));
//                    file.WriteLine(String.Format("Winning team: {0}\n", currMatchLog.winning_team));
//                    file.WriteLine(String.Format("Map: {0}\n", Map.MapNameFromID(currMatchLog.map)));
//                    //file.WriteLine(String.Format("Mode index: {0}\n", currMatchLog.modeIndex)); //todosoon set to use map tag, not a number
//
//                    file.WriteLine(String.Format("Joins: {0}\n", currMatchLog.joins));
//                    file.WriteLine(String.Format("Leaves: {0}\n", currMatchLog.leaves));
//
//                    file.WriteLine(String.Format("Blue player count: {0}\n", (double)currMatchLog.player_count[Team.Blue] / (double)currMatchLog.duration));
//                    file.WriteLine(String.Format("Red player count: {0}\n", (double)currMatchLog.player_count[Team.Red] / (double)currMatchLog.duration));
//
//                    Team[] teams = { Team.Blue, Team.Red };
//                    foreach (Team team in teams)
//                    {
//                        string[] names = Enum.GetNames(typeof(Hero));
//                        Hero[] heroes = Enum.GetValues(typeof(Hero)).Cast<Hero>().ToArray();
//
//                        for (int i = 0; i < heroes.Length; i++)
//                        {
//                            file.WriteLine(String.Format(
//                                "{0} {1} Average count: {2}\n",
//                                team,
//                                names[i],
//                                (double)currMatchLog.hero_play_time[team][heroes[i]] / (double)currMatchLog.duration));
//                        }
//
//                    }
//
//                }
//            }
//
//        }
//    }
//
//    class MatchLog
//    {
//        public DateTime startTime;
//        public DateTime endTime;
//        public int duration = 0;
//
//        public int joins = 0;
//        public int leaves = 0;
//
//        public Map map;
//        //private int LockLevel = 0;
//
//        public Dictionary<Team, Dictionary<Hero, int>> hero_play_time;
//        public Dictionary<Team, int> player_count;
//        public PlayerTeam winning_team;
//
//        public MatchLog()
//        {
//            startTime = DateTime.Now;
//
//            hero_play_time = new Dictionary<Team, Dictionary<Hero, int>>()
//            {
//                {
//                    Team.Blue, new Dictionary<Hero, int>()
//                    {
//                        { Hero.Doomfist, 0 },
//                        { Hero.Genji, 0 },
//                        { Hero.McCree, 0 },
//                        { Hero.Pharah, 0 },
//                        { Hero.Reaper, 0 },
//                        { Hero.Soldier76, 0 },
//                        { Hero.Sombra, 0 },
//                        { Hero.Tracer, 0 },
//                        { Hero.Bastion, 0 },
//                        { Hero.Hanzo, 0 },
//                        { Hero.Junkrat, 0 },
//                        { Hero.Mei, 0 },
//                        { Hero.Torbjorn, 0 },
//                        { Hero.Widowmaker, 0 },
//                        { Hero.DVA, 0 },
//                        { Hero.Orisa, 0 },
//                        { Hero.Reinhardt, 0 },
//                        { Hero.Roadhog, 0 },
//                        { Hero.Winston, 0 },
//                        { Hero.Zarya, 0 },
//                        { Hero.Ana, 0 },
//                        { Hero.Brigitte, 0 },
//                        { Hero.Lucio, 0 },
//                        { Hero.Mercy, 0 },
//                        { Hero.Moira, 0 },
//                        { Hero.Symmetra, 0 },
//                        { Hero.Zenyatta, 0 }
//                    }
//                },
//                {
//                    Team.Red, new Dictionary<Hero, int>()
//                    {
//                        { Hero.Doomfist, 0 },
//                        { Hero.Genji, 0 },
//                        { Hero.McCree, 0 },
//                        { Hero.Pharah, 0 },
//                        { Hero.Reaper, 0 },
//                        { Hero.Soldier76, 0 },
//                        { Hero.Sombra, 0 },
//                        { Hero.Tracer, 0 },
//                        { Hero.Bastion, 0 },
//                        { Hero.Hanzo, 0 },
//                        { Hero.Junkrat, 0 },
//                        { Hero.Mei, 0 },
//                        { Hero.Torbjorn, 0 },
//                        { Hero.Widowmaker, 0 },
//                        { Hero.DVA, 0 },
//                        { Hero.Orisa, 0 },
//                        { Hero.Reinhardt, 0 },
//                        { Hero.Roadhog, 0 },
//                        { Hero.Winston, 0 },
//                        { Hero.Zarya, 0 },
//                        { Hero.Ana, 0 },
//                        { Hero.Brigitte, 0 },
//                        { Hero.Lucio, 0 },
//                        { Hero.Mercy, 0 },
//                        { Hero.Moira, 0 },
//                        { Hero.Symmetra, 0 },
//                        { Hero.Zenyatta, 0 }
//                    }
//                }
//            };
//            player_count = new Dictionary<Team, int>() {
//                {Team.Blue, 0 },
//                {Team.Red, 0 }
//            };
//
//
//        }
//
//    }
//
//
//}