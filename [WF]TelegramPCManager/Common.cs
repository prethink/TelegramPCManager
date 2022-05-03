using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _WF_TelegramPCManager
{
    /// <summary>
    /// Класс упрощенной работы
    /// </summary>
    public class Common
    {
        //Конфигурационный файл по умолчанию
        private static Configuration configFile = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);

        /// <summary>
        /// Добавляет или обновляет поле в конфигурационном файле
        /// </summary>
        /// <param name="key">Названия поля</param>
        /// <param name="value">Значение</param>
        public static void AddOrUpdateAppSettings(string key, string value)
        {
            try
            {
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }
        }

        /// <summary>
        /// Получает значение по названию поля
        /// </summary>
        /// <param name="key">Названия поля</param>
        /// <returns>Значение поля или нечего</returns>
        public static string GetValueFromConfig(string key)
        {
            return configFile.AppSettings.Settings[key]?.Value ?? "";
        }

        /// <summary>
        /// Получает список id пользовалей которые могут работать с телеграм ботом
        /// </summary>
        /// <returns>Список id</returns>
        public static List<long> GetUsersId()
        {
            return JsonConvert.DeserializeObject<List<long>>(Common.GetValueFromConfig("users")) ?? new List<long>();
        }

        /// <summary>
        /// Обновляет список пользователей в конфигурационном файле
        /// </summary>
        /// <param name="users">Коллекция пользователей</param>
        public static void UpdateUsers(List<long> users) 
        {
            var serUsers = JsonConvert.SerializeObject(users);
            AddOrUpdateAppSettings("users", serUsers);
        }

        /// <summary>
        /// Получает значение переменной tray
        /// </summary>
        /// <returns>true/false</returns>
        public static bool GetTrayStatus()
        {
            bool status = false;
            bool.TryParse(GetValueFromConfig("TrayStatus"),out status);
            return status;
        }

        /// <summary>
        /// Записывает значение с переменной tray
        /// </summary>
        /// <param name="value">true/false</param>
        public static void SetTrayStatus(bool value)
        {
            Common.AddOrUpdateAppSettings("TrayStatus", value.ToString());
        }

        /// <summary>
        /// Получает значение переменной notify
        /// </summary>
        /// <returns>true/false</returns>
        public static bool GetNotifyStatus()
        {
            bool status = false;
            bool.TryParse(GetValueFromConfig("NotifyStatus"), out status);
            return status;
        }

        /// <summary>
        /// Записывает значение с переменной notify
        /// </summary>
        /// <param name="value">true/false</param>
        public static void SetNotifyStatus(bool value)
        {
            Common.AddOrUpdateAppSettings("NotifyStatus", value.ToString());
        }

        /// <summary>
        /// Проверка доступа к боту
        /// </summary>
        /// <param name="idUser">Идентификатор пользователя</param>
        /// <returns></returns>
        public static bool HasAccess(long idUser)
        {
            foreach (var item in GetUsersId())
            {
                if (item == idUser)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
