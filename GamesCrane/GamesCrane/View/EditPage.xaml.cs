using GamesCrane.ViewModel;
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
    /// The <c>EditPage</c> is the page that enables the selection of add, edit, and remove functionality.
    /// </summary>
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
