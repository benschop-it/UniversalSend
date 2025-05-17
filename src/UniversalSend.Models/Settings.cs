using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.Storage;
using static UniversalSend.Models.SystemHelper;

namespace UniversalSend.Models
{
    public class Settings
    {
        
        public const string Network_DeviceName = "Network_DeviceName";

        public const string Network_DeviceType = "Network_DeviceType";

        public const string Network_DeviceModel = "Network_DeviceModel";

        public const string Network_Port = "Network_Port";

        public const string Network_MulticaastAddress = "Network_MulticaastAddress";

        public const string Receive_SaveToFolder = "Receive_SaveToFolder";

        public const string Receive_Histories  = "Receive_Histories";

        public static ApplicationDataContainer UserSettings;

        public static void InitUserSettings()
        {
            UserSettings = ApplicationData.Current.LocalSettings;
            InitSettings();
        }

        private static void InitSettings()
        {
            //设备名称
            EasClientDeviceInformation deviceInfo = new EasClientDeviceInformation();
            SetInitSetting(Network_DeviceName,$"{deviceInfo.SystemProductName} (UWP)");

            //设备类型
            SystemHelper.DeviceFormFactorType deviceType = SystemHelper.GetDeviceFormFactorType();
            switch(deviceType)
            {
                case DeviceFormFactorType.Phone:
                    SetInitSetting(Network_DeviceType, $"mobile");
                    break;
                case DeviceFormFactorType.Desktop:
                case DeviceFormFactorType.SurfaceHub:
                case DeviceFormFactorType.Tablet:
                    SetInitSetting(Network_DeviceType, $"desktop");
                    break;
                case DeviceFormFactorType.IoT:
                    SetInitSetting(Network_DeviceType, $"headless");
                    break;
                case DeviceFormFactorType.Xbox:
                    SetInitSetting(Network_DeviceType, $"server");
                    break;
                default:
                    SetInitSetting(Network_DeviceType, $"web");
                    break;
            }
            //设备型号
            if(!string.IsNullOrEmpty(deviceInfo.SystemSku))
                SetInitSetting(Network_DeviceModel,deviceInfo.SystemSku);
            else
                SetInitSetting(Network_DeviceModel, deviceInfo.SystemProductName);
            //端口
            SetInitSetting(Network_Port, 53317);
            //广播地址
            SetInitSetting(Network_MulticaastAddress, "224.0.0.167");
        }

        public static bool SetSetting(string key,object value)
        {
            UserSettings.Values[key] = value;
            return true;
        }

        public static bool SetInitSetting(string key, object value)
        {
            if (UserSettings.Values.ContainsKey(key))
                return false;
            UserSettings.Values.Add(key,value);
            return true;
        }

        public static object GetSettingContent(string key)
        {
            object value;
            if (UserSettings.Values.TryGetValue(key, out value))
                return value;
            else
                return null;
        }

        public static string GetSettingContentAsString(string key)
        {
            object value;
            if (UserSettings.Values.TryGetValue(key, out value))
                return value.ToString();
            else
                return null;
        }
    }
}
