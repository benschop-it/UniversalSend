using Microsoft.Extensions.DependencyInjection;
using System;
using UniversalSend.Interfaces;
using UniversalSend.Misc;
using UniversalSend.Models;
using UniversalSend.Models.Common;
using UniversalSend.Models.Data;
using UniversalSend.Models.Helpers;
using UniversalSend.Models.HttpData;
using UniversalSend.Models.Interfaces;
using UniversalSend.Models.Managers;
using UniversalSend.Models.Tasks;
using UniversalSend.Services;
using UniversalSend.Services.HttpMessage.ServerRequestParsers;
using UniversalSend.Services.Interfaces;
using UniversalSend.Views;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Services.Maps;
using Windows.Storage.Pickers.Provider;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace UniversalSend {

    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application {

        #region Private Fields

        private Frame _rootFrame;
        public static IServiceProvider Services { get; private set; }

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Initializes the single-instance application object. This is the first line of authoring code executed,
        /// and is logically equivalent to main() or WinMain().
        /// </summary>
        public App() {
            InitializeComponent();
            Suspending += OnSuspending;
            LogManager.SetLogFactory(new DebugLogFactory());
            ConfigureServices();
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override void OnFileOpenPickerActivated(FileOpenPickerActivatedEventArgs args) {
            FileOpenPickerUI UI = args.FileOpenPickerUI;
            Frame f = Window.Current.Content as Frame;
            if (f == null) {
                f = new Frame();
                Window.Current.Content = f;
            }

            f.Navigate(typeof(ExplorerPage), UI);

            Window.Current.Activate();
        }

        protected override void OnFileSavePickerActivated(FileSavePickerActivatedEventArgs args) {
            FileSavePickerUI UI = args.FileSavePickerUI;
            Frame f = Window.Current.Content as Frame;
            if (f == null) {
                f = new Frame();
                Window.Current.Content = f;
            }

            f.Navigate(typeof(ExplorerPage), UI);

            Window.Current.Activate();
        }

        /// <summary>
        /// Invoked when the application is normally launched by the end user.
        /// Other entry points will be used when the app is launched to open a specific file, etc.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e) {
            _rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the window already has content,
            // just ensure that the window is active
            if (_rootFrame == null) {
                // Create a Frame to act as the navigation context and navigate to the first page
                _rootFrame = new Frame();

                _rootFrame.NavigationFailed += OnNavigationFailed;
                _rootFrame.Navigated += RootFrame_Navigated;
                SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated) {
                    // TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = _rootFrame;
            }

            if (e.PrelaunchActivated == false) {
                if (_rootFrame.Content == null) {
                    // When the navigation stack isn't restored, navigate to the first page,
                    // and configure the new page by passing required info as a navigation parameter
                    _rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }
        }

        protected override void OnShareTargetActivated(ShareTargetActivatedEventArgs args) {
            // Code to handle activation goes here.
            var appState = args.PreviousExecutionState;

            var rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null) {
                rootFrame = new Frame();
                Window.Current.Content = rootFrame;
            }

            rootFrame.Navigate(typeof(MainPage), args.ShareOperation);
            Window.Current.Activate();
        }

        #endregion Protected Methods

        #region Private Methods

        private void ConfigureServices() {
            var services = new ServiceCollection();

            // Register your services
            services.AddSingleton<IDeviceManager, DeviceManager>();
            services.AddSingleton<IFavoriteManager, FavoriteManager>();
            services.AddSingleton<IHistoryManager, HistoryManager>();
            services.AddSingleton<IInfoDataManager, InfoDataManager>();
            services.AddSingleton<INetworkHelper, NetworkHelper>();
            services.AddSingleton<IReceiveManager, ReceiveManager>();
            services.AddSingleton<IReceiveTaskManager, ReceiveTaskManager>();
            services.AddSingleton<IRegister, Register>();
            services.AddSingleton<IRegisterDataManager, RegisterDataManager>();
            services.AddSingleton<IRegisterRequestDataManager, RegisterRequestDataManager>();
            services.AddSingleton<IRegisterResponseDataManager, RegisterResponseDataManager>();
            services.AddSingleton<ISendManager, SendManager>();
            services.AddSingleton<ISendTaskManager, SendTaskManager>();
            services.AddSingleton<ISettings, Settings>();
            services.AddSingleton<IStorageHelper, StorageHelper>();
            services.AddSingleton<ISystemHelper, SystemHelper>();
            services.AddSingleton<ITokenFactory, TokenFactory>();
            services.AddSingleton<IUniversalSendFileManager, UniversalSendFileManager>();
            services.AddSingleton<IUIManager, UIManager>();
            services.AddSingleton<IOperationFunctions, OperationFunctions>();
            services.AddSingleton<ISendRequestDataManager, SendRequestDataManager>();
            services.AddSingleton<IHttpRequestParser, HttpRequestParser>();
            services.AddSingleton<IConfiguration, Configuration>();
            services.AddSingleton<IEncodingCache, EncodingCache>();

            services.AddTransient<IContentDialogManager, ContentDialogManager>();
            services.AddTransient<IDevice, Device>();
            services.AddTransient<IFavorite, Favorite>();
            services.AddTransient<IFileRequestData, FileRequestData>();
            services.AddTransient<IHistory, History>();
            services.AddTransient<IReceiveTask, ReceiveTask>();
            services.AddTransient<IRegisterData, RegisterData>();
            services.AddTransient<IRegisterRequestData, RegisterRequestData>();
            services.AddTransient<IRegisterResponseData, RegisterResponseData>();
            services.AddTransient<ISendTask, SendTask>();
            services.AddTransient<IServiceHttpServer, ServiceHttpServer>();
            services.AddTransient<IUniversalSendFile, UniversalSendFile>();
            services.AddTransient<ISendRequestData, SendRequestData>();

            Services = services.BuildServiceProvider();
        }

        private void App_BackRequested(object sender, BackRequestedEventArgs e) {
            if (_rootFrame != null && _rootFrame.CanGoBack && _rootFrame.Content is HistoryPage || _rootFrame.Content is ExplorerPage) {
                e.Handled = true;
                _rootFrame.GoBack();
            }
        }

        /// <summary>
        /// Invoked when navigation to a specific page fails
        /// </summary>
        ///<param name="sender">The frame that failed navigation</param>
        ///<param name="e">Details about the navigation failure</param>
        private void OnNavigationFailed(object sender, NavigationFailedEventArgs e) {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when the application execution is being suspended.
        /// The application state should be saved without knowing whether
        /// the application will be terminated or resumed later.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e) {
            var deferral = e.SuspendingOperation.GetDeferral();
            // TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        private void RootFrame_Navigated(object sender, NavigationEventArgs e) {
            if (_rootFrame != null && _rootFrame.CanGoBack && _rootFrame.Content is HistoryPage || _rootFrame.Content is ExplorerPage) {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            } else {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            }
        }

        #endregion Private Methods
    }
}