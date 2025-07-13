﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers.Provider;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using UniversalSend.Models;
using System.Diagnostics;
using UniversalSend.Controls.ContentDialogControls;
using Windows.UI;
using Windows.System;
using Windows.ApplicationModel.DataTransfer;

// https://go.microsoft.com/fwlink/?LinkId=234238 describes the "Blank Page" item template

namespace UniversalSend.Views
{
    /// <summary>
    /// Can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ExplorerPage : Page
    {
        public ExplorerPage()
        {
            this.InitializeComponent();
            string ViewMode = Settings.GetSettingContentAsString(Settings.ExplorerPage_ViewMode);
            if (ViewMode == "List")
                CurrentViewMode = ExplorerPage.ViewMode.List;
            else
                CurrentViewMode = ExplorerPage.ViewMode.Grid;
            UpdateViewMode();
        }

        ContentDialogManager ContentDialogManager { get; set; } = new ContentDialogManager();

        FileOpenPickerUI fileOpenPickerUI;

        FileSavePickerUI fileSavePickerUI;

        List<StorageFolder> folderStack = new List<StorageFolder>();

        List<string> AllowedFileTypes;

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            Settings.InitUserSettings();
            UpdateViewMode();
            if (e.Parameter is FileOpenPickerUI)
            {
                // Get parameter
                fileOpenPickerUI = e.Parameter as FileOpenPickerUI;
                // Get local file list
                AllowedFileTypes = fileOpenPickerUI.AllowedFileTypes.ToList();
            }
            else if(e.Parameter is FileSavePickerUI)
            {
                fileSavePickerUI = e.Parameter as FileSavePickerUI;
                AllowedFileTypes = fileSavePickerUI.AllowedFileTypes.ToList();
                fileSavePickerUI.TargetFileRequested += FileSavePickerUI_TargetFileRequested;
            }
            await UpdateViewAsync(await StorageHelper.GetReceiveStoageFolderAsync());
        }

        List<ViewStorageItem> viewItems = new List<ViewStorageItem>();

        async Task UpdateViewAsync(StorageFolder folder)
        {
            LoadingProgressBar.Visibility = Visibility.Visible;
            lvFiles.IsEnabled = false;
            UpdateFolderPathTextBlock();
            List<IStorageItem> items = (await folder.GetItemsAsync()).ToList();
            BackButton.IsEnabled = folderStack.Count > 0 ? true : false;

            viewItems = new List<ViewStorageItem>();
            EmptyFolderTip.Visibility = Visibility.Collapsed;
            if (AllowedFileTypes == null || AllowedFileTypes.Contains("*"))
                foreach (var item in items)
                {
                    viewItems.Add(new ViewStorageItem(item));
                }
            else
                foreach (var item in items)
                {
                    if (item is StorageFile && !AllowedFileTypes.Contains(((StorageFile)item).FileType.ToLower()))
                    {
                        continue;
                    }
                    viewItems.Add(new ViewStorageItem(item));
                }
            if (viewItems.Count == 0)
            {
                EmptyFolderTip.Visibility = Visibility.Visible;
            }

            await Task.Delay(100);
            lvFiles.ItemsSource = viewItems;
            lvFiles.IsEnabled = true;
            LoadingProgressBar.Visibility = Visibility.Collapsed;
        }

        void UpdateViewMode()
        {
            if (CurrentViewMode == ViewMode.List)
            {
                lvFiles.ItemTemplate = ListViewModeItemTemplate;
                lvFiles.ItemContainerStyle = ListViewModeListViewItemStyle;
                lvFiles.ItemsPanel = ListViewModeItemsPanelTemplate;
                CurrentViewMode = ViewMode.Grid;
                ViewModeButtonIcon.Glyph = "\uF0E2";
                ViewModeButton.Label = "Grid View";
            }
            else
            {
                lvFiles.ItemTemplate = GridViewModeItemTemplate;
                lvFiles.ItemContainerStyle = GridViewModeListViewItemStyle;
                lvFiles.ItemsPanel = GridViewModeItemsPanelTemplate;
                CurrentViewMode = ViewMode.List;
                ViewModeButtonIcon.Glyph = "\uEA37";
                ViewModeButton.Label = "List View";
            }
        }

