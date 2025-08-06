using System;
using UniversalSend.Interfaces;
using Windows.UI.Xaml.Controls;

namespace UniversalSend.Misc {

    public class FrameNavigationService : INavigationService {

        #region Private Fields

        private readonly Frame _frame;

        #endregion Private Fields

        #region Public Constructors

        public FrameNavigationService(Frame frame) {
            _frame = frame ?? throw new ArgumentNullException(nameof(frame));
        }

        #endregion Public Constructors

        #region Public Methods

        public void GoBack() {
            if (_frame.CanGoBack) {
                _frame.GoBack();
            }
        }

        public void NavigateTo(Type pageType, object parameter = null) {
            _frame.Navigate(pageType, parameter);
        }

        public void NavigateTo<TPage>(object parameter = null) {
            NavigateTo(typeof(TPage), parameter);
        }

        #endregion Public Methods
    }
}