using Windows.ApplicationModel.Resources;

namespace UniversalSend.Models {

    public class LocalizeManager {

        #region Public Methods

        public static string GetLocalizedString(string uid) {
            var resourceLoader = ResourceLoader.GetForCurrentView();
            var currentLanguage = resourceLoader.GetString("CurrentLanguage");
            return resourceLoader.GetString(uid);
        }

        #endregion Public Methods
    }
}