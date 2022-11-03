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
        const string USER_ID            = "👤 UserId";
        const string TIME_WORK          = "⌛ Время работы ПК";
        const string USAGE_PC           =  "🌡 Нагрузка ПК";
        const string SHUTDOWN           = "🟠 Выключить ПК";
        const string MAIN_MEHU          = "🗺 Главное меню";
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
            _commands.Add(Tuple.Create("/menu", false), MainMenu);
            _commands.Add(Tuple.Create("/start", false), MainMenu);
            _commands.Add(Tuple.Create(MAIN_MEHU, false), MainMenu);
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

        /// <summary>
        /// Отображение главного меню
        /// </summary>
        public async Task MainMenu(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            List<string> menu = new();
            menu.Add(USER_ID);
            menu.Add(SHUTDOWN);
            menu.Add(TIME_WORK);
            menu.Add(USAGE_PC);

            var generatedMenu = GenerateMenu(1, menu, MAIN_MEHU); 
        
            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "Главное меню",
                replyMarkup: generatedMenu,
                cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Генерирует меню для бота
        /// </summary>
        /// <param name="maxColumn">Максимальное количество столбцов</param>
        /// <param name="menu">Коллекция меню</param>
        /// <param name="mainMenu">Есть не пусто, добавляет главное меню</param>
        /// <returns></returns>
        public ReplyKeyboardMarkup GenerateMenu(int maxColumn, List<string> menu,string mainMenu)
        {
            List < List < KeyboardButton >> buttons = new();

            int row = 0;
            int currentElement = 0;

            foreach (var item in menu)
            {
                if(currentElement == 0)
                {
                    buttons.Add(new List<KeyboardButton>());
                    buttons[row].Add(new KeyboardButton(item));
                }
                else
                {
                    buttons[row].Add(new KeyboardButton(item));
                }

                currentElement++;

                if(currentElement >= maxColumn)
                {
                    currentElement = 0;
                    row++;
                }
            }

            if(!string.IsNullOrWhiteSpace(mainMenu))
            {
                if(currentElement != 0)
                {
                    row++;
                }
                buttons.Add(new List<KeyboardButton>());
                buttons[row].Add(mainMenu);
            }

            ReplyKeyboardMarkup replyKeyboardMarkup = new(buttons)
            {
                ResizeKeyboard = true
            };

            return replyKeyboardMarkup;
        }
    }
}
