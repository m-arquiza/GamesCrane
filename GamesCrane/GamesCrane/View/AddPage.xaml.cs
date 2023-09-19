using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;
using GamesCrane.ViewModel;
using Microsoft.UI.Xaml.Media.Imaging;
using System.ComponentModel;
using GamesCrane.Services;
using Windows.Storage;
using Windows.UI.Popups;
using System.Diagnostics;
using GamesCrane.Model;

namespace GamesCrane.View
{
    /// <summary>
    /// The <c>AddPage</c> is the page that holds all functionality for adding a new game.
    /// </summary>
    public sealed partial class AddPage : Page
    {
        private AddViewModel viewModel;
        private readonly ImageSelectionService _imageselectService;

        public AddPage()
        {
            this.InitializeComponent();
            viewModel = new AddViewModel();
            DataContext = viewModel;

            _imageselectService = new ImageSelectionService();
        }

        /// <summary>
        /// Enables user to select image then displays it for confirmation.
        /// <param name="sender">select button</param> 
        /// <param name="e">event data</param> 
        /// </summary>
        public async void SelectImage(object sender, RoutedEventArgs e)
        {
            try
            {
                await _imageselectService.SelectImageAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An exception occurred: {ex.Message}");
            }
            StorageFile image = _imageselectService.ImageSelected;
            if (image != null)
            {
                string selectedImage = image.Path;
                if (selectedImage != null)
                {
                    var bitmapImage = new BitmapImage();
                    bitmapImage.UriSource = new Uri(selectedImage);
                    SelectedImage.Source = bitmapImage;

                    viewModel.GameImagePath = selectedImage;
                    SelectedImagePath.Text = selectedImage;
                }
            }
        }

        /// <summary>
        /// Sets corresponding data based on user-input.
        /// <param name="sender">buttons, textboxes, checkboxes, etc.</param> 
        /// <param name="e">event data</param> 
        /// </summary>

        private void handleAdmin(object sender, RoutedEventArgs e)
        {
            viewModel.GameNeedsAdmin = !viewModel.GameNeedsAdmin;
        }

        private void handleFlags(object sender, RoutedEventArgs e)
        {
            viewModel.GamepathHasFlags = !viewModel.GamepathHasFlags;
        }

        private void EnableAdd(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox)sender;
            SendDetailsButton.IsEnabled = !string.IsNullOrEmpty(textBox.Text);
        }

        /// <summary>
        /// If shorcut path invalid, errors and informs user. 
        /// Otherwise, goes for additional error checking before adding to "machine".
        /// <param name="sender">send button</param> 
        /// <param name="e">event data</param> 
        /// </summary>
        private void VerifyAndSend(object sender, RoutedEventArgs e)
        {
            if (!viewModel.verifyPath())
            {
                FlyoutBase.ShowAttachedFlyout(PathBox);
            }
            else
            {
                viewModel.CheckDetails();
            }
        }
    }
}
