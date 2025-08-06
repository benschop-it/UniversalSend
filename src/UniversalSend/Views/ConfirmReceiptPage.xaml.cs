using Microsoft.Extensions.DependencyInjection;
using UniversalSend.Interfaces;
using UniversalSend.Misc;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace UniversalSend.Views {

    public sealed partial class ConfirmReceiptPage : Page {

        #region Private Fields

        private readonly IConfirmReceiptViewModel _viewModel;

        #endregion Private Fields

        #region Public Constructors

        public ConfirmReceiptPage() {
            InitializeComponent();

            _viewModel = App.Services.GetRequiredService<IConfirmReceiptViewModel>();
            DataContext = _viewModel;
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            var parameter = e.Parameter as ConfirmReceiptPageParameter;
            _viewModel.Initialize(parameter);
        }

        #endregion Protected Methods
    }
}