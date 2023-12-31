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
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace GamesCrane.View
{
    /// <summary>
    /// The <c>AddGameDetailsContentDialog</c> confirms (or enables modification for) that default game values (image and/or title) will be used.
    /// </summary>
    public sealed partial class AddGameDetailsContentDialog : ContentDialog
    {
        public AddGameDetailsContentDialog()
        {
            this.InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

    }
}
