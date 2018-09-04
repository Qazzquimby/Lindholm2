using System;
using System.Collections.Generic;

namespace Lindholm.Slots
{
    public class SlotsManager
    {
        public AllSlots All;
        public EmptySlots Empty;
        public FilledSlots Filled;
        public PlayerSlots Players;
        public BotSlots Bots;

        private readonly SlotContentHistory _history = new SlotContentHistory();
        private bool _botsModified; //Triggers safety code in Update while true, then unset.

        public SlotsManager()
        {
            All = new AllSlots(_history);
            Filled = new FilledSlots(_history);
            Empty = new EmptySlots(_history);
            Players = new PlayerSlots(_history);
            Bots = new BotSlots(_history);
        }

        internal void FlagBotsModified()
        {
            _botsModified = true;
        }

        internal void Update()
        {
            if (_botsModified)
            {
                _botsModified = false;
            }
        }
    }

    public static class SlotConstants
    {
        private static List<int> BlueSlots { get; } = new List<int>() {0, 1, 2, 3, 4, 5};
        private static List<int> RedSlots { get; } = new List<int>() {6, 7, 8, 9, 10, 11};

        public static List<int> AllSlots(Team team)
        {
            if (team == Team.Blue)
            {
                return BlueSlots;
            }

            return RedSlots;
        }

        public static Team TeamWithSlot(int slot)
        {
            if (BlueSlots.Contains(slot))
            {
                return Team.Blue;
            }
            else if (RedSlots.Contains(slot))
            {
                return Team.Red;
            }
            else
            {
                throw (new ArgumentOutOfRangeException(nameof(slot), "Must belong to red or blue."));
            }
        }
    }


    //    internal class PlayerSlots : BaseSlots
    //    {
    //        PlayerManager playerManager;
    //        public PlayerSlots(PlayerManager playerManager) : base()
    //        {
    //            this.playerManager = playerManager;
    //        }
    //
    //        protected override List<int> BlueSlots {
    //            get {
    //                try
    //                {
    //                    return playerManager.PlayerSlotHistory[Team.Blue][playerManager.PlayerSlotHistory[Team.Blue].Count - 1];
    //                }
    //                catch (ArgumentOutOfRangeException)
    //                {
    //                    return new List<int>();
    //                }
    //            }
    //        }
    //
    //        protected override List<int> RedSlots {
    //            get {
    //                try
    //                {
    //                    return playerManager.PlayerSlotHistory[Team.Red][playerManager.PlayerSlotHistory[Team.Red].Count - 1];
    //                }
    //                catch (ArgumentOutOfRangeException)
    //                {
    //                    return new List<int>();
    //                }
    //            }
    //        }
    //    }







    //
    //
    //
    //    internal class JoinSlots : BaseSlots
    //    {
    //        Dictionary<Team, List<List<int>>> History;
    //        public JoinSlots(Dictionary<Team, List<List<int>>> history) : base()
    //        {
    //            History = history;
    //        }
    //
    //        protected override List<int> BlueSlots {
    //            get {
    //                Team team = Team.Blue;
    //                return Slots(team).Except(History[team][-1]).ToList();
    //            }
    //        }
    //
    //        protected override List<int> RedSlots {
    //            get {
    //                Team team = Team.Red;
    //                return Slots(team).Except(History[team][-1]).ToList();
    //            }
    //        }
    //    }
    //
    //    internal class LeaveSlots : BaseSlots
    //    {
    //        Dictionary<Team, List<List<int>>> History;
    //        public LeaveSlots(Dictionary<Team, List<List<int>>> history) : base()
    //        {
    //            History = history;
    //        }
    //
    //        protected override List<int> BlueSlots {
    //            get {
    //                Team team = Team.Blue;
    //                return History[team][-1].Except(Slots(team)).ToList();
    //            }
    //        }
    //
    //        protected override List<int> RedSlots {
    //            get {
    //                Team team = Team.Red;
    //                return History[team][-1].Except(Slots(team)).ToList();
    //            }
    //        }
    //    }
    //
    //    
}