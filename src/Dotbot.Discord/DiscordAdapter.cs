using System;
using System.Threading;
using System.Threading.Tasks;
using Dotbot.Diagnostics;
using Dotbot.Models.Events;

namespace Dotbot.Discord
{
    public class DiscordAdapter : IAdapter, IWorker
    {
        private readonly DiscordBroker _broker;
        private readonly IEventQueue _queue;
        private readonly ILog _log;

        public DiscordAdapter(DiscordBroker broker, IEventQueue queue, ILog log)
        {
            _broker = broker;
            _queue = queue;
            _log = log;
        }

        public string FriendlyName => "Discord Adapter";

        public IBroker Broker => _broker;

        public async Task<bool> Run(CancellationToken token)
        {
            var client = await _broker.Connect();
            if (!client.IsValid) throw new InvalidOperationException("Could not connect to Discord.");
            _log.Information("Connected to Discord.");
            _broker.Client.MessageReceived += (sender, args) =>
            {
                _log.Verbose("Discord: Message received, adding event to queue.");
                _queue.Enqueue(new MessageEvent(_broker)
                {
                    Bot = client.Bot,
                    Message = new Dotbot.Models.Message {Text = args.Message.Text, User = args.User.ToBotUser()},
                    Room = args.Channel.ToRoom()
                });
            };
            _log.Verbose("Discord: Adapter started, listening for messages.");
            token.WaitHandle.WaitOne(Timeout.Infinite);
            return true;
        }
    }
}