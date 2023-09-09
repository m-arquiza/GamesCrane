using GamesCrane.ViewModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;
using GamesCrane.Model;

namespace GamesCrane.View
{
    public sealed partial class MainPage : Page
    {
        private MainViewModel viewModel;
        private string[,] gameImages;

        public MainPage()
        {
            this.InitializeComponent();
            viewModel = new MainViewModel();
            DataContext = viewModel;

            gameImages = new string[3,5];
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is Game)
            {
                viewModel.NewGame = (Game) e.Parameter;
                UpdateImage(viewModel.NewGame);
            }
        }

        public void UpdateImage(Game newGame)
        {
            int picIndex = newGame.NumIndex;
            string imageName = $"GameImage{picIndex}";

            Image image = (Image)FindName(imageName);

            if (image != null)
            {
                var bitmapImage = new BitmapImage();
                bitmapImage.UriSource = new Uri(newGame.ImagePath);
                image.Source = bitmapImage;
            }
            else
            {
                Console.WriteLine("Image index not found!");
            }

        }

        private void Game_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Vend(sender);
        }

        private void Game_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            Vend(sender);
            LaunchGame(sender, (RoutedEventArgs) e);
        }

        private void Vend(object sender)
        {
            if (sender is Image clickedImage)
            {
                string imageName = clickedImage.Name;
                Game game = viewModel.GetGame(imageName);
                viewModel.VendedGame = new Game(game);

                var bitmapImage = new BitmapImage();
                bitmapImage.UriSource = new Uri(game.ImagePath);

                Image vendImage = (Image)FindName("GameSelectedImage");
                vendImage.Source = bitmapImage;

                TextBlock vendTitle = (TextBlock)FindName("GameSelectedTitle");
                vendTitle.Text = game.Title;
            }
        }

        private void LaunchGame(object sender, RoutedEventArgs e)
        {
            bool success = viewModel.GameLaunch();
            if (!success)
            {
                FlyoutBase.ShowAttachedFlyout(PlayButton);
            }
        }

    }

}
