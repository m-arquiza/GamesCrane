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
    public sealed partial class EditPage : Page
    {
        private EditViewModel viewModel;
        private Window m_window;
        private readonly ImageSelectionService _imageselectService;

        public EditPage()
        {
            this.InitializeComponent();
            viewModel = new EditViewModel();
            DataContext = viewModel;


            var appInstance = App.Current as App;
            //var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(appInstance.windowReference);
            m_window = appInstance.windowReference;


            _imageselectService = new ImageSelectionService();


        }

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

        private async void CheckDetails(object sender, RoutedEventArgs e)
        {
            if (!verifyPath())
            {
                FlyoutBase.ShowAttachedFlyout((FrameworkElement)PathBox);
            }
            else
            {
                if ((viewModel.GameTitle).Equals("Untitled Game") |
                    (viewModel.GameImagePath).Equals("ms-appx:///Assets/StarsBorder.png"))
                {
                    AddGameDetailsContentDialog dialog = new AddGameDetailsContentDialog();
                    if (m_window.Content is FrameworkElement fe)
                    {
                        dialog.XamlRoot = fe.XamlRoot;
                    }
                    ContentDialogResult result = await dialog.ShowAsync();

                    if (result == ContentDialogResult.Primary)
                    {
                        viewModel.SendDetails();
                    }
                }
                else
                {
                    viewModel.SendDetails();
                }
            }
        }


            private bool verifyPath()
            {

            //string programFilesFolder = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            //string filePath = Path.Combine(programFilesFolder, "Minecraft Launcher\\MinecraftLauncher.exe");
            string filePath = viewModel.GamePath.Trim('"');
            //if (File.Exists(filePath))
            //{
            string fileExtension = Path.GetExtension(filePath);
                if (string.Equals(fileExtension, ".exe", StringComparison.OrdinalIgnoreCase))
                {
                    if (filePath.IndexOfAny(Path.GetInvalidPathChars()) == -1)
                    {
                        Debug.WriteLine("all good");
                        return true;
                    }
                    else
                    {
                        Debug.WriteLine("invalid chars");
                    }
                }
                else
                {
                    Debug.WriteLine("doesn't end in exe");
                }
            //}
            //else
            //{
            //    Debug.WriteLine("file no exist");
            //}
            return false;
        }
    }
}
