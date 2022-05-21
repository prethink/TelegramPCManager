using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace _WF_TelegramPCManager
{
    public class Commands
    {
        #region кнопки
        const string USER_ID = "👤 UserId";
        const string TIME_WORK = "⌛ Время работы ПК";
        const string USAGE_PC = "🌡 Нагрузка ПК";
        const string SHUTDOWN = "🟠 Выключить ПК";
        #endregion



        delegate Task Command(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken);
        Dictionary<Tuple<string,bool>, Command> _commands; 
        private Telegram telegram;
        public Commands(Telegram telegram)
        {
            this.telegram = telegram;
            _commands = new Dictionary<Tuple<string, bool>, Command> ();
            RegisterCommands();
        }

        /// <summary>
        /// Регистрация всех доступных комманд
        /// </summary>
        public void RegisterCommands()
        {
            /*Формат регистрации Вызов команды, требуется права доступа, команда*/
            _commands.Add(Tuple.Create(USER_ID, false), GetMyUserId);
            _commands.Add(Tuple.Create("/menu", true), MainMenu);
            _commands.Add(Tuple.Create(SHUTDOWN, true), ShutDown);
            _commands.Add(Tuple.Create(TIME_WORK, true), WorkTime);
            _commands.Add(Tuple.Create(USAGE_PC, true), UsageComputer);

        }

        /// <summary>
        /// Выполнение команды
        /// </summary>
        /// <param name="command">Запрос команды</param>
        public async Task ExecuteCommand(string command, ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            foreach (var item in _commands)
            {
                if(item.Key.Item1.ToLower() == command.ToLower())
                {
                    //Требуется проверить права доступа к команде
                    if(item.Key.Item2)
                    {
                        //Получение данных о правах пользователя
                        bool HasPrivilage = Common.HasAccess(update.Message.Chat.Id);
                        //Нет прав доступа
                        if(!HasPrivilage)
                        {
                            await PrivilagesMissing(botClient, update, cancellationToken);
                            return;
                        }
                    }
                    //Выполнение команды
                    await item.Value(botClient, update, cancellationToken);
                    return;
                }
            }
            //Сообщение что команда не найдена
            await CommandMissing(botClient, update, cancellationToken);
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

            Computer.ShutDown();
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
        public async Task WorkTime(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            string workTime = Computer.GetWorkTime();

            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: workTime,
                cancellationToken: cancellationToken);
        }
        /// <summary>
        /// Нагрузка компьютера
        /// </summary>
        public async Task UsageComputer(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var memory = Computer.GetMemoryUsage();
            var cpu = await Computer.GetCPULoad();

            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: cpu + memory,
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

        public async Task MainMenu(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
                {
                new KeyboardButton[] { USER_ID },
                new KeyboardButton[] { SHUTDOWN },
                new KeyboardButton[] { TIME_WORK },
                new KeyboardButton[] { USAGE_PC },
            })
            {
                ResizeKeyboard = true
            };

            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "Главное меню",
                replyMarkup: replyKeyboardMarkup,
                cancellationToken: cancellationToken);
        }
    }
}
