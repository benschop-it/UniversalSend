﻿using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Windows.UI.Xaml.Data;

namespace UniversalSend.Converters {

    public class EnumDescriptionConverter : IValueConverter {

        #region Public Methods

        public object Convert(object value, Type targetType, object parameter, string language) {
            if (value is Enum enumValue) {
                MemberInfo[] memberInfo = value.GetType().GetMember(enumValue.ToString());

                if (memberInfo != null && memberInfo.Length > 0) {
                    // Try to get the Description attribute
                    var attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                    if (attrs != null && attrs.Count() > 0) {
                        // Return the value of the Description attribute
                        return ((DescriptionAttribute)attrs.ToList()[0]).Description;
                    }
                }
            }
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }

        #endregion Public Methods
    }
}