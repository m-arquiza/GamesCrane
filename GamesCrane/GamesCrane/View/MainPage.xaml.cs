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
    /// <summary>
    /// The <c>MainPage</c> is the page that models the vending machine itself.
    /// </summary>
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

        /// <summary>
        /// Handles special navigation: if a state is specified, will switch app into that state; 
        /// otherwise if a game is specified, adds a new game to the "machine".
        /// <param name="e">optional parameter that will either hold a state to switch to or a game to add</param> 
        /// </summary>
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

        /// <summary>
        /// Updates the corresponding XML image of the given game to the given game's image.
        /// <param name="newGame">game with the image to update</param> 
        /// </summary>
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
        /// <summary>
        /// Helper function to update the given XML image from the given image path.
        /// <param name="image">image to update</param> 
        /// <param name="path">new image source</param> 
        /// </summary>
        private void UpdateImageHelper(Image image, string path)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.UriSource = new Uri(path);
            image.Source = bitmapImage;
        }

        /// <summary>
        /// Updates all XML images to match their corresponding game information.
        /// </summary>
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

        /// <summary>
        /// After shifting all games down after game deletion, makes sure the last game's image is also deleted.
        /// </summary>
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

        /// <summary>
        /// Shows game in viewport onclick if not in an active switch.
        /// <param name="sender">image to show</param> 
        /// <param name="e">event data</param> 
        /// </summary>
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

        /// <summary>
        /// If default state, launches game. On remove state, removes game. 
        /// On switch state and actively switching, switch game with currently vended game.
        /// <param name="sender">image to show</param> 
        /// <param name="e">event data</param> 
        /// </summary>
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


        /// <summary>
        /// Shows specified game information at viewport.
        /// <param name="sender">game to show</param> 
        /// <param name="e">event data</param> 
        /// </summary>
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

        /// <summary>
        /// Removes game from viewport.
        /// <param name="sender">overall machine clickable bounds</param> 
        /// <param name="e">event data</param> 
        /// </summary>
        private void UnVend(object sender, TappedRoutedEventArgs e)
        {
            if (vended)
            {
                RemoveFromPort();
                vended = false;
            }
        }

        /// <summary>
        /// Sets viewport image and title to nothing.
        /// </summary>
        private void RemoveFromPort()
        {
            Image vendImage = (Image)FindName("GameSelectedImage");
            vendImage.Source = null;

            TextBlock vendTitle = (TextBlock)FindName("GameSelectedTitle");
            vendTitle.Text = "";
        }

        /// <summary>
        /// Sets vended game to the image that was clicked.
        /// <param name="gameImage">image to get game from</param> 
        /// <returns> Game from image </returns>
        /// </summary>
        private Game SetVendedGameFromImage(Image gameImage)
        {
            string imageName = gameImage.Name;
            Game game = viewModel.GetGame(imageName);
            viewModel.VendedGame = new Game(game);

            return game;
        }

        /// <summary>
        /// Launches game from its executable path.
        /// <param name="sender">play button or doubleclicked image</param> 
        /// <param name="e">event data</param> 
        /// </summary>
        private void LaunchGame(object sender, RoutedEventArgs e)
        {
            bool success = viewModel.GameLaunch();
            if (!success)
            {
                FlyoutBase.ShowAttachedFlyout(PlayButton);
            }
        }


        /// <summary>
        /// If special state, show state instructions.
        /// <param name="sender">overall machine pointer movable bounds</param> 
        /// <param name="e">event data</param> 
        /// </summary>
        private void ShowFlyoutHandler(object sender, PointerRoutedEventArgs e)
        {
            if (flyoutShow == 1)
            {
                FlyoutBase.ShowAttachedFlyout(EditButton);
                flyoutShow = 0;
            }
        }

        /// <summary>
        /// Removes special state if in one.
        /// <param name="sender">keyboard</param> 
        /// <param name="e">event data</param> 
        /// </summary>
        private void KeyboardAccelerator_Invoked( KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs e)
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
