using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace _WF_TelegramPCManager
{
    public class Commands
    {
        private Telegram telegram;
        public Commands(Telegram telegram)
        {
            this.telegram = telegram;
        }
        /// <summary>
        /// Команда для выключения компьютера
        /// </summary>
        public async Task ShutDown(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: $"Выключаю компьютер!",
                cancellationToken: cancellationToken);

            Process.Start(@"C:\Windows\System32\shutdown.exe", "/s /f /t 10");
        }
        /// <summary>
        /// Команда для получения идентификатора пользователя из telegram
        /// </summary>
        public async Task GetMyUserId(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: $"Твой UserId: {update.Message.Chat.Id}",
                cancellationToken: cancellationToken) ;
        }

        /// <summary>
        /// Команда для получения времени работы компьютера
        /// </summary>
        public async Task Status(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            Console.WriteLine("Команда Status");

            String strResult = "Время работы компьютера ";
            strResult += Convert.ToString(Environment.TickCount / 86400000) + " дней, ";
            strResult += Convert.ToString(Environment.TickCount / 3600000 % 24) + " часов, ";
            strResult += Convert.ToString(Environment.TickCount / 120000 % 60) + " минут, ";
            strResult += Convert.ToString(Environment.TickCount / 1000 % 60) + " секунд.";

            
            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: strResult,
                cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Отображение информации что такой команды не существует
        /// </summary>
        public async Task CommandMissing(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: $"Отсуствует команда '{update.Message.Text}'",
                cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Отображение сообщения что не хватает прав для того, чтобы использовать данный бот
        /// </summary>
        public async Task PrivilagesMissing(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: $"У вас не достаточно прав, для этого бота!",
                cancellationToken: cancellationToken);
        }
    }
}
