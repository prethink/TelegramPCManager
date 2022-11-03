using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace _WF_TelegramPCManager.TelegramCore
{
    public static class BotHandler
    {
        public delegate Task Command(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken);
        static Dictionary<long, Command> Step = new();
        static Dictionary<long, List<Dictionary<string,string>>> Value = new();

        public static void RegisterNextStep(this ITelegramBotClient bot, long userId, Command command)
        {
            ClearStepUser(bot, userId);
            Step.Add(userId, command);
        }

        public static KeyValuePair<long, Command> GetStepOrNull(this ITelegramBotClient bot, long userId)
        {
            return Step.FirstOrDefault(x => x.Key == userId);
        }

        public static void ClearStepUser(this ITelegramBotClient bot, long userId)
        {
            if (HasStep(bot, userId))
            {
                Step.Remove(userId);
            }

        }

        public static bool HasStep(this ITelegramBotClient bot, long userId)
        {
            return Step.ContainsKey(userId);
        }

    }
}
