namespace UniversalSend.Models.Interfaces {
    public interface ISettings {
        object GetSettingContent(string key);
        string GetSettingContentAsString(string key);
        void InitUserSettings();
        bool SetInitSetting(string key, object value);
        bool SetSetting(string key, object value);
    }
}