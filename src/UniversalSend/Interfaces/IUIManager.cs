using Windows.UI.Xaml;

namespace UniversalSend.Interfaces {
    public interface IUIManager {
        Thickness RootElementMargin { get; set; }
        Thickness RootElementMarginWithoutTop { get; set; }

        void InitRootElementMargin();
    }
}