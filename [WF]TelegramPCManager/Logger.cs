using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _WF_TelegramPCManager
{
    /// <summary>
    /// Класс для работы с логами
    /// </summary>
    class Logger
    {
        /// <summary>
        /// Запись в лог файл асинхронно
        /// </summary>
        /// <param name="msg">текст сообщения</param>
        public async static Task WriteLog(string msg)
        {
            string path = $"log-{DateTime.Now.ToShortDateString()}.txt";

            // добавление в файл
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                await writer.WriteLineAsync($"{msg}");
            }
        }
    }
}
