using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using UniversalSend.Controls.ContentDialogControls;
using UniversalSend.Models;
using UniversalSend.Models.Helpers;
using UniversalSend.Models.Managers;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Pickers.Provider;
using Windows.System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace UniversalSend.Views {

    public sealed partial class ExplorerPage : Page {

        #region Private Fields

        private List<string> _allowedFileTypes;
        private ViewMode _currentViewMode = ViewMode.Grid;
        private bool _cutMode = false;
        private FileOpenPickerUI _fileOpenPickerUI;
        private FileSavePickerUI _fileSavePickerUI;
        private List<StorageFolder> _folderStack = new List<StorageFolder>();
        private IStorageItem _rightTabedItem = null;
        private List<ViewStorageItem> _viewItems = new List<ViewStorageItem>();

        #endregion Private Fields

        #region Private Enums

        private enum ViewMode {
            List,
            Grid
        }

        #endregion Private Enums

        #region Public Constructors

        public ExplorerPage() {
            InitializeComponent();
            string ViewMode = Settings.GetSettingContentAsString(Settings.ExplorerPage_ViewMode);
            if (ViewMode == "List") {
                _currentViewMode = ExplorerPage.ViewMode.List;
            } else {
                _currentViewMode = ExplorerPage.ViewMode.Grid;
            }

            UpdateViewMode();
        }

        #endregion Public Constructors

        #region Private Properties

        private IStorageItem ClipboardItem { get; set; }

        private ContentDialogManager ContentDialogManager { get; set; } = new ContentDialogManager();

        #endregion Private Properties

        #region Public Methods

        public async Task<StorageFolder> GetFolderOfCurrentViewAsync() {
            if (_folderStack.Count <= 1) {
                return await StorageHelper.GetReceiveStorageFolderAsync();
            }
            StorageFolder folder = _folderStack[_folderStack.Count - 2];
            return folder;
        }

        #endregion Public Methods

        #region Protected Methods

        protected override async void OnNavigatedTo(NavigationEventArgs e) {
            Settings.InitUserSettings();
            UpdateViewMode();
            if (e.Parameter is FileOpenPickerUI) {
                // Get parameter
                _fileOpenPickerUI = e.Parameter as FileOpenPickerUI;
                // Get local file list
                _allowedFileTypes = _fileOpenPickerUI.AllowedFileTypes.ToList();
            } else if (e.Parameter is FileSavePickerUI) {
                _fileSavePickerUI = e.Parameter as FileSavePickerUI;
                _allowedFileTypes = _fileSavePickerUI.AllowedFileTypes.ToList();
                _fileSavePickerUI.TargetFileRequested += FileSavePickerUI_TargetFileRequested;
            }
            await UpdateViewAsync(await StorageHelper.GetReceiveStorageFolderAsync());
        }

        #endregion Protected Methods

        #region Private Methods

        private async void BackButton_Click(object sender, RoutedEventArgs e) {
            _folderStack.Remove(_folderStack.Last());
            await UpdateViewAsync(await GetFolderOfCurrentViewAsync());
        }

        private async void CreateFolderButton_Click(object sender, RoutedEventArgs e) {
            CreateNewFolderControl control = new CreateNewFolderControl();
            control.CancelButton.Click += (a, b) => {
                ContentDialogManager.HideContentDialog();
            };
            control.OKButton.Click += async (a, b) => {
                if (StringHelper.IsValidFileName(control.NameTextBox.Text)) {
                    StorageFolder folder = null;
                    if (_folderStack.Count != 0) {
                        folder = _folderStack.Last();
                    } else {
                        folder = await StorageHelper.GetReceiveStorageFolderAsync();
                    }

                    await folder.CreateFolderAsync(control.NameTextBox.Text);
                    ContentDialogManager.HideContentDialog();
                    await UpdateViewAsync(folder);
                } else {
                    control.NameTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                }
            };
            await ContentDialogManager.ShowContentDialogAsync(control);
        }

        private async void FileSavePickerUI_TargetFileRequested(FileSavePickerUI sender, TargetFileRequestedEventArgs args) {
            var deferral = args.Request.GetDeferral();
            StorageFolder folder = null;
            if (_folderStack.Count != 0) {
                folder = _folderStack.Last();
            } else {
                folder = await StorageHelper.GetReceiveStorageFolderAsync();
            }

            try {
                // Create an empty file at the specified location
                StorageFile file = await folder.CreateFileAsync(sender.FileName, CreationCollisionOption.GenerateUniqueName);

                // Set TargetFile, the caller of the "custom file save picker" will receive this object
                args.Request.TargetFile = file;
            } catch (Exception ex) {
                // Output exception information
                Debug.WriteLine(ex.ToString());
            } finally {
                // Complete the asynchronous operation
                deferral.Complete();
            }
        }

        private async void HomeButton_Click(object sender, RoutedEventArgs e) {
            _folderStack.Clear();
            await UpdateViewAsync(await StorageHelper.GetReceiveStorageFolderAsync());
        }

        private void ListViewFlyout_Copy_Click(object sender, RoutedEventArgs e) {
            _cutMode = false;
            ClipboardItem = _rightTabedItem;
        }

        private void ListViewFlyout_CopyFilePath_Click(object sender, RoutedEventArgs e) {
            DataPackage dataPackage = new DataPackage();
            if (_rightTabedItem is StorageFile) {
                dataPackage.SetText(((StorageFile)_rightTabedItem).Path);
                Clipboard.SetContent(dataPackage);
            } else if (_rightTabedItem is StorageFolder) {
                dataPackage.SetText(((StorageFolder)_rightTabedItem).Path);
                Clipboard.SetContent(dataPackage);
            }
        }

        private void ListViewFlyout_Cut_Click(object sender, RoutedEventArgs e) {
            _cutMode = true;
            ClipboardItem = _rightTabedItem;
        }

        private async void ListViewFlyout_Delete_Click(object sender, RoutedEventArgs e) {
            ContentDialog contentDialog = new ContentDialog();
            contentDialog.Title = $"Are you sure you want to delete item \"{_rightTabedItem.Name}\"?";
            contentDialog.PrimaryButtonText = "No";
            contentDialog.SecondaryButtonText = "Yes";
            var result = await contentDialog.ShowAsync();
            if (result == ContentDialogResult.Secondary) {
                if (_rightTabedItem is StorageFile) {
                    await ((StorageFile)_rightTabedItem).DeleteAsync();
                } else if (_rightTabedItem is StorageFolder) {
                    await ((StorageFolder)_rightTabedItem).DeleteAsync();
                }
                await UpdateViewAsync(await GetFolderOfCurrentViewAsync());
            }
        }

        private async void ListViewFlyout_Open_Click(object sender, RoutedEventArgs e) {
            if (_rightTabedItem is StorageFile) {
                await Launcher.LaunchFileAsync((StorageFile)_rightTabedItem);
            } else if (_rightTabedItem is StorageFolder) {
                StorageFolder folder = (StorageFolder)_rightTabedItem;
                _folderStack.Add(folder);
                await UpdateViewAsync(folder);
            }
        }

        private void ListViewFlyout_Opened(object sender, object e) {
            if (_rightTabedItem == null) {
                ListViewFlyout.Hide();
                return;
            }
            if (_rightTabedItem is StorageFile) {
                ListViewFlyout_OpenFilePath.Text = "Open File Location";
            } else if (_rightTabedItem is StorageFolder) {
                ListViewFlyout_OpenFilePath.Text = "Open Folder Location";
            }

            if (ClipboardItem == null) {
                ListViewFlyout_Paste.Visibility = Visibility.Collapsed;
            } else {
                ListViewFlyout_Paste.Visibility = Visibility.Visible;
            }
        }

        private async void ListViewFlyout_OpenFilePath_Click(object sender, RoutedEventArgs e) {
            if (_rightTabedItem is StorageFile) {
                StorageFile file = ((StorageFile)_rightTabedItem);
                string str = file.Path.Substring(0, file.Path.Length - file.Name.Length);
                await Launcher.LaunchFolderPathAsync(str);
            } else if (_rightTabedItem is StorageFolder) {
                await Launcher.LaunchFolderAsync((StorageFolder)_rightTabedItem);
            }
        }

        private async void ListViewFlyout_Paste_Click(object sender, RoutedEventArgs e) {
            StorageFolder currentFolder = await GetFolderOfCurrentViewAsync();
            if (_rightTabedItem is StorageFile) {
                StorageFile file = (StorageFile)_rightTabedItem;
                if (_cutMode) {
                    await file.MoveAsync(currentFolder);
                } else {
                    await file.CopyAsync(currentFolder);
                }
                await UpdateViewAsync(await GetFolderOfCurrentViewAsync());
            } else {
                ContentDialog contentDialog = new ContentDialog();
                contentDialog.CloseButtonText = "Close";
                contentDialog.Title = $"Unsupported Operation";
                contentDialog.Content = "Cut and copy operations are not supported for folders";
                await contentDialog.ShowAsync();
            }
        }

        private async void ListViewFlyout_Properties_Click(object sender, RoutedEventArgs e) {
            StorageItemPropertiesControl control = new StorageItemPropertiesControl(_rightTabedItem);
            control.CancelButton.Click += (a, b) => {
                ContentDialogManager.HideContentDialog();
            };
            await ContentDialogManager.ShowContentDialogAsync(control);
        }

        private async void ListViewFlyout_Rename_Click(object sender, RoutedEventArgs e) {
            RenameStorageItemControl control = new RenameStorageItemControl();
            control.NameTextBox.Text = _rightTabedItem.Name;
            control.CancelButton.Click += (a, b) => {
                ContentDialogManager.HideContentDialog();
            };
            control.OKButton.Click += async (a, b) => {
                if (StringHelper.IsValidFileName(control.NameTextBox.Text)) {
                    if (_rightTabedItem is StorageFile) {
                        StorageFile storageFile = (StorageFile)_rightTabedItem;
                        await storageFile.RenameAsync(control.NameTextBox.Text);
                    } else if (_rightTabedItem is StorageFolder) {
                        StorageFolder storageFolder = (StorageFolder)_rightTabedItem;
                        await storageFolder.RenameAsync(control.NameTextBox.Text);
                    }
                    await UpdateViewAsync(await GetFolderOfCurrentViewAsync());
                    ContentDialogManager.HideContentDialog();
                } else {
                    control.NameTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                }
            };
            await ContentDialogManager.ShowContentDialogAsync(control);
            await UpdateViewAsync(await GetFolderOfCurrentViewAsync());
        }

        private async void lvFiles_ItemClick(object sender, ItemClickEventArgs e) {
            ViewStorageItem item = e.ClickedItem as ViewStorageItem;
            if (item.Item is StorageFolder) {
                StorageFolder folder = item.Item as StorageFolder;
                _folderStack.Add(folder);
                await UpdateViewAsync(folder);
            } else if (item.Item is StorageFile && _fileOpenPickerUI == null && _fileSavePickerUI == null) {
                await Launcher.LaunchFileAsync(((ViewStorageItem)e.ClickedItem).Item as StorageFile);
            }
        }

        private void lvFiles_RightTapped(object sender, RightTappedRoutedEventArgs e) {
            ViewStorageItem item = (e.OriginalSource as FrameworkElement).DataContext as ViewStorageItem;
            if (item == null) {
                _rightTabedItem = null;
            } else {
                _rightTabedItem = item.Item;
            }
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (_fileOpenPickerUI != null) {
                // Remove list
                if (e.RemovedItems.Count > 0) {
                    if (_fileOpenPickerUI.SelectionMode == FileSelectionMode.Multiple) {
                        for (int i = 0; i < e.RemovedItems.Count; i++) {
                            ViewStorageItem item = e.RemovedItems[i] as ViewStorageItem;
                            // Check if item exists before removing
                            if (_fileOpenPickerUI.ContainsFile(item.Name)) {
                                _fileOpenPickerUI.RemoveFile(item.Name);
                            }
                        }
                    } else {
                        ViewStorageItem item = e.RemovedItems[0] as ViewStorageItem;
                        if (_fileOpenPickerUI.ContainsFile(item.Name)) {
                            _fileOpenPickerUI.RemoveFile(item.Name);
                        }
                    }
                }

                // Add to list
                if (e.AddedItems.Count > 0) {
                    // If multi-select
                    if (_fileOpenPickerUI.SelectionMode == FileSelectionMode.Multiple) {
                        for (int i = 0; i < e.AddedItems.Count; i++) {
                            ViewStorageItem item = e.AddedItems[i] as ViewStorageItem;
                            if (item.Item is StorageFile) {
                                StorageFile file = (StorageFile)item.Item;
                                if (_fileOpenPickerUI.CanAddFile(file)) {
                                    _fileOpenPickerUI.AddFile(item.Name, file);
                                }
                            } else if (item.Item is StorageFolder) {
                                /*To-Do: Enter subfolder*/
                            }
                        }
                    } else // If single select
                      {
                        ViewStorageItem item = e.AddedItems[0] as ViewStorageItem;
                        if (item.Item is StorageFile) {
                            StorageFile file = (StorageFile)item.Item;
                            if (_fileOpenPickerUI.CanAddFile(file)) {
                                _fileOpenPickerUI.AddFile(item.Name, file);
                            }
                        } else if (item.Item is StorageFolder) {
                            /*To-Do: Enter subfolder*/
                        }
                    }
                }
            }
        }

        private async void RefreshButton_Click(object sender, RoutedEventArgs e) {
            await UpdateViewAsync(await GetFolderOfCurrentViewAsync());
        }

        private void UpdateFolderPathTextBlock() {
            string str = "Root";
            foreach (var item in _folderStack) {
                str += $"/{item.Name}";
            }
            FolderPathTextBlock.Text = str;
        }

        private async Task UpdateViewAsync(StorageFolder folder) {
            LoadingProgressBar.Visibility = Visibility.Visible;
            lvFiles.IsEnabled = false;
            UpdateFolderPathTextBlock();
            List<IStorageItem> items = (await folder.GetItemsAsync()).ToList();
            BackButton.IsEnabled = _folderStack.Count > 0 ? true : false;

            _viewItems = new List<ViewStorageItem>();
            EmptyFolderTip.Visibility = Visibility.Collapsed;
            if (_allowedFileTypes == null || _allowedFileTypes.Contains("*")) {
                foreach (var item in items) {
                    _viewItems.Add(new ViewStorageItem(item));
                }
            } else {
                foreach (var item in items) {
                    if (item is StorageFile && !_allowedFileTypes.Contains(((StorageFile)item).FileType.ToLower())) {
                        continue;
                    }
                    _viewItems.Add(new ViewStorageItem(item));
                }
            }

            if (_viewItems.Count == 0) {
                EmptyFolderTip.Visibility = Visibility.Visible;
            }

            await Task.Delay(100);
            lvFiles.ItemsSource = _viewItems;
            lvFiles.IsEnabled = true;
            LoadingProgressBar.Visibility = Visibility.Collapsed;
        }

        private void UpdateViewMode() {
            if (_currentViewMode == ViewMode.List) {
                lvFiles.ItemTemplate = ListViewModeItemTemplate;
                lvFiles.ItemContainerStyle = ListViewModeListViewItemStyle;
                lvFiles.ItemsPanel = ListViewModeItemsPanelTemplate;
                _currentViewMode = ViewMode.Grid;
                ViewModeButtonIcon.Glyph = "\uF0E2";
                ViewModeButton.Label = "Grid View";
            } else {
                lvFiles.ItemTemplate = GridViewModeItemTemplate;
                lvFiles.ItemContainerStyle = GridViewModeListViewItemStyle;
                lvFiles.ItemsPanel = GridViewModeItemsPanelTemplate;
                _currentViewMode = ViewMode.List;
                ViewModeButtonIcon.Glyph = "\uEA37";
                ViewModeButton.Label = "List View";
            }
        }

        private void ViewModeButton_Click(object sender, RoutedEventArgs e) {
            UpdateViewMode();
            Debug.WriteLine(_currentViewMode.ToString());
            Settings.SetSetting(Settings.ExplorerPage_ViewMode, _currentViewMode.ToString());
        }

        #endregion Private Methods
    }
}