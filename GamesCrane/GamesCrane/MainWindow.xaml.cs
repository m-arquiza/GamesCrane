using GamesCrane.Model;
using GamesCrane.Services;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace GamesCrane
{
    public sealed partial class MainWindow : Window
    {
        private OverlappedPresenter _presenter;
        public MainWindow()
        {
            this.InitializeComponent();
            //this.Closed += MainWindow_Closed;

            // Resizes and positions window in center of screen
            IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);
            var appWindow = AppWindow.GetFromWindowId(windowId);

            if (appWindow is not null)
            {
                appWindow.Resize(new Windows.Graphics.SizeInt32 { Width = 1520, Height = 855 });

                DisplayArea displayArea =
                DisplayArea.GetFromWindowId(windowId, DisplayAreaFallback.Nearest);

                if (displayArea is not null)
                {
                    var CenteredPosition = appWindow.Position;
                    CenteredPosition.X = ((displayArea.WorkArea.Width - appWindow.Size.Width) / 2);
                    CenteredPosition.Y = ((displayArea.WorkArea.Height - appWindow.Size.Height) / 2);
                    appWindow.Move(CenteredPosition);
                }
            }

            _presenter = appWindow.Presenter as OverlappedPresenter;
            _presenter.IsResizable = false;
        }

        //public Game[,] Games { get; set; }

        //private async void MainWindow_Closed(object sender, WindowEventArgs e)
        //{
        //    Debug.WriteLine("suspending");
        //    var deferral = e.SuspendingOperation.GetDeferral();

        //    await AppStateManagerService.SaveAppStateAsync(Games);

        //    deferral.Complete();
        //}
    }
}
