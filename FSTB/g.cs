using Telegram.Bot;
using Telegram.Bot.Types;

namespace FSTB
{
    public class g
    {
        public static TelegramBotClient TLC;
        public static ChatId            MomChatChat = new(699561154L);

        static g() => TLC = new TelegramBotClient("5240710415:AAHEpsaDb3jCRyOQX2Ju7Xkg4x8tycew7b0");
    }
}
