using System.Collections.Generic;
using Elite.Utilities;

namespace EliteGST
{
    internal static class Config
    {
        public static Dictionary<string, object> config = new Dictionary<string, object>();

        internal static string Salt => "nizlwr0hisax2brhtui89s8uprohria1az7tr237i@ripho1ri0us3ide";
        internal static string AppSalt => "{30A8228A-9E33-416E-B981-3DD3DAE4ECA8}";

        public static string GetDatabaseConnectionString()
        {
            string _host = GetStringValue("host", string.Empty);
            string _user = GetStringValue("user", string.Empty);
            string _password = GetStringValue("password", string.Empty);
            string _database = GetStringValue("database", string.Empty);
            return string.Format("server={0};uid={1};pwd={2};database={3}", _host, _user, _password, _database);
        }

        private static string GetValue(string key, string defaultVaue)
        {
            return ConfigManager.GetValue(key, defaultVaue);
        }

        public static void SetValue(string key, object value)
        {
            ConfigManager.SaveValue(key, value.ToString());
        }

        public static string GetStringValue(string key, string defaultValue, bool setIfNotAvailable = false)
        {
            string value = GetValue(key, null);
            if (setIfNotAvailable && value == null) SetValue(key, defaultValue);
            return value != null ? value : defaultValue;
        }

        public static int GetIntValue(string key, int defaultValue, bool setIfNotAvailable = false)
        {
            if (int.TryParse(GetStringValue(key, string.Empty, setIfNotAvailable), out int result))
            {
                return result;
            }
            return defaultValue;
        }

        public static bool GetBoolValue(string key, bool defaultValue, bool setIfNotAvailable = false)
        {
            if (bool.TryParse(GetStringValue(key, bool.FalseString, setIfNotAvailable), out bool result))
            {
                return result;
            }
            return defaultValue;
        }
    }
}
