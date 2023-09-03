using Microsoft.UI.Xaml;
using System.Diagnostics;
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
using Microsoft.UI.Windowing;

namespace GamesCrane.View
{
    public sealed partial class MainWindow : Window
    {
        private OverlappedPresenter _presenter;

        public MainWindow()
        {
            this.InitializeComponent();

            // Resizes and positions window in center of screen
            IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);
            var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);
            
            if (appWindow is not null)
            {
                appWindow.Resize(new Windows.Graphics.SizeInt32 { Width = 1520, Height = 855 });

                Microsoft.UI.Windowing.DisplayArea displayArea =
                    Microsoft.UI.Windowing.DisplayArea.GetFromWindowId(windowId,
                    Microsoft.UI.Windowing.DisplayAreaFallback.Nearest);

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

    }
}
