
namespace _WF_TelegramPCManager
{
    partial class Main
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.Tb_Token = new System.Windows.Forms.TextBox();
            this.Lb_Token = new System.Windows.Forms.Label();
            this.Gb_Logs = new System.Windows.Forms.GroupBox();
            this.Rc_Logs = new System.Windows.Forms.RichTextBox();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.Bt_Start = new System.Windows.Forms.Button();
            this.Bt_Stop = new System.Windows.Forms.Button();
            this.Lb_BotStatus = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Bt_AddUser = new System.Windows.Forms.Button();
            this.Tb_AddUser = new System.Windows.Forms.TextBox();
            this.Lb_Users = new System.Windows.Forms.ListBox();
            this.Cb_Tray = new System.Windows.Forms.CheckBox();
            this.Pb_Status = new System.Windows.Forms.PictureBox();
            this.CtMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.Cb_Notify = new System.Windows.Forms.CheckBox();
            this.Gb_Logs.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pb_Status)).BeginInit();
            this.SuspendLayout();
            // 
            // Tb_Token
            // 
            this.Tb_Token.Location = new System.Drawing.Point(135, 12);
            this.Tb_Token.Name = "Tb_Token";
            this.Tb_Token.Size = new System.Drawing.Size(341, 23);
            this.Tb_Token.TabIndex = 0;
            // 
            // Lb_Token
            // 
            this.Lb_Token.AutoSize = true;
            this.Lb_Token.Location = new System.Drawing.Point(13, 16);
            this.Lb_Token.Name = "Lb_Token";
            this.Lb_Token.Size = new System.Drawing.Size(89, 15);
            this.Lb_Token.TabIndex = 1;
            this.Lb_Token.Text = "Telegram Token";
            // 
            // Gb_Logs
            // 
            this.Gb_Logs.Controls.Add(this.Rc_Logs);
            this.Gb_Logs.Location = new System.Drawing.Point(12, 99);
            this.Gb_Logs.Name = "Gb_Logs";
            this.Gb_Logs.Size = new System.Drawing.Size(464, 120);
            this.Gb_Logs.TabIndex = 2;
            this.Gb_Logs.TabStop = false;
            this.Gb_Logs.Text = "Логи";
            // 
            // Rc_Logs
            // 
            this.Rc_Logs.Location = new System.Drawing.Point(7, 22);
            this.Rc_Logs.Name = "Rc_Logs";
            this.Rc_Logs.Size = new System.Drawing.Size(451, 92);
            this.Rc_Logs.TabIndex = 0;
            this.Rc_Logs.Text = "";
            // 
            // notifyIcon
            // 
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "notifyIcon1";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.OnMouseDoubleClick_Notify);
            // 
            // Bt_Start
            // 
            this.Bt_Start.Location = new System.Drawing.Point(135, 45);
            this.Bt_Start.Name = "Bt_Start";
            this.Bt_Start.Size = new System.Drawing.Size(170, 23);
            this.Bt_Start.TabIndex = 3;
            this.Bt_Start.Text = "Запустить бота";
            this.Bt_Start.UseVisualStyleBackColor = true;
            this.Bt_Start.Click += new System.EventHandler(this.OnClick_BtStart);
            // 
            // Bt_Stop
            // 
            this.Bt_Stop.Location = new System.Drawing.Point(311, 45);
            this.Bt_Stop.Name = "Bt_Stop";
            this.Bt_Stop.Size = new System.Drawing.Size(159, 23);
            this.Bt_Stop.TabIndex = 4;
            this.Bt_Stop.Text = "Остановить бота";
            this.Bt_Stop.UseVisualStyleBackColor = true;
            this.Bt_Stop.Click += new System.EventHandler(this.OnClick_BtStop);
            // 
            // Lb_BotStatus
            // 
            this.Lb_BotStatus.AutoSize = true;
            this.Lb_BotStatus.Location = new System.Drawing.Point(13, 74);
            this.Lb_BotStatus.Name = "Lb_BotStatus";
            this.Lb_BotStatus.Size = new System.Drawing.Size(74, 15);
            this.Lb_BotStatus.TabIndex = 5;
            this.Lb_BotStatus.Text = "Статус бота:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Bt_AddUser);
            this.groupBox1.Controls.Add(this.Tb_AddUser);
            this.groupBox1.Controls.Add(this.Lb_Users);
            this.groupBox1.Location = new System.Drawing.Point(482, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(169, 207);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Доступ к боту";
            // 
            // Bt_AddUser
            // 
            this.Bt_AddUser.Location = new System.Drawing.Point(138, 25);
            this.Bt_AddUser.Name = "Bt_AddUser";
            this.Bt_AddUser.Size = new System.Drawing.Size(25, 23);
            this.Bt_AddUser.TabIndex = 2;
            this.Bt_AddUser.Text = "+";
            this.Bt_AddUser.UseVisualStyleBackColor = true;
            this.Bt_AddUser.Click += new System.EventHandler(this.OnClick_AddUser);
            // 
            // Tb_AddUser
            // 
            this.Tb_AddUser.Location = new System.Drawing.Point(9, 25);
            this.Tb_AddUser.Name = "Tb_AddUser";
            this.Tb_AddUser.PlaceholderText = "Id Telegram User";
            this.Tb_AddUser.Size = new System.Drawing.Size(123, 23);
            this.Tb_AddUser.TabIndex = 1;
            this.Tb_AddUser.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress_AddUser);
            // 
            // Lb_Users
            // 
            this.Lb_Users.FormattingEnabled = true;
            this.Lb_Users.ItemHeight = 15;
            this.Lb_Users.Location = new System.Drawing.Point(6, 63);
            this.Lb_Users.Name = "Lb_Users";
            this.Lb_Users.Size = new System.Drawing.Size(157, 139);
            this.Lb_Users.TabIndex = 0;
            this.Lb_Users.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown_Users);
            // 
            // Cb_Tray
            // 
            this.Cb_Tray.AutoSize = true;
            this.Cb_Tray.Location = new System.Drawing.Point(311, 74);
            this.Cb_Tray.Name = "Cb_Tray";
            this.Cb_Tray.Size = new System.Drawing.Size(130, 19);
            this.Cb_Tray.TabIndex = 8;
            this.Cb_Tray.Text = "В трей при запуске";
            this.Cb_Tray.UseVisualStyleBackColor = true;
            this.Cb_Tray.CheckedChanged += new System.EventHandler(this.OnCheckedChange_Tray);
            // 
            // Pb_Status
            // 
            this.Pb_Status.BackgroundImage = global::_WF_TelegramPCManager.Properties.Resources.no;
            this.Pb_Status.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Pb_Status.Location = new System.Drawing.Point(93, 74);
            this.Pb_Status.Name = "Pb_Status";
            this.Pb_Status.Size = new System.Drawing.Size(16, 16);
            this.Pb_Status.TabIndex = 9;
            this.Pb_Status.TabStop = false;
            // 
            // CtMenu
            // 
            this.CtMenu.Name = "contextMenuStrip1";
            this.CtMenu.Size = new System.Drawing.Size(61, 4);
            // 
            // Cb_Notify
            // 
            this.Cb_Notify.AutoSize = true;
            this.Cb_Notify.Location = new System.Drawing.Point(135, 74);
            this.Cb_Notify.Name = "Cb_Notify";
            this.Cb_Notify.Size = new System.Drawing.Size(157, 19);
            this.Cb_Notify.TabIndex = 10;
            this.Cb_Notify.Text = "Включить уведомления";
            this.Cb_Notify.UseVisualStyleBackColor = true;
            this.Cb_Notify.CheckedChanged += new System.EventHandler(this.OnCheckedChange_Notify);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(657, 226);
            this.Controls.Add(this.Cb_Tray);
            this.Controls.Add(this.Cb_Notify);
            this.Controls.Add(this.Pb_Status);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.Lb_BotStatus);
            this.Controls.Add(this.Bt_Stop);
            this.Controls.Add(this.Bt_Start);
            this.Controls.Add(this.Gb_Logs);
            this.Controls.Add(this.Lb_Token);
            this.Controls.Add(this.Tb_Token);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Main";
            this.Text = "Telegram bot - управление компьютером";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Main_FormClosed);
            this.Load += new System.EventHandler(this.Main_Load);
            this.Resize += new System.EventHandler(this.Main_Resize);
            this.Gb_Logs.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pb_Status)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Tb_Token;
        private System.Windows.Forms.Label Lb_Token;
        private System.Windows.Forms.GroupBox Gb_Logs;
        private System.Windows.Forms.RichTextBox Rc_Logs;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.Button Bt_Start;
        private System.Windows.Forms.Button Bt_Stop;
        private System.Windows.Forms.Label Lb_BotStatus;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox Lb_Users;
        private System.Windows.Forms.Button Bt_AddUser;
        private System.Windows.Forms.TextBox Tb_AddUser;
        private System.Windows.Forms.CheckBox Cb_Tray;
        private System.Windows.Forms.PictureBox Pb_Status;
        private System.Windows.Forms.ContextMenuStrip CtMenu;
        private System.Windows.Forms.CheckBox Cb_Notify;
    }
}

