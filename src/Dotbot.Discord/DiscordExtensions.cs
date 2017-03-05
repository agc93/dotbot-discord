using User = Discord.User;
using LogSeverity = Discord.LogSeverity;
using Profile = Discord.Profile;
using Channel = Discord.Channel;
using Dotbot.Diagnostics;

namespace Dotbot.Discord
{
    public static class DiscordExtensions
    {
        public static Models.User ToBotUser(this User user)
        {
            return new Models.User(user.Id.ToString(), user.Nickname, user.Name);
        }

        public static Models.User ToBotUser(this Profile profile)
        {
            return new Models.User(profile.Id.ToString(), profile.Email, profile.Name);
        }

        public static LogLevel ToLogLevel(this LogSeverity severity)
        {
            switch (severity)
            {
                case LogSeverity.Debug:
                    return LogLevel.Debug;
                case LogSeverity.Error:
                    return LogLevel.Error;
                case LogSeverity.Info:
                    return LogLevel.Information;
                case LogSeverity.Verbose:
                    return LogLevel.Verbose;
                case LogSeverity.Warning:
                    return LogLevel.Warning;
                default:
                    return LogLevel.Fatal;
            }
        }

        public static Models.Room ToRoom(this Channel channel)
        {
            return new Models.Room(channel.Id.ToString(), channel.Name);
        }
    }
}