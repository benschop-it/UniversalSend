﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UniversalSend.Views;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.DataTransfer.ShareTarget;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers.Provider;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Diagnostics;
using System.Xml.Linq;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;
using Windows.Storage;
using UniversalSend.Models.Tasks;
using Windows.UI.Core;

namespace UniversalSend
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the single-instance application object. This is the first line of authoring code executed,
        /// and is logically equivalent to main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is normally launched by the end user.
        /// Other entry points will be used when the app is launched to open a specific file, etc.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        Frame rootFrame;
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;
                rootFrame.Navigated += RootFrame_Navigated;
                SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored, navigate to the first page,
                    // and configure the new page by passing required info as a navigation parameter
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }
        }

        private void RootFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (rootFrame != null && rootFrame.CanGoBack && rootFrame.Content is HistoryPage || rootFrame.Content is ExplorerPage)
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            }
            else
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            }
        }

        private void App_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (rootFrame != null && rootFrame.CanGoBack && rootFrame.Content is HistoryPage || rootFrame.Content is ExplorerPage)
            {
                e.Handled = true;
                rootFrame.GoBack();
            }
        }

        /// <summary>
        /// Invoked when navigation to a specific page fails
        /// </summary>
        ///<param name="sender">The frame that failed navigation</param>
        ///<param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when the application execution is being suspended.
        /// The application state should be saved without knowing whether
        /// the application will be terminated or resumed later.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            // TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        protected override void OnFileOpenPickerActivated(FileOpenPickerActivatedEventArgs args)
        {
            FileOpenPickerUI UI = args.FileOpenPickerUI;
            Frame f = Window.Current.Content as Frame;
            if (f == null)
            {
                f = new Frame();
                Window.Current.Content = f;
            }

            f.Navigate(typeof(ExplorerPage), UI);

            Window.Current.Activate();
        }

        protected override void OnFileSavePickerActivated(FileSavePickerActivatedEventArgs args)
        {
            FileSavePickerUI UI = args.FileSavePickerUI;
            Frame f = Window.Current.Content as Frame;
            if (f == null)
            {
                f = new Frame();
                Window.Current.Content = f;
            }

            f.Navigate(typeof(ExplorerPage), UI);

            Window.Current.Activate();
        }

        protected override void OnShareTargetActivated(ShareTargetActivatedEventArgs args)
        {
            // Code to handle activation goes here. 
            var appState = args.PreviousExecutionState;

            var rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
            {
                rootFrame = new Frame();
                Window.Current.Content = rootFrame;
            }

            rootFrame.Navigate(typeof(MainPage), args.ShareOperation);

            //if (appState == ApplicationExecutionState.Running || appState == ApplicationExecutionState.Suspended || appState == ApplicationExecutionState.Terminated)
            //{
            //    rootFrame.Navigate(typeof(SendPage), args.ShareOperation);
            //}
            //else
            //{
            //    rootFrame.Navigate(typeof(MainPage), args.ShareOperation);
            //}
            Window.Current.Activate();
        }
    }
}
