namespace Dotbot.Discord
{
    public class DiscordContext
    {
        public DiscordContext(Dotbot.Models.User user, bool isValid)
        {
            Bot = user;
            IsValid = isValid;
        }
        public bool IsValid {get;}
        public Dotbot.Models.User Bot {get;}
    }
}