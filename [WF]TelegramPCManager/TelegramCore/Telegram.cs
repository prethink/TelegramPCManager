using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace _WF_TelegramPCManager
{
    public class Telegram
    {
        /// <summary>
        /// События
        /// </summary>
        public delegate void NotifyTelegram(string msg);
        public delegate void ChangeStatus(bool status);
        public event NotifyTelegram OnMessage;
        public event ChangeStatus OnChangeStatus;
        /// <summary>
        /// Команды
        /// </summary>
        private Commands commands;
        /// <summary>
        /// Токен для отключения бота
        /// </summary>
        private CancellationTokenSource cts;
        /// <summary>
        /// Телеграм клиент
        /// </summary>
        public TelegramBotClient telegram { get; private set; }
        /// <summary>
        /// Переменная работает или нет бот
        /// </summary>
        public bool IsWork { get; private set; }
        /// <summary>
        /// Ссылка на форму для вызова уведомлений из осноного потока
        /// </summary>
        private Main form;

        public Telegram(Main form)
        {
            this.form = form;
            commands = new Commands(this);
            OnChangeStatus?.Invoke(IsWork);
        }

        /// <summary>
        /// Запуск бота
        /// </summary>
        /// <param name="token">Токен бота</param>
        public async void Start(string token)
        {
            try
            {
                cts = new CancellationTokenSource();
                telegram = new TelegramBotClient(token);
                await ClearUpdates();

                var receiverOptions = new ReceiverOptions
                {
                    AllowedUpdates = { } 
                };

                telegram.StartReceiving(
                    HandleUpdateAsync,
                    HandleErrorAsync,
                    receiverOptions,
                    cancellationToken: cts.Token);

                var me = await telegram.GetMeAsync();

                OnMessage?.Invoke($"Бот начал работу @{me.Username}");
                IsWork = true;
            }
            catch(Exception ex)
            {
                OnMessage?.Invoke($"[TelegramBot] ошибка {ex.Message}");
            }
            finally
            {
                OnChangeStatus?.Invoke(IsWork);
            }
            
        }

        /// <summary>
        /// Остановка бота
        /// </summary>
        public void Stop()
        {
            try
            {
                cts.Cancel();
                IsWork = false;
                OnMessage?.Invoke($"Бот остановлен");
            }
            catch (Exception ex)
            {
                OnMessage?.Invoke($"[TelegramBot] ошибка {ex.Message}");
            }
            finally
            {
                OnChangeStatus?.Invoke(IsWork);
            }
        }

        /// <summary>
        /// Очищает все команды перед новым запуском бота
        /// </summary>
        public async Task ClearUpdates()
        {
            var update = await telegram.GetUpdatesAsync();
            foreach (var item in update)
            {
                var offset = item.Id + 1;
                await telegram.GetUpdatesAsync(offset);
            }
        }
        /// <summary>
        /// Обработчик бота
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="update"></param>
        /// <param name="cancellationToken"></param>
        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Only process Message updates: https://core.telegram.org/bots/api#message
            if (update.Type != UpdateType.Message)
                return;
            // Only process text messages
            if (update.Message!.Type != MessageType.Text)
                return;

            try
            {
                var chatId = update.Message.Chat.Id;
                var messageText = update.Message.Text;

                string messageData = $"Получено сообщение '{messageText}' от пользователя {update.Message.Chat.Username} c id {chatId} с данными {update.Message.Chat?.FirstName} {update.Message.Chat?.LastName}.";

                //Уведомление что пользователь отправил сообщение боту
                form.Invoke(new Action(() => OnMessage?.Invoke(messageData)));

                await commands.ExecuteCommand(messageText, botClient, update, cancellationToken);
            }
            catch (Exception ex)
            {
                form.Invoke(new Action(() => OnMessage?.Invoke($"[TelegramBot] ошибка: {ex.Message}")));
            }
        }

        /// <summary>
        /// Обработчик для Exceptions на стороне API бота
        /// </summary>
        Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            form.Invoke(new Action(() => OnMessage?.Invoke(ErrorMessage)));

            return Task.CompletedTask;
        }
    }
}
