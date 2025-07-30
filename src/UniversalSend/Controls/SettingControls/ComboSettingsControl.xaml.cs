using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using UniversalSend.Models;
using UniversalSend.Models.Interfaces;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UniversalSend.Controls.SettingControls {

    public sealed partial class ComboSettingsControl : UserControl {
        private readonly ISettings _settings = App.Services.GetRequiredService<ISettings>();

        #region Public Constructors

        public ComboSettingsControl(string key, Type enumType, Dictionary<int, string> selectionDisplayName) {
            InitializeComponent();
            Key = key;
            SelectionDisplayName = selectionDisplayName;
            EnumType = enumType;
        }

        #endregion Public Constructors

        #region Private Properties

        private Type EnumType { get; set; }

        private string Key { get; set; }

        // Display name, corresponding to constants in the enum
        private Dictionary<int, string> SelectionDisplayName { get; set; }

        #endregion Private Properties

        #region Private Methods

        private void MainComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            _settings.SetSetting(Key, Enum.GetName(EnumType, ((SelectionItem)MainComboBox.SelectedItem).ConstNumber));
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) {
            string[] names = Enum.GetNames(EnumType);
            Array valueArray = Enum.GetValues(EnumType);
            List<SelectionItem> items = new List<SelectionItem>();

            for (int i = 0; i < names.Length; i++) {
                string displayName;
                int num = Convert.ToInt32(valueArray.GetValue(i));
                if (SelectionDisplayName.TryGetValue(num, out displayName))
                    items.Add(new SelectionItem(num, displayName, names[i]));
                else
                    items.Add(new SelectionItem(num, names[i], names[i]));
            }

            MainComboBox.ItemsSource = items;

            string setting = _settings.GetSettingContentAsString(Key);
            if (!string.IsNullOrEmpty(setting)) {
                MainComboBox.SelectedIndex = items.FindIndex(x => x.Name == setting);
            } else {
                MainComboBox.PlaceholderText = "Unavailable";
                MainComboBox.IsEnabled = false;
            }
        }

        #endregion Private Methods

        #region Private Classes

        private class SelectionItem {

            #region Public Constructors

            public SelectionItem(int constNumber, string displayName, string Name) {
                this.ConstNumber = constNumber;
                this.DisplayName = displayName;
                this.Name = Name;
            }

            #endregion Public Constructors

            #region Public Properties

            public int ConstNumber { get; set; }
            public string DisplayName { get; set; }
            public string Name { get; set; }

            #endregion Public Properties
        }

        #endregion Private Classes
    }
}