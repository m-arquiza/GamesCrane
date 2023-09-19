using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using Windows.Storage;
//using GamesCrane.Interop;
using Microsoft.UI.Xaml.Controls;
using GamesCrane;
using System.Diagnostics;

namespace GamesCrane.Services
{
    /// <summary>
    /// The <c>ImageSelectionService</c> is used to open a file selector and enable image selection.
    /// </summary>
    public class ImageSelectionService
    {
        public StorageFile ImageSelected;

        /// <summary>
        /// Opens the file selector for an image and records the file.
        /// </summary>
        public async Task SelectImageAsync()
        {
            try
            {
                FileOpenPicker openPicker = new FileOpenPicker();

                var appInstance = App.Current as App;
                var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(appInstance.windowReference);

                WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hwnd);

                openPicker.ViewMode = PickerViewMode.Thumbnail;
                openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                openPicker.FileTypeFilter.Add(".jpg");
                openPicker.FileTypeFilter.Add(".jpeg");
                openPicker.FileTypeFilter.Add(".png");
                openPicker.FileTypeFilter.Add(".bmp");

                ImageSelected = await openPicker.PickSingleFileAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An exception occurred: {ex.Message}");
            }

        }
    }
}
