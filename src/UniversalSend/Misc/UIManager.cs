using Microsoft.Extensions.DependencyInjection;
using System;
using UniversalSend.Interfaces;
using UniversalSend.Models.Helpers;
using UniversalSend.Models.Interfaces;
using Windows.UI.Xaml;

namespace UniversalSend.Misc {

    internal class UIManager : IUIManager {

        private ISystemHelper _systemHelper;

        public UIManager(ISystemHelper systemHelper) {
            _systemHelper = systemHelper ?? throw new ArgumentNullException(nameof(systemHelper));
        }

        #region Public Properties

        public Thickness RootElementMargin { get; set; } = new Thickness(24);

        public Thickness RootElementMarginWithoutTop { get; set; } = new Thickness(24, 0, 24, 24);

        #endregion Public Properties

        #region Public Methods

        public void InitRootElementMargin() {
            if (_systemHelper.GetDeviceFormFactorType() == DeviceFormFactorType.Phone) {
                RootElementMargin = new Thickness(12);
                RootElementMarginWithoutTop = new Thickness(12, 0, 12, 12);
            }
        }

        #endregion Public Methods
    }
}