using System;

namespace UniversalSend.Interfaces {

    public interface INavigationService {

        #region Public Methods

        void GoBack();

        void NavigateTo(Type pageType, object parameter = null);

        void NavigateTo<TPage>(object parameter = null);

        #endregion Public Methods
    }
}