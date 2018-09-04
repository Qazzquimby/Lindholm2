// ReSharper disable UnusedMember.Global

using System;

namespace Lindholm.Chat
{
    public enum Channel
    {
        Team,

        Match,

        General,

        Group,

        PrivateMessage
    }

    public static class ChannelExtension
    {
        public static Deltin.CustomGameAutomation.Channel ToDeltin(this Channel lindholmChannel)
        {
            var deltinChannel =
                (Deltin.CustomGameAutomation.Channel) Enum.Parse(typeof(Deltin.CustomGameAutomation.Channel),
                    lindholmChannel.ToString());
            return deltinChannel;
        }
    }
}