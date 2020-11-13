namespace WanderGuideFrontend.Services
{
    // Helpers/Settings.cs
    using Plugin.Settings;
    using Plugin.Settings.Abstractions;
    public static class Settings
    {
        private static ISettings AppSettings => CrossSettings.Current;

        #region Setting Constants

        private const string LastUsernameSettingsKey = "last_username_key";
        private const string LastPasswordSettingsKey = "last_password_key";
        private const string RememberMeToggleKey = "remember_me_key";
        private const string AutologinToggleKey = "autologin_key";
        private static readonly string SettingsDefault = string.Empty;
        private static readonly bool RememberMeDefault = false;
        private static readonly bool AutologinDefault = false;
        #endregion


        public static string LastUsedUsername
        {
            get => AppSettings.GetValueOrDefault(LastUsernameSettingsKey, SettingsDefault);
            set => AppSettings.AddOrUpdateValue(LastUsernameSettingsKey, value);
        }
        public static string LastUsedPassword
        {
            get => AppSettings.GetValueOrDefault(LastPasswordSettingsKey, SettingsDefault);
            set => AppSettings.AddOrUpdateValue(LastPasswordSettingsKey, value);
        }
        public static bool RememberMe
        {
            get => AppSettings.GetValueOrDefault(RememberMeToggleKey, RememberMeDefault);
            set => AppSettings.AddOrUpdateValue(RememberMeToggleKey, value);
        }
        public static bool Autologin
        {
            get => AppSettings.GetValueOrDefault(AutologinToggleKey, AutologinDefault);
            set => AppSettings.AddOrUpdateValue(AutologinToggleKey, value);
        }
    }
}
