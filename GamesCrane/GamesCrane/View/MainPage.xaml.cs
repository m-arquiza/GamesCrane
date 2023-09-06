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
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;


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
            if (e.Parameter is Dictionary<string, object>)
            {
                viewModel.NewGame = (Dictionary<string, object>) e.Parameter;
                UpdateImage(viewModel.NewGame);
            }
        }

        public void UpdateImage(Dictionary<string, object> newGame)
        {
            int picIndex = (int) newGame["numIndex"];
            string imageName = $"GameImage{picIndex}";

            Image image = (Image)FindName(imageName);

            if (image != null)
            {
                var bitmapImage = new BitmapImage();
                bitmapImage.UriSource = new Uri((string)newGame["image"]);
                image.Source = bitmapImage;
            }
            else
            {
                Console.WriteLine("Image index not found!");
            }

        }

        private void Game_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (sender is Image clickedImage)
            {
                string imageName = clickedImage.Name;
                string path = viewModel.HandleGameTap(imageName);

                var bitmapImage = new BitmapImage();
                bitmapImage.UriSource = new Uri(path);

                Image vendImage = (Image)FindName("GameSelected");
                vendImage.Source = bitmapImage;

            }
        }

        private void Game_DoubleTapped(object sender, TappedRoutedEventArgs e)
        {
            Game_Tapped(sender, e);
        }

    }

}