        void UpdateFolderPathTextBlock()
        {
            string str = "Root";
            foreach(var item in folderStack)
            {
                str += $"/{item.Name}";
            }
            FolderPathTextBlock.Text = str;
        }

        private async void FileSavePickerUI_TargetFileRequested(FileSavePickerUI sender, TargetFileRequestedEventArgs args)
        {
            var deferral = args.Request.GetDeferral();
            StorageFolder folder = null;
            if (folderStack.Count != 0)
                folder = folderStack.Last();
            else
                folder = await StorageHelper.GetReceiveStoageFolderAsync();

            try
            {
                // Create an empty file at the specified location
                StorageFile file = await folder.CreateFileAsync(sender.FileName, CreationCollisionOption.GenerateUniqueName);

                // Set TargetFile, the caller of the "custom file save picker" will receive this object
                args.Request.TargetFile = file;
            }
            catch (Exception ex)
            {
                // Output exception information
                Debug.WriteLine(ex.ToString());
            }
            finally
            {
                // Complete the asynchronous operation
                deferral.Complete();
            }
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(fileOpenPickerUI != null)
            {
                // Remove list
                if (e.RemovedItems.Count > 0)
                {
                    if (fileOpenPickerUI.SelectionMode == FileSelectionMode.Multiple)
                    {
                        for (int i = 0; i < e.RemovedItems.Count; i++)
                        {
                            ViewStorageItem item = e.RemovedItems[i] as ViewStorageItem;
                            // Check if item exists before removing
                            if (fileOpenPickerUI.ContainsFile(item.Name))
                            {
                                fileOpenPickerUI.RemoveFile(item.Name);
                            }
                        }
                    }
                    else
                    {
                        ViewStorageItem item = e.RemovedItems[0] as ViewStorageItem;
                        if (fileOpenPickerUI.ContainsFile(item.Name))
                        {
                            fileOpenPickerUI.RemoveFile(item.Name);
                        }
                    }
                }

                // Add to list
                if (e.AddedItems.Count > 0)
                {
                    // If multi-select
                    if (fileOpenPickerUI.SelectionMode == FileSelectionMode.Multiple)
                    {
                        for (int i = 0; i < e.AddedItems.Count; i++)
                        {
                            ViewStorageItem item = e.AddedItems[i] as ViewStorageItem;
                            if (item.Item is StorageFile)
                            {
                                StorageFile file = (StorageFile)item.Item;
                                if (fileOpenPickerUI.CanAddFile(file))
                                {
                                    fileOpenPickerUI.AddFile(item.Name, file);
                                }
                            }
                            else if (item.Item is StorageFolder)
                            {
                                /*To-Do: Enter subfolder*/
                            }
                        }
                    }
                    else // If single select
                    {
                        ViewStorageItem item = e.AddedItems[0] as ViewStorageItem;
                        if (item.Item is StorageFile)
                        {
                            StorageFile file = (StorageFile)item.Item;
                            if (fileOpenPickerUI.CanAddFile(file))
                            {
                                fileOpenPickerUI.AddFile(item.Name, file);
                            }
                        }
                        else if (item.Item is StorageFolder)
                        {
                            /*To-Do: Enter subfolder*/
                        }
                    }
                }
            }
        }

        private async void lvFiles_ItemClick(object sender, ItemClickEventArgs e)
        {
            ViewStorageItem item = e.ClickedItem as ViewStorageItem;
            if (item.Item is StorageFolder)
            {
                StorageFolder folder = item.Item as StorageFolder;
                folderStack.Add(folder);
                await UpdateViewAsync(folder);
            }
            else if(item.Item is StorageFile && fileOpenPickerUI == null && fileSavePickerUI == null)
            {
                await Launcher.LaunchFileAsync(((ViewStorageItem)e.ClickedItem).Item as StorageFile);
            }
        }

