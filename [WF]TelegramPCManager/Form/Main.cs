using _WF_TelegramPCManager.Properties;
using System;
using System.Configuration;
using System.Linq;
using System.Windows.Forms;

namespace _WF_TelegramPCManager
{
    public partial class Main : Form
    {
        private Telegram telegram;
        public Main()
        {
            InitializeComponent();
            InitializeTelegram();
            InitializeNotify();
            LoadSettings();
        }

        #region Инициализация
        /// <summary>
        /// Инициализация настроек телеграм бота
        /// </summary>
        public void InitializeTelegram()
        {
            telegram = new Telegram(this);
            //Подписка на события сообщений
            telegram.OnMessage += OnMessage_Telegram;
            //Подписка на события изменения статуса
            telegram.OnChangeStatus += OnChangeStatus_Telegram;
        }
        /// <summary>
        /// Инициализация настроек уведомления
        /// </summary>
        public void InitializeNotify()
        {
            notifyIcon.Text = "Telegram Bot - управление компьютером";
            notifyIcon.Visible = false;

            CtMenu.Items.Add("Запустить бота");
            CtMenu.Items.Add("Остановить бота");
            CtMenu.Items.Add("Выход");

            CtMenu.Items[0].Click += OnClick_CtStartBot;
            CtMenu.Items[1].Click += OnClick_CtStopBot;
            CtMenu.Items[2].Click += OnClick_CtExit;

            notifyIcon.ContextMenuStrip = CtMenu;
        }
        /// <summary>
        /// Загрузка настроек
        /// </summary>
        public void LoadSettings()
        {
            Tb_Token.Text = Common.GetValueFromConfig("BotToken");
            Cb_Tray.Checked = Common.GetTrayStatus();
            Cb_Notify.Checked = Common.GetNotifyStatus();

            //Обновление списка пользователей для работы с ботом
            ListUsersUpdate();

            //Если токен не пустой, попытка запустить бота
            if (!string.IsNullOrWhiteSpace(Tb_Token.Text))
            {
                telegram.Start(Tb_Token.Text);
                Bt_Start.Enabled = false;
            }
        }
        #endregion

        #region Events

        #region Clicks
        /// <summary>
        /// Кнопка на форме для запуска бота
        /// </summary>
        private void OnClick_BtStart(object sender, EventArgs e)
        {
            Common.AddOrUpdateAppSettings("BotToken", Tb_Token.Text);
            telegram.Start(Tb_Token.Text);
            Bt_Start.Enabled = false;
        }

        /// <summary>
        /// Кнопка на форме для остановки бота
        /// </summary>
        private void OnClick_BtStop(object sender, EventArgs e)
        {
            telegram.Stop();
            Bt_Stop.Enabled = false;
        }

        /// <summary>
        /// Клик для показа формы
        /// </summary>
        private void OnMouseDoubleClick_Notify(object sender, MouseEventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon.Visible = false;
        }

        /// <summary>
        /// Кнопка контекстное меню уведомления дла запуска бота
        /// </summary>
        private void OnClick_CtStartBot(object sender, EventArgs e)
        {
            OnClick_BtStart(sender, e);
        }

        /// <summary>
        /// Кнопка контекстное меню уведомления дла остановки бота
        /// </summary>
        private void OnClick_CtStopBot(object sender, EventArgs e)
        {
            OnClick_BtStop(sender, e);
        }

        /// <summary>
        /// Кнопка контекстное меню уведомления дла закрытия программы
        /// </summary>
        private void OnClick_CtExit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Добавляет нового пользователя для доступа к телеграм боту
        /// </summary>
        private void OnClick_AddUser(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Tb_AddUser.Text))
            {
                MessageBox.Show("Нужно ввести id пользователя из telegram");
                return;
            }

            var users = Common.GetUsersId();
            users.Add(long.Parse(Tb_AddUser.Text));
            Common.UpdateUsers(users.Distinct().ToList());

            ListUsersUpdate();

