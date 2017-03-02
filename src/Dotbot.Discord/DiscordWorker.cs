using System;
using System.Threading;
using System.Threading.Tasks;
using Dotbot.Diagnostics;
using Dotbot.Models.Events;

namespace Dotbot.Discord
{
    public class DiscordWorker : IWorker
    {
        private DiscordBroker _broker;
        private IEventQueue _queue;
        private ILog _log;

        public DiscordWorker(DiscordBroker broker, IEventQueue queue, ILog log)
        {
            _broker = broker;
            _queue = queue;
            _log = log;
        }
        public string FriendlyName => throw new NotImplementedException();

        public async Task<bool> Run(CancellationToken token)
        {
            var client = await _broker.Connect();
            if (!client.IsValid) throw new InvalidOperationException("Could not connect to Discord.");
            _broker.Client.MessageReceived += (sender, args) => {
                _queue.Enqueue(new MessageEvent(_broker) {
                    Bot = client.Bot,
                    Message = new Dotbot.Models.Message { Text = args.Message.Text, User = args.User.ToBotUser()},
                    Room = args.Channel.ToRoom()
                });
            };
            token.WaitHandle.WaitOne(Timeout.Infinite);
            return true;
        }
    }
}