using System;
using UniversalSend.Models.Interfaces;
using Windows.UI.Core;

namespace UniversalSend.Models.Misc {
    public sealed class DispatcherProvider : IDispatcherProvider {
        private CoreDispatcher _dispatcher;

        public CoreDispatcher Dispatcher =>
            _dispatcher ?? throw new InvalidOperationException("Dispatcher not initialized yet.");

        public void Initialize(CoreDispatcher dispatcher) {
            _dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
        }
    }
}
