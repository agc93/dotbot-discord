using System;
using Microsoft.Extensions.DependencyInjection;

namespace Dotbot.Discord
{
    public static class DiscordBuilder
    {
        public static RobotBuilder UseDiscord(this RobotBuilder builder, string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            builder.Services.AddSingleton(new DiscordConfiguration {Token = token});

            builder.Services.AddSingleton<DiscordAdapter>();
            builder.Services.AddSingleton<IAdapter>(s => s.GetService<DiscordAdapter>());
            builder.Services.AddSingleton<IWorker>(s => s.GetService<DiscordAdapter>());

            builder.Services.AddSingleton<DiscordBroker>();
            builder.Services.AddSingleton<IBroker>(s => s.GetService<DiscordBroker>());

            return builder;
        }
    }
}