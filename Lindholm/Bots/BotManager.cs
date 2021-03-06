﻿// ReSharper disable InvertIf

namespace Lindholm.Bots
{
    public class BotManager : IBotManager
    {
        public IBotRequester BotRequester { get; }
        public IBotRequestFulfillmentManager BotRequestFulfillmentManager { get; }

        internal BotManager(IBotRequester botRequester, IBotRequestFulfillmentManager botRequestFulfillmentManager)
        {
            BotRequester = botRequester;
            BotRequestFulfillmentManager = botRequestFulfillmentManager;
        }


//        private bool IsBotStateCorrupt(Team team)
//        {
//            return _previousBotExpectations[team].Count != wrapper.BotSlots.bots.Count(team);
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