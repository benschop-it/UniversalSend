using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UniversalSend.Models;
using UniversalSend.Models.Data;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace UniversalSend.Controls.ContentDialogControls
{
    public sealed partial class EditFavoriteItemControl : UserControl
    {
        public Favorite Favorite { get;set; }
        public EditFavoriteItemControl(Favorite favorite)
        {
            this.InitializeComponent();
            Favorite = favorite;

            TitleTextBlock.Text = "编辑";
            DeviceNameTextBox.Text = Favorite.DeviceName;
            IPAddrTextBox.Text = Favorite.IPAddr;
            PortTextBox.Text = Favorite.Port.ToString();
        }

        public EditFavoriteItemControl()
        {
            this.InitializeComponent();
            TitleTextBlock.Text = "添加到收藏夹";
            DeleteButton.Visibility = Visibility.Collapsed;
        }

        public EditFavoriteItemControl(Device device)
        {
            this.InitializeComponent();
            TitleTextBlock.Text = "添加到收藏夹";
            DeleteButton.Visibility = Visibility.Collapsed;
            DeviceNameTextBox.Text = device.Alias;
            IPAddrTextBox.Text = device.IP;
            PortTextBox.Text = device.Port.ToString();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ProgramData.ContentDialogManager.HideContentDialog();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SolidColorBrush solidColorBrush = new SolidColorBrush(Colors.Red);
            /*To-Do:校验并保存*/
            if(String.IsNullOrEmpty(DeviceNameTextBox.Text))
            {
                DeviceNameTextBox.BorderBrush = solidColorBrush;
                return;
            }
            if(String.IsNullOrEmpty(IPAddrTextBox.Text) || !StringHelper.IsIpaddr(IPAddrTextBox.Text))
            {
                IPAddrTextBox.BorderBrush = solidColorBrush;
                return;
            }
            if(String.IsNullOrEmpty(PortTextBox.Text) && !PortTextBox.Text.All(char.IsDigit))
            {
                PortTextBox.BorderBrush = solidColorBrush;
                return;
            }else if(String.IsNullOrEmpty(PortTextBox.Text))
            {
                PortTextBox.Text = "53317";
            }

            if (Favorite == null)
            {
                Favorite = new Favorite(DeviceNameTextBox.Text,IPAddrTextBox.Text,Convert.ToInt64(PortTextBox.Text));
                FavoriteManager.Favorites.Add(Favorite);
                FavoriteManager.SaveFavoritesData();
            }
            else
            {
                Favorite.DeviceName = DeviceNameTextBox.Text;
                Favorite.IPAddr = IPAddrTextBox.Text;
                Favorite.Port = Convert.ToInt64(PortTextBox.Text);
                FavoriteManager.SaveFavoritesData();
            }
            ProgramData.ContentDialogManager.HideContentDialog();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            FavoriteManager.Favorites.Remove(Favorite);
            ProgramData.ContentDialogManager.HideContentDialog();
        }
    }
}
