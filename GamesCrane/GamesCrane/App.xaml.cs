using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using GamesCrane.Services;
using GamesCrane.Model;
using System.Diagnostics;
using Windows.ApplicationModel.Core;


namespace GamesCrane
{
    public partial class App : Application
    {
        public static Frame RootFrame { get; private set; }

        public App()
        {
            this.InitializeComponent();
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            m_window = new MainWindow();

            Frame rootFrame = new Frame();
            rootFrame.NavigationFailed += OnNavigationFailed;

            RootFrame = rootFrame;

            rootFrame.Navigate(typeof(View.MainPage), args.Arguments);

            m_window.Content = rootFrame;
            m_window.Activate();

        }

        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private Window m_window;

        public Window windowReference { get { return m_window; } }

    }
}
