using Deltin.CustomGameAutomation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lindholm
{
    public partial class Lindholm : IDisposable
    {

        public BotTeam TeamToBotTeam(Team team)
        {
            if (team == Team.Blue)
            {
                return BotTeam.Blue;
            }
            else
            {
                return BotTeam.Red;
            }
        }

        public Team OtherTeam(Team team)
        {
            if(team == Team.Blue)
            {
                return Team.Red;
            }
            else
            {
                return Team.Blue;
            }
        }

        public Team TeamWithSlot(int slot)
        {
            if (slots.all.Slots(Team.Blue).Contains(slot))
            {
                return Team.Blue;
            }
            else if(slots.all.Slots(Team.Red).Contains(slot))
            {
                return Team.Red;
            }
            else
            {
                throw ( new ArgumentOutOfRangeException("Slot must belong to red or blue."));
            }

        }

        public readonly List<Team> TEAMS = new List<Team>() { Team.Blue, Team.Red };

    }

    public enum Team // For designer use in referring to teams. Doesn't mirror any in game use of teams, like BotTeam or InviteTeam. No value generally indicates both teams.
    {
        Blue,
        Red,
    }

}
