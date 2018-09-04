using System;
using System.Collections.Generic;
using System.Linq;
using Deltin.CustomGameAutomation;

namespace Lindholm
{

    internal abstract class BaseSlots
    {
        internal BaseSlots() { }

        public List<int> Slots()
        {
            return Slots(Team.Blue).Union(Slots(Team.Red)).ToList();
        }

        public List<int> Slots(Team team)
        {
            if (team == Team.Blue)
            {
                return BlueSlots;
            }
            else
            {
                return RedSlots;
            }
        }

        public int Count()
        {
            return Slots().Count;
        }

        public int Count(Team team)
        {
            return Slots(team).Count;
        }

        //USER DEFINED
        abstract protected List<int> BlueSlots { get; }
        abstract protected List<int> RedSlots { get; }
    }

    internal class PlayerSlots : BaseSlots
    {
        PlayerManager playerManager;
        public PlayerSlots(PlayerManager playerManager) : base()
        {
            this.playerManager = playerManager;
        }

        protected override List<int> BlueSlots {
            get {
                try
                {
                    return playerManager.PlayerSlotHistory[Team.Blue][playerManager.PlayerSlotHistory[Team.Blue].Count - 1];
                }
                catch (ArgumentOutOfRangeException)
                {
                    return new List<int>();
                }
            }
        }

        protected override List<int> RedSlots {
            get {
                try
                {
                    return playerManager.PlayerSlotHistory[Team.Red][playerManager.PlayerSlotHistory[Team.Red].Count - 1];
                }
                catch (ArgumentOutOfRangeException)
                {
                    return new List<int>();
                }
            }
        }
    }

    internal class BotSlots : BaseSlots
    {
        private BotManager botManager;

        public BotSlots(BotManager botManager) : base()
        {
            this.botManager = botManager;
        }

        protected override List<int> BlueSlots {
            get {
                return botManager.IBlueBotSlots;
            }
        }

        protected override List<int> RedSlots {
            get {
                return botManager.IRedBotSlots;
            }
        }
    }


    class SlotManager : WrapperComponent
    {
        public AllSlots all;
        public EmptySlots empty;
        public FilledSlots filled;
        public PlayerSlots players;
        public BotSlots bots;

        public SlotManager(Lindholm wrapper) : base(wrapper)
        {
            all = new AllSlots();
            filled = new FilledSlots(this.cg);
            empty = new EmptySlots(this);
        }

        public class AllSlots : BaseSlots
        {
            public AllSlots() : base()
            {
            }

            protected override List<int> BlueSlots {
                get {
                    return new List<int>() { 0, 1, 2, 3, 4, 5 };
                }
            }

            protected override List<int> RedSlots {
                get {
                    return new List<int>() { 6, 7, 8, 9, 10, 11 };
                }
            }
        }

        public class FilledSlots : BaseSlots
        {
            private CustomGame cg;
            public FilledSlots(CustomGame cg) : base()
            {
                this.cg = cg;
            }

            protected override List<int> BlueSlots {
                get {
                    return cg.BlueSlots;
                }
            }

            protected override List<int> RedSlots {
                get {
                    return cg.RedSlots;
                }
            }
        }

        public class EmptySlots : BaseSlots
        {
            private SlotManager slotManager;
            public EmptySlots(SlotManager slotManager) : base()
            {
                this.slotManager = slotManager;
            }

            protected override List<int> BlueSlots {
                get {
                    return slotManager.all.Slots(Team.Blue).Except(slotManager.cg.BlueSlots).ToList();
                }
            }

            protected override List<int> RedSlots {
                get {
                    return slotManager.all.Slots(Team.Blue).Except(slotManager.cg.RedSlots).ToList();
                }
            }
        }

        public bool TeamHasMorePlayers(Team team)
        {
            if (team == Team.Blue)
            {
                return players.Count(Team.Blue) > players.Count(Team.Red);
            }
            else
            {
                return players.Count(Team.Red) > players.Count(Team.Blue);
            }
        }

        public Team TeamWithMoreOrEqualPlayers()
        {
            if (TeamHasMorePlayers(Team.Blue))
            {
                return Team.Blue;
            }
            else
            {
                return Team.Red;
            }
        }

        public bool TeamsHaveEqualPlayers {
            get {
                return players.Count(Team.Blue) == players.Count(Team.Red);
            }
        }
    }
}