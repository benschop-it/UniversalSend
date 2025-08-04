using Windows.UI.Core;

namespace UniversalSend.Models.Interfaces {
    public interface IDispatcherProvider {
        CoreDispatcher Dispatcher { get; }

        void Initialize(CoreDispatcher dispatcher);
    }
}