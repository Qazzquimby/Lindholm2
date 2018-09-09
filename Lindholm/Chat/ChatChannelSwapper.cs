namespace Lindholm.Chat
{
    internal class ChatChannelSwapper : IChatChannelSwapper
    {
        private readonly IChatDeltinChannelSwapper _deltinSwapper;
        private Channel? _currentChannel;
        private Channel? _previousChannel;

        public ChatChannelSwapper(IChatDeltinChannelSwapper deltinSwapper)
        {
            _currentChannel = null;
            _previousChannel = null;
            _deltinSwapper = deltinSwapper;
        }

        public void SwapChannel(Channel? channel)
        {
            if (channel == null)
            {
                return;
            }

            var nonNullChannel = channel.Value;
            _previousChannel = _currentChannel;
            _currentChannel = channel;
            _deltinSwapper.SwapChannel(nonNullChannel);
        }

        public void SwapChannelIfDifferent(Channel channel)
        {
            if (_currentChannel != channel)
            {
                SwapChannel(channel);
            }
        }

        public void SwapBack()
        {
            if (_previousChannel != null)
            {
                SwapChannel(_previousChannel);
            }
        }
    }
}