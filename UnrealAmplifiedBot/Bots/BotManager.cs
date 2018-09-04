// ReSharper disable InvertIf

namespace Lindholm.Bots
{
    public class BotManager
    {
        public BotRequester BotRequester { get; }
        private BotExpectations BotExpectations { get; }

        internal BotManager(BotRequester botRequester, BotExpectations botExpectations)
        {
            BotRequester = botRequester;
            BotExpectations = botExpectations;
        }


//        private bool IsBotStateCorrupt(Team team)
//        {
//            return _previousBotExpectations[team].Count != wrapper.Slots.bots.Count(team);
//        }



        //        public void Start()
        //        {
        //            wrapper.Players.AddJoinOrLeaveFunc(HandleBots);
        //            RemoveBots(); //Simplifies bot management by starting with no bots.
        //        }

        //        public void HandleBots()
        //        {
        //            if (wrapper.Maps.IsDMOrTDM)
        //            {
        //                RemoveBotsIfAny();
        //            }
        //            else
        //            {
        //                UpdateBotSlots(); //Fixme, Shouldn't be needed frequently. A failsafe.
        //                UpdateBotExpectations();
        //                FulfillBotExpectations();
        //            }
        //        }

    }
}