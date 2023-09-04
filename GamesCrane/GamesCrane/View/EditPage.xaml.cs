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

namespace GamesCrane.View
{
    public sealed partial class EditPage : Page
    {
        private EditViewModel viewModel;

        public EditPage()
        {
            this.InitializeComponent();
            viewModel = new EditViewModel();
            DataContext = viewModel;
        }

    }
}
