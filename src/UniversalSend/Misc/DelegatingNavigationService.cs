using System;
using UniversalSend.Interfaces;
using Windows.UI.Xaml.Controls;

namespace UniversalSend.Misc {

    public class DelegatingNavigationService : INavigationService {

        #region Private Fields

        private INavigationService _inner;

        #endregion Private Fields

        #region Public Methods

        public void GoBack() {
            if (_inner == null) {
                throw new InvalidOperationException("NavigationService is not initialized.");
            }

            _inner.GoBack();
        }

        public void Initialize(Frame frame) {
            _inner = new FrameNavigationService(frame);
        }

        public void NavigateTo(Type pageType, object parameter = null) {
            if (_inner == null) {
                throw new InvalidOperationException("NavigationService is not initialized.");
            }

            _inner.NavigateTo(pageType, parameter);
        }

        public void NavigateTo<TPage>(object parameter = null) {
            if (_inner == null) {
                throw new InvalidOperationException("NavigationService is not initialized.");
            }

            _inner.NavigateTo(typeof(TPage), parameter);
        }


        #endregion Public Methods
    }
}