        private async void BackButton_Click(object sender, RoutedEventArgs e)
        {
            folderStack.Remove(folderStack.Last());
            await UpdateViewAsync(await GetFolderOfCurrentViewAsync());
        }

        public async Task<StorageFolder> GetFolderOfCurrentViewAsync()
        {
            if (folderStack.Count <= 1)
            {
                return await StorageHelper.GetReceiveStoageFolderAsync();
            }
            StorageFolder folder = folderStack[folderStack.Count - 2];
            return folder;
        }

        private async void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            folderStack.Clear();
            await UpdateViewAsync(await StorageHelper.GetReceiveStoageFolderAsync());
        }

        private async void CreateFolderButton_Click(object sender, RoutedEventArgs e)
        {
            CreateNewFolderControl control = new CreateNewFolderControl();
            control.CancelButton.Click += (a, b) =>
            {
                ContentDialogManager.HideContentDialog();
            };
            control.OKButton.Click += async (a, b) =>
            {
                if (StringHelper.IsValidFileName(control.NameTextBox.Text))
                {
                    StorageFolder folder = null;
                    if (folderStack.Count != 0)
                        folder = folderStack.Last();
                    else
                        folder = await StorageHelper.GetReceiveStoageFolderAsync();

                    await folder.CreateFolderAsync(control.NameTextBox.Text);
                    ContentDialogManager.HideContentDialog();
                    await UpdateViewAsync(folder);
                }
                else
                {
                    control.NameTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                }
            };
            await ContentDialogManager.ShowContentDialogAsync(control);
        }

        IStorageItem RightTabedItem = null;

        private void lvFiles_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            ViewStorageItem item = (e.OriginalSource as FrameworkElement).DataContext as ViewStorageItem;
            if (item == null)
                RightTabedItem = null;
            else
                RightTabedItem = item.Item;
        }

        private async void ListViewFlyout_Open_Click(object sender, RoutedEventArgs e)
        {
            if (RightTabedItem is StorageFile)
            {
                await Launcher.LaunchFileAsync((StorageFile)RightTabedItem);
            }
            else if (RightTabedItem is StorageFolder)
            {
                StorageFolder folder = (StorageFolder)RightTabedItem;
                folderStack.Add(folder);
                await UpdateViewAsync(folder);
            }
        }

        private async void ListViewFlyout_OpenFilePath_Click(object sender, RoutedEventArgs e)
        {
            if (RightTabedItem is StorageFile)
            {
                StorageFile file = ((StorageFile)RightTabedItem);
                string str = file.Path.Substring(0, file.Path.Length - file.Name.Length);
                await Launcher.LaunchFolderPathAsync(str);
            }
            else if (RightTabedItem is StorageFolder)
            {
                await Launcher.LaunchFolderAsync((StorageFolder)RightTabedItem);
            }
        }

        private void ListViewFlyout_CopyFilePath_Click(object sender, RoutedEventArgs e)
        {
            DataPackage dataPackage = new DataPackage();
            if (RightTabedItem is StorageFile)
            {
                dataPackage.SetText(((StorageFile)RightTabedItem).Path);
                Clipboard.SetContent(dataPackage);
            }
            else if (RightTabedItem is StorageFolder)
            {
                dataPackage.SetText(((StorageFolder)RightTabedItem).Path);
                Clipboard.SetContent(dataPackage);
            }
        }

        private void ListViewFlyout_Opened(object sender, object e)
        {
            if (RightTabedItem == null)
            {
                ListViewFlyout.Hide();
                return;
            }
            if (RightTabedItem is StorageFile)
            {
                ListViewFlyout_OpenFilePath.Text = "Open File Location";
            }
            else if(RightTabedItem is StorageFolder)
            {
                ListViewFlyout_OpenFilePath.Text = "Open Folder Location";
            }

            if (ClipboardItem == null)
            {
                ListViewFlyout_Paste.Visibility = Visibility.Collapsed;
            }
            else
            {
                ListViewFlyout_Paste.Visibility = Visibility.Visible;
            }
        }

        private void ListViewFlyout_Cut_Click(object sender, RoutedEventArgs e)
        {
            CutMode = true;
            ClipboardItem = RightTabedItem;
        }

        IStorageItem ClipboardItem { get; set; }

        bool CutMode = false;

        private void ListViewFlyout_Copy_Click(object sender, RoutedEventArgs e)
        {
            CutMode = false;
            ClipboardItem = RightTabedItem;
        }

        private async void ListViewFlyout_Paste_Click(object sender, RoutedEventArgs e)
        {
            StorageFolder currentFolder = await GetFolderOfCurrentViewAsync();
            if (RightTabedItem is StorageFile)
            {
                StorageFile file = (StorageFile)RightTabedItem;
                if (CutMode)
                {
                    await file.MoveAsync(currentFolder);
                }
                else
                {
                    await file.CopyAsync(currentFolder);
                }
                await UpdateViewAsync(await GetFolderOfCurrentViewAsync());
            }
            else
            {
                ContentDialog contentDialog = new ContentDialog();
                contentDialog.CloseButtonText = "Close";
                contentDialog.Title = $"Unsupported Operation";
                contentDialog.Content = "Cut and copy operations are not supported for folders";
                await contentDialog.ShowAsync();
            }
        }

        private async void ListViewFlyout_Delete_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog contentDialog = new ContentDialog();
            contentDialog.Title = $"Are you sure you want to delete item \"{RightTabedItem.Name}\"?";
            contentDialog.PrimaryButtonText = "No";
            contentDialog.SecondaryButtonText = "Yes";
            var result = await contentDialog.ShowAsync();
            if (result == ContentDialogResult.Secondary)
            {
                if (RightTabedItem is StorageFile)
                {
                    await ((StorageFile)RightTabedItem).DeleteAsync();
                }
                else if (RightTabedItem is StorageFolder)
                {
                    await ((StorageFolder)RightTabedItem).DeleteAsync();
                }
                await UpdateViewAsync(await GetFolderOfCurrentViewAsync());
            }
        }

        private async void ListViewFlyout_Rename_Click(object sender, RoutedEventArgs e)
        {
            RenameStorageItemControl control = new RenameStorageItemControl();
            control.NameTextBox.Text = RightTabedItem.Name;
            control.CancelButton.Click += (a, b) =>
            {
                ContentDialogManager.HideContentDialog();
            };
            control.OKButton.Click += async (a, b) =>
            {
                if (StringHelper.IsValidFileName(control.NameTextBox.Text))
                {
                    if(RightTabedItem is StorageFile)
                    {
                        StorageFile storageFile = (StorageFile)RightTabedItem;
                        await storageFile.RenameAsync(control.NameTextBox.Text);
                    }
                    else if(RightTabedItem is StorageFolder)
                    {
                        StorageFolder storageFolder = (StorageFolder)RightTabedItem;
                        await storageFolder.RenameAsync(control.NameTextBox.Text);
                    }
                    await UpdateViewAsync(await GetFolderOfCurrentViewAsync());
                    ContentDialogManager.HideContentDialog();
                }
                else
                {
                    control.NameTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                }
            };
            await ContentDialogManager.ShowContentDialogAsync(control);
            await UpdateViewAsync(await GetFolderOfCurrentViewAsync());
        }

        private async void ListViewFlyout_Properties_Click(object sender, RoutedEventArgs e)
        {
            StorageItemPropertiesControl control = new StorageItemPropertiesControl(RightTabedItem);
            control.CancelButton.Click += (a, b) =>
            {
                ContentDialogManager.HideContentDialog();
            };
            await ContentDialogManager.ShowContentDialogAsync(control);
        }

        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            await UpdateViewAsync(await GetFolderOfCurrentViewAsync());
        }

        enum ViewMode
        {
            List,
            Grid
        }

        ViewMode CurrentViewMode = ViewMode.Grid;

        private void ViewModeButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateViewMode();
            Debug.WriteLine(CurrentViewMode.ToString());
            Settings.SetSetting(Settings.ExplorerPage_ViewMode, CurrentViewMode.ToString());
        }
    }
}
