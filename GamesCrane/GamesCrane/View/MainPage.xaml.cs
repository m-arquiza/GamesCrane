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
using Windows.Storage;
using Windows.UI.Core;
using System.ComponentModel;
using Windows.System;

namespace GamesCrane.View
{
    public sealed partial class MainPage : Page
    {
        private MainViewModel viewModel;
        private bool vended;
        private bool selected;
        private bool needsInitialUpdate;
        private string state;
        private Image background;
        private Button edit;
        private Button play;
        private int flyoutShow;
        public MainPage()
        {
            this.InitializeComponent();
            viewModel = new MainViewModel();
            DataContext = viewModel;

            vended = false;
            selected = false;

            needsInitialUpdate = true;
            state = viewModel.State;

            background = (Image)FindName("BackgroundImage");
            edit = (Button)FindName("EditButton");
            play = (Button)FindName("PlayButton");

            flyoutShow = -1;

            Container.PointerMoved += ShowFlyoutHandler;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (needsInitialUpdate)
            {
                while (!viewModel.IsDataLoaded)
                {
                    await Task.Delay(100);
                }
                UpdateImagesFromGames();
                needsInitialUpdate = false;
            }
            
            else if(e.Parameter is string && e.Parameter.Equals("remove"))
            {
                Container.Focus(FocusState.Programmatic);
                UpdateImageHelper(background, "ms-appx:///Assets/MainPageBackground_Removing.png");

                TextBlock flyText = (TextBlock)FindName("StateText");
                flyText.Text = "Click game to verify correct game is chosen (optional). Double-click to remove.";
                
                edit.IsEnabled = false;
                play.IsEnabled = false;

                flyoutShow = 1;
                state = "remove";
                viewModel.State = "remove";
            }

            else if (e.Parameter is string && e.Parameter.Equals("switch"))
            {
                Container.Focus(FocusState.Programmatic);
                UpdateImageHelper(background, "ms-appx:///Assets/MainPageBackground_Switching.png");

                TextBlock flyText = (TextBlock)FindName("StateText");
                flyText.Text = "Click game to select game to swap to a different spot. Double-click game to switch the games' spots.";

                edit.IsEnabled = false;
                play.IsEnabled = false;

                flyoutShow = 1;
                state = "switch";
                viewModel.State = "switch";
            }

            else if (e.Parameter is Game)
            {
                Game game = viewModel.AddGame((Game)e.Parameter);
                if (game != null)
                {
                    UpdateImage(game);
                } else
                {
                    FlyoutBase.ShowAttachedFlyout(VendPort);
                }
            }
        }

        public void UpdateImage(Game newGame)
        {
            int picIndex = newGame.NumIndex;
            string imageName = $"GameImage{picIndex}";
            Image image = (Image)FindName(imageName);

            if (image != null)
            {
                UpdateImageHelper(image, newGame.ImagePath);
            }
            else
            {
                Console.WriteLine("Image index not found!");
            }

        }

        private void UpdateImageHelper(Image image, string path)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.UriSource = new Uri(path);
            image.Source = bitmapImage;
        }

        private void UpdateImagesFromGames()
        {
            Game[,] games = viewModel.Games;
            for (int row = 0; row < 3; row++)
            {
                for (int column = 0; column < 5; column++)
                {
                    Game game = games[row, column];

                    if (game != null && !string.IsNullOrEmpty(game.ImagePath))
                    {
                        UpdateImage(game);
                    }

                }
            }
        }

        private void HandleLastImageAfterDelete()
        {
            if (viewModel.GamesCount == 0)
            {
                Image lastImage = (Image)FindName("GameImage1");
                lastImage.Source = null;
            } else
            {
                int lastIndex = viewModel.GamesCount + 1;
                string lastImageName = $"GameImage{lastIndex}";
                Image lastImage = (Image)FindName(lastImageName);
                lastImage.Source = null;

            }
        }

        private void Game_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (state.Equals("default") || state.Equals("remove"))
            {
                Vend(sender);
            }
            if (state.Equals("switch") && !selected)
            {
                Vend(sender);
                selected = true;
            }
            e.Handled = true;
        }

        private void Game_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            if (state.Equals("default"))
            {
                Vend(sender);
                LaunchGame(sender, (RoutedEventArgs)e);
            }
            else if (state.Equals("remove") && sender is Image removeImage)
            {
                Image vendImage = (Image)FindName("GameSelectedImage");
                vendImage.Source = null;

                TextBlock vendTitle = (TextBlock)FindName("GameSelectedTitle");
                vendTitle.Text = "";

                Game game = SetVendedGameFromImage(removeImage);
                RemoveFromPort();

                viewModel.DeleteGame(game);

                UpdateImagesFromGames();
                HandleLastImageAfterDelete();
            }
            else if (state.Equals("switch") && sender is Image switchImage)
            {
                string imageName = switchImage.Name;
                Game original = viewModel.GetGame(imageName);
                viewModel.SwapGame(original);

                RemoveFromPort();
                UpdateImagesFromGames();

                selected = false;
            }
            e.Handled = true;
        }


        private void Vend(object sender)
        {
            if (sender is Image clickedImage)
            {
                Game game = SetVendedGameFromImage(clickedImage);

                var bitmapImage = new BitmapImage();
                bitmapImage.UriSource = new Uri(game.ImagePath);

                Image vendImage = (Image)FindName("GameSelectedImage");
                vendImage.Source = bitmapImage;

                TextBlock vendTitle = (TextBlock)FindName("GameSelectedTitle");
                vendTitle.Text = game.Title;

                vended = true;
            }
        }

        private void UnVend(object sender, TappedRoutedEventArgs e)
        {
            if (vended)
            {
                RemoveFromPort();
                vended = false;
            }
        }
        
        private void RemoveFromPort()
        {
            Image vendImage = (Image)FindName("GameSelectedImage");
            vendImage.Source = null;

            TextBlock vendTitle = (TextBlock)FindName("GameSelectedTitle");
            vendTitle.Text = "";
        }

        private Game SetVendedGameFromImage(Image gameImage)
        {
            string imageName = gameImage.Name;
            Game game = viewModel.GetGame(imageName);
            viewModel.VendedGame = new Game(game);

            return game;
        }

        private void LaunchGame(object sender, RoutedEventArgs e)
        {
            bool success = viewModel.GameLaunch();
            if (!success)
            {
                FlyoutBase.ShowAttachedFlyout(PlayButton);
            }
        }

        private void ShowFlyoutHandler(object sender, PointerRoutedEventArgs e)
        {
            if (flyoutShow == 1)
            {
                FlyoutBase.ShowAttachedFlyout(EditButton);
                flyoutShow = 0;
            }
        }

        private void KeyboardAccelerator_Invoked( KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            if (state.Equals("remove") || state.Equals("switch")) 
            {
                UpdateImageHelper(background, "ms-appx:///Assets/MainPageBackground.png");

                state = "default";
                viewModel.State = "default";
                flyoutShow = -1;
                edit.IsEnabled = true;
                play.IsEnabled = true;
            }
        }

    }

}
