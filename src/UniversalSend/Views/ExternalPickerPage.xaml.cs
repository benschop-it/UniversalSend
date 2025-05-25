using System;
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

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace UniversalSend.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ExternalPickerPage : Page
    {
        public ExternalPickerPage()
        {
            this.InitializeComponent();
        }

        FileOpenPickerUI fileOpenPickerUI;

        FileSavePickerUI fileSavePickerUI;

        List<StorageFolder> folderStack = new List<StorageFolder>();

        List<string> AllowedFileTypes;

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            Settings.InitUserSettings();

            if(e.Parameter is FileOpenPickerUI)
            {
                // 获取参数
                fileOpenPickerUI = e.Parameter as FileOpenPickerUI;
                // 获取本地文件列表
                AllowedFileTypes = fileOpenPickerUI.AllowedFileTypes.ToList();



            }
            else if(e.Parameter is FileSavePickerUI)
            {
                fileSavePickerUI = e.Parameter as FileSavePickerUI;
                AllowedFileTypes = fileSavePickerUI.AllowedFileTypes.ToList();
                fileSavePickerUI.TargetFileRequested += FileSavePickerUI_TargetFileRequested;
            }
            //List<IStorageItem> items = (await (await StorageHelper.GetReceiveStoageFolderAsync()).GetItemsAsync()).ToList();
            await UpdateViewAsync(await StorageHelper.GetReceiveStoageFolderAsync());
        }

        List<ViewStorageItem> viewItems = new List<ViewStorageItem>();

        async Task UpdateViewAsync(StorageFolder folder)
        {
            LoadingProgressBar.Visibility = Visibility.Visible;
            lvFiles.IsEnabled = false;
            List<IStorageItem> items = (await folder.GetItemsAsync()).ToList();
            BackButton.IsEnabled = folderStack.Count > 0 ? true : false;

            viewItems = new List<ViewStorageItem>();
            EmptyFolderTip.Visibility = Visibility.Collapsed;
            if (AllowedFileTypes.Contains("*"))
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
                // 在指定的地址新建一个没有任何内容的空白文件
                StorageFile file = await folder.CreateFileAsync(sender.FileName, CreationCollisionOption.GenerateUniqueName);

                // 设置 TargetFile，“自定义文件保存选取器”的调用端会收到此对象
                args.Request.TargetFile = file;
            }
            catch (Exception ex)
            {
                // 输出异常信息
                Debug.WriteLine(ex.ToString());
            }
            finally
            {
                // 完成异步操作
                deferral.Complete();
            }
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(fileOpenPickerUI != null)
            {
                // 移除列表
                if (e.RemovedItems.Count > 0)
                {
                    if (fileOpenPickerUI.SelectionMode == FileSelectionMode.Multiple)
                    {
                        for (int i = 0; i < e.RemovedItems.Count; i++)
                        {
                            ViewStorageItem item = e.RemovedItems[i] as ViewStorageItem;
                            // 移除前先判断是否存在目标项
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

                // 添加列表
                if (e.AddedItems.Count > 0)
                {
                    // 如果是多选
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
                                /*To-Do:进入子文件夹*/
                            }
                        }
                    }
                    else //如果是单选
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
                            /*To-Do:进入子文件夹*/
                        }

                    }
                }
            }else if(fileSavePickerUI != null)
            {
                
                //// 移除列表
                //if (e.RemovedItems.Count > 0)
                //{
                //    if (fileSavePickerUI. == FileSelectionMode.Multiple)
                //    {
                //        for (int i = 0; i < e.RemovedItems.Count; i++)
                //        {
                //            ViewStorageItem item = e.RemovedItems[i] as ViewStorageItem;
                //            // 移除前先判断是否存在目标项
                //            if (fileSavePickerUI.ContainsFile(item.Name))
                //            {
                //                fileSavePickerUI.RemoveFile(item.Name);
                //            }
                //        }
                //    }
                //    else
                //    {
                //        ViewStorageItem item = e.RemovedItems[0] as ViewStorageItem;
                //        if (fileSavePickerUI.ContainsFile(item.Name))
                //        {
                //            fileSavePickerUI.RemoveFile(item.Name);
                //        }
                //    }
                //}

                //// 添加列表
                //if (e.AddedItems.Count > 0)
                //{
                //    // 如果是多选
                //    if (fileSavePickerUI.SelectionMode == FileSelectionMode.Multiple)
                //    {
                //        for (int i = 0; i < e.AddedItems.Count; i++)
                //        {
                //            ViewStorageItem item = e.AddedItems[i] as ViewStorageItem;
                //            if (item.Item is StorageFile)
                //            {
                //                StorageFile file = (StorageFile)item.Item;
                //                if (fileSavePickerUI.CanAddFile(file))
                //                {
                //                    fileSavePickerUI.AddFile(item.Name, file);
                //                }
                //            }
                //            else if (item.Item is StorageFolder)
                //            {
                //                /*To-Do:进入子文件夹*/
                //            }
                //        }
                //    }
                //    else //如果是单选
                //    {
                //        ViewStorageItem item = e.AddedItems[0] as ViewStorageItem;
                //        if (item.Item is StorageFile)
                //        {
                //            StorageFile file = (StorageFile)item.Item;
                //            if (fileSavePickerUI.CanAddFile(file))
                //            {
                //                fileSavePickerUI.AddFile(item.Name, file);
                //            }
                //        }
                //        else if (item.Item is StorageFolder)
                //        {
                //            /*To-Do:进入子文件夹*/
                //        }

                //    }
                //}
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
        }

        private async void BackButton_Click(object sender, RoutedEventArgs e)
        {
            folderStack.Remove(folderStack.Last());
            if (folderStack.Count <= 1)
            {
                await UpdateViewAsync(await StorageHelper.GetReceiveStoageFolderAsync());
                return;
            }
            StorageFolder folder = folderStack[folderStack.Count - 2];
            await UpdateViewAsync(folder);
            
        }

        private async void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            await UpdateViewAsync(await StorageHelper.GetReceiveStoageFolderAsync());
        }

        //private async void CreateFolderButton_Click(object sender, RoutedEventArgs e)
        //{
        //    CreateNewFolderControl control = new CreateNewFolderControl();
        //    control.CancelButton.Click += (a, b) =>
        //    {
        //        ContentDialogManager.HideContentDialog();
        //    };
        //    control.OKButton.Click += async (a, b) =>
        //    {
        //        if(StringHelper.IsValidFileName(control.NameTextBox.Text))
        //        {
        //            StorageFolder folder = null;
        //            if (folderStack.Count != 0)
        //                folder = folderStack.Last();
        //            else
        //                folder = await StorageHelper.GetReceiveStoageFolderAsync();

        //            await folder.CreateFolderAsync(control.NameTextBox.Text);
        //            ContentDialogManager.HideContentDialog();
        //            await UpdateViewAsync(folder);
        //        }
        //        else
        //        {
        //            control.NameTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
        //        }
                    
        //    };
        //    await ContentDialogManager.ShowContentDialogAsync(control);
        //}

        private void Flyout_Opened(object sender, object e)
        {
            CreateFolderFlyoutGrid.Children.Clear();
            
            CreateNewFolderControl control = new CreateNewFolderControl();
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
                    CreateFolderFlyout.Hide();
                    await UpdateViewAsync(folder);
                }
                else
                {
                    control.NameTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                }

            };
            CreateFolderFlyoutGrid.Children.Add(control);
            
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
                ListViewFlyout_OpenFilePath.Text = "打开文件位置";
            }
            else if(RightTabedItem is StorageFolder)
            {
                ListViewFlyout_OpenFilePath.Text = "打开文件夹位置";
            }
        }
    }
}
