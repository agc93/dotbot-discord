# Dotbot.Discord

An adapter for the [Dotbot framework](https://github.com/botnetframework/dotbot) for [Discord](https://discordapp.com/).

Powered by [Discord.NET](https://github.com/RogueException/Discord.Net).

## Getting Started

To add the Discord adapter to your bot, just update your startup code:

```csharp
// Build the robot.
var robot = new RobotBuilder()
    // ...
    .UseDiscord("MY_DISCORD_BOT_TOKEN")
    // ...
    .Build();
```