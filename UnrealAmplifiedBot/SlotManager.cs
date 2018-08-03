using System;
using System.Collections.Generic;
using System.Linq;

namespace BotLibrary
{

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
}