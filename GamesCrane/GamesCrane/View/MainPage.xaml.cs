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
using System.Threading.Tasks;

namespace GamesCrane.View
{
    public sealed partial class MainPage : Page
    {
        private MainViewModel viewModel;
        private string[,] gameImages;
        private bool needsInitialUpdate;
        public MainPage()
        {
            this.InitializeComponent();
            viewModel = new MainViewModel();
            DataContext = viewModel;

            gameImages = new string[3,5];
            needsInitialUpdate = true;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (needsInitialUpdate)
            {
                while (!viewModel.IsDataLoaded)
                {
                    await Task.Delay(100);
                }
                Game[,] games = viewModel.Games;
                for (int row = 0; row < 3; row++)
                {
                    for (int column = 0; column < 5; column++)
                    {
                        Game game = games[row, column];
                        string image = gameImages[row, column];
                        if (game != null && string.IsNullOrEmpty(image))
                        {
                            gameImages[row, column] = game.ImagePath;
                            UpdateImage(game);
                        }
                    }
                }
                needsInitialUpdate = false;
            }
            
            
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
