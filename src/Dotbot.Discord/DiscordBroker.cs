using System;
using System.Threading.Tasks;
using Discord;
using Dotbot.Diagnostics;
using Dotbot.Models;
using User = Dotbot.Models.User;

namespace Dotbot.Discord
{
    public class DiscordBroker : IBroker
    {
        private readonly DiscordConfiguration _configuration;
        private readonly ILog _log;

        public DiscordBroker(DiscordConfiguration configuration, ILog log)
        {
            _configuration = configuration;
            _log = log;
        }

        internal async Task<DiscordContext> Connect()
        {
            Client = new DiscordClient(c =>
            {
                c.LogHandler = HandleLogMessage;
                c.AppName = "Dotbot.Discord";
                c.CacheToken = true;
            });
            await Client.Connect(_configuration.Token, TokenType.Bot);
            return new DiscordContext(Client.CurrentUser.ToBotUser(), Client.State == ConnectionState.Connected);
        }

        internal DiscordClient Client { get; private set; }

        public async Task Broadcast(Room room, string text)
        {
            var channel = Client.GetChannel(ulong.Parse(room.Id));
            await channel.SendMessage(text);
        }

        public async Task Reply(Room room, User fromUser, string text)
        {
            var channel = Client.GetChannel(ulong.Parse(room.Id));
            await channel.SendMessage(text);
        }

        public void HandleLogMessage(object sender, LogMessageEventArgs logEvent)
        {
            _log.Write(logEvent.Severity.ToLogLevel(), "Discord Client: {0}", logEvent.Message);
        }
    }
}