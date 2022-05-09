using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _WF_TelegramPCManager
{
    public static class Computer
    {
        static PerformanceCounter cpuCounter = new PerformanceCounter("Processor Information", "% Processor Time", "_Total");
        //Выключение компьютера
        public static void ShutDown()
        {
            Process.Start(@"C:\Windows\System32\shutdown.exe", "/s /f /t 10");
        }

        /// <summary>
        /// Рабочее время компьютера
        /// </summary>
        public static string GetWorkTime()
        {
            String result = "Время работы компьютера " + Environment.NewLine;
            result += Convert.ToString(Environment.TickCount / 86400000) + " дней, " + Environment.NewLine;
            result += Convert.ToString(Environment.TickCount / 3600000 % 24) + " часов, " + Environment.NewLine;
            result += Convert.ToString(Environment.TickCount / 120000 % 60) + " минут, " + Environment.NewLine;
            result += Convert.ToString(Environment.TickCount / 1000 % 60) + " секунд." + Environment.NewLine;
            return result;
        }
        /// <summary>
        /// Информация об оперативной памяти
        /// </summary>
        public static string GetMemoryUsage()
        {
            var pcInfo = new Microsoft.VisualBasic.Devices.ComputerInfo();
            var totalMemory = pcInfo.TotalPhysicalMemory;
            var avalibleMemory = pcInfo.AvailablePhysicalMemory;
            var usageMemory = pcInfo.TotalPhysicalMemory - pcInfo.AvailablePhysicalMemory;

            return $"Оперативная память: " + Environment.NewLine +
                    "Всего: " + GetGBOfMemory(totalMemory) + Environment.NewLine +
                    "Занято: " + GetGBOfMemory(usageMemory) + Environment.NewLine +
                    "Свободно: " + GetGBOfMemory(avalibleMemory) + Environment.NewLine;

        }
        /// <summary>
        /// Преобразовывает память в гигабайты
        /// </summary>
        /// <param name="memory"></param>
        private static string GetGBOfMemory(ulong memory)
        {
            return memory / (1024 * 1024 * 1024) + " Гб";
        }
        /// <summary>
        /// Загрузка процессора
        /// </summary>
        public async static Task<string> GetCPULoad()
        {
            cpuCounter.NextValue();
            await Task.Delay(1000);
            return $"Процессор загружен на {(int)cpuCounter.NextValue()} %" + Environment.NewLine;
        }
    }
}
