using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UniversalSend.Models;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace UniversalSend.Controls.SettingControls
{
    public sealed partial class ComboSettingsControl : UserControl
    {
        string Key { get; set; }
        Dictionary<int,string> SelectionDisplayName { get; set; }//显示名称，与枚举中的常数对应

        Type EnumType { get; set; }
        public ComboSettingsControl(string key,Type enumType, Dictionary<int, string> selectionDisplayName)
        {
            this.InitializeComponent();
            this.Key = key;
            this.SelectionDisplayName = selectionDisplayName;
            this.EnumType = enumType;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            string[] names = Enum.GetNames(EnumType);
            Array valueArray = Enum.GetValues(EnumType);
            List<SelectionItem> items = new List<SelectionItem>();
            for (int i = 0; i < names.Length; i++)
            {
                string displayName;
                int num = Convert.ToInt32(valueArray.GetValue(i));
                if (SelectionDisplayName.TryGetValue(num, out displayName))
                    items.Add(new SelectionItem(num, displayName, names[i]));
                else
                    items.Add(new SelectionItem(num, names[i], names[i]));

            }
            MainComboBox.ItemsSource = items;
            string setting = Settings.GetSettingContentAsString(Key);
            if (!string.IsNullOrEmpty(setting))
            {
                MainComboBox.SelectedIndex = items.FindIndex(x=>x.Name == setting);
            }
            else
            {
                MainComboBox.PlaceholderText = "不可用";
                MainComboBox.IsEnabled = false;
            }
        }

        private void MainComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Settings.SetSetting(Key, Enum.GetName(EnumType, ((SelectionItem)MainComboBox.SelectedItem).ConstNumber));
        }

        class SelectionItem
        {
            public SelectionItem(int constNumber, string displayName, string Name)
            {
                this.ConstNumber = constNumber;
                this.DisplayName = displayName;
                this.Name = Name;
            }

            public int ConstNumber { get; set; }
            public string DisplayName { get; set; }
            public string Name { get; set; }
        }
    }

    
}
