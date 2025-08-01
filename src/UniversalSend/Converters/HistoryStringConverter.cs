using System;
using UniversalSend.Misc;
using UniversalSend.Models.Interfaces;
using Windows.UI.Xaml.Data;

namespace UniversalSend.Converters {
    public class HistoryStringConverter : IValueConverter {

        #region Public Methods

        public object Convert(object value, Type targetType, object parameter, string language) {
            if (value is IHistory) {
                IHistory history = (IHistory)value;
                string deviceAlias = "Unknown Device";
                if (history.Device != null) {
                    deviceAlias = history.Device.Alias;
                }
                return $"{history.DateTime.ToString()} - {StringHelper.GetByteUnit(history.File.Size)} - {deviceAlias}";
            }
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }

        #endregion Public Methods
    }
}