            Tb_AddUser.Clear();
        }

        #endregion
        #region OtherEvents
        /// <summary>
        /// Удаление выбранного пользователя из списка
        /// </summary>
        private void OnKeyDown_Users(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (Lb_Users.SelectedItem != null)
                {
                    var tempList = Common.GetUsersId();
                    tempList.RemoveAll(x => x == (long)Lb_Users.SelectedItem);
                    Common.UpdateUsers(tempList);
                    ListUsersUpdate();
                }
            }
        }

        /// <summary>
        /// Событие сообщение от класса telegram
        /// </summary>
        /// <param name="msg"></param>
        private void OnMessage_Telegram(string msg)
        {
            if (InvokeRequired)
            {
                Action action = () =>
                {
                    WriteLog(msg);
                };
            }
            else
            {
                WriteLog(msg);
            }
        }

        /// <summary>
        /// Событие изменения статуса работы бота
        /// </summary>
        /// <param name="isWork">Работает/не работает</param>
        private void OnChangeStatus_Telegram(bool isWork)
        {
            //Изменение изображение взависимости от работы бота
            Pb_Status.BackgroundImage = isWork ? Resources.check : Resources.no;

            //Изменение статуса кнопок
            Bt_Start.Enabled = !isWork;
            Bt_Stop.Enabled = isWork;

            // пункт 'Запустить бота'
            CtMenu.Items[0].Enabled = !isWork;
            // пункт 'Остановить бота'
            CtMenu.Items[1].Enabled = isWork;

            //Если бот заработал и установлен флаг 'Спрятать в трей'
            if (isWork && Common.GetTrayStatus())
            {
                HideWindow();
                NotifyMessage("Бот запущен");
            }
        }

        /// <summary>
        /// Изменение значения checkBox tray
        /// </summary>
        private void OnCheckedChange_Tray(object sender, EventArgs e)
        {
            Common.SetTrayStatus(Cb_Tray.Checked);
        }

        /// <summary>
        /// Изменение значения checkBox Notify
        /// </summary>
        private void OnCheckedChange_Notify(object sender, EventArgs e)
        {
            Common.SetNotifyStatus(Cb_Notify.Checked);
        }

        /// <summary>
        /// Позволяет вводить только цифры в textbox AddUser
        /// </summary>
        private void OnKeyPress_AddUser(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!Char.IsDigit(number) && number != (int)Keys.Back && number != (int)Keys.Delete)
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Событие изменения размера формы
        /// </summary>
        private void Main_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                Hide();
                notifyIcon.Visible = true;
            }
        }

        /// <summary>
        /// Загрузка формы
        /// </summary>
        private void Main_Load(object sender, EventArgs e)
        {
            OnChangeStatus_Telegram(telegram.IsWork);
        }

        /// <summary>
        /// Событие закрытие формы
        /// </summary>
        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            notifyIcon.Icon.Dispose();
            notifyIcon.Dispose();
        }

        #endregion

        #endregion

        /// <summary>
        /// Скрывает формы и оставляет значок в трее
        /// </summary>
        public void HideWindow()
        {
            this.WindowState = FormWindowState.Minimized;
            Hide();
            notifyIcon.Visible = true;
        }

        /// <summary>
        /// Запись логов
        /// </summary>
        /// <param name="msg">Сообщение</param>
        private async void WriteLog(string msg)
        {
            //Уведомление
            NotifyMessage(msg);
            //Форматированное сообщение вида Дата время: сообщение
            var formateMsg = $"{DateTime.Now}: " + msg;
            //Запись в лог файл
            await Logger.WriteLog(formateMsg);
            //Запись в лог поле
            Rc_Logs.Text += formateMsg + Environment.NewLine;
            Rc_Logs.SelectionStart = Rc_Logs.Text.Length;
            Rc_Logs.ScrollToCaret();
        }

        /// <summary>
        /// Вывод уведомления
        /// </summary>
        /// <param name="msg">Сообщение</param>
        private void NotifyMessage(string msg)
        {
            if (Common.GetNotifyStatus())
            {
                notifyIcon.BalloonTipText = msg;
                notifyIcon.ShowBalloonTip(1000);
            }
        }

        /// <summary>
        /// Обновление списка пользователей из конфиг файла
        /// </summary>
        private void ListUsersUpdate()
        {
            Lb_Users.Items.Clear();

            foreach (var item in Common.GetUsersId())
            {
                Lb_Users.Items.Add(item);
            }
        }
    }
